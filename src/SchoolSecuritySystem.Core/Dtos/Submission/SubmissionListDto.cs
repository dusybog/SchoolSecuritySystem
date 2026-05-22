namespace SchoolSecuritySystem.Core.Dtos.Submission
{
    public class SubmissionListDto
    {
        public long Id { get; set; }

        public string TraceCode { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string ReporterName { get; set; } = string.Empty;

        public string DepartmentName { get; set; } = string.Empty;

        public short Status { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}

