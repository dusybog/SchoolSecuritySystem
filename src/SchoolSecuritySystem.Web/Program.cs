using Microsoft.EntityFrameworkCore;
using SchoolSecuritySystem.Core;
using SchoolSecuritySystem.Core.Interfaces.Services;
using SchoolSecuritySystem.Core.Services;
using SchoolSecuritySystem.Infrastructure;
using SchoolSecuritySystem.Infrastructure.Data;
using SchoolSecuritySystem.Infrastructure.Services;
using SchoolSecuritySystem.Web.Extensions;
using SchoolSecuritySystem.Web.Services;
using SchoolSecuritySystem.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 依賴注入與系統核心設定區塊
// ==========================================

// 1. 註冊資料庫 DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    );
});

// 2. 註冊客製化 O365 驗證與權限控管 (傳入 Configuration)
builder.Services.AddO365Authentication(builder.Configuration);

// 3. 註冊客製化 MVC 與 Feature Folders 架構
builder.Services.AddCustomMvc();

// 4. DI 商業邏輯層與基礎設施層註冊
builder.Services.AddCoreServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddHostedService<GraphEmailBackgroundService>();

// 5. 從設定檔讀取 Base64 金鑰
var encryptionKey = builder.Configuration["EncryptionSettings:Aes256Key"];

if (string.IsNullOrEmpty(encryptionKey))
{ 
    throw new Exception("啟動失敗：找不到 AES 加密金鑰。");
}
// 註冊為 Singleton 服務
builder.Services.AddSingleton<IEncryptionService>(new AesGcmEncryptionService(encryptionKey));


// 6. 註冊 NSwag 服務
builder.Services.AddControllers();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "v1";
    config.Title = "API document";
    config.Version = "v1.0.0";
    config.Description = "";
});

builder.Services.Configure<HostOptions>(options =>
{
    options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});

var app = builder.Build();


// ==========================================
// HTTP Request Pipeline (中介軟體管線)
// ==========================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalAuditMiddleware>();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=submission}/{action=Create}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();