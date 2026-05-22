namespace SchoolSecuritySystem.Core.Interfaces.Services
{
    public interface ISerialNumberService
    {
        Task<string> GenerateTraceCodeAsync(DateTime targetDate);
    }
}
