using SchoolSecuritySystem.Core.Entities;
using System.Text.Json.Nodes;

public interface IReportService
{
    Task<byte[]> GenerateReportAsync(JsonNode jsonContent, submission_dispatch SD, string webRootPath, DateTime printTime);
}