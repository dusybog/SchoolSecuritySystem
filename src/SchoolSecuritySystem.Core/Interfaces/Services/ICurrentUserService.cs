// 📁 Core/Interfaces/Services/ICurrentUserService.cs
namespace SchoolSecuritySystem.Core.Interfaces.Services
{
    public interface ICurrentUserService
    {
        string Email { get; }
        string Role { get; }
        long DepartmentId { get; }
    }
}