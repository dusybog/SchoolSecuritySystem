using SchoolSecuritySystem.Core.Entities;

namespace SchoolSecuritySystem.Core.Dtos.Submission
{
    public class SubmissionDetailDto
    {

        public long Id { get; set; }
        public string TraceCode { get; set; } = string.Empty;
        public short Status { get; set; }
        public int LatestVersion { get; set; }


        // ==========================================
        // 3. 版本紀錄清單 (對應前端的 appState.versions)
        // ==========================================
        public List<VersionSummaryDto> Versions { get; set; } = new();

        // ==========================================
        // 4. 流程表/簽核歷程 (對應前端的 appState.workflows)
        // ==========================================
        public List<WorkflowLogDto> Workflows { get; set; } = new();

        // ==========================================
        // 5. 派發紀錄 (對應前端的 appState.dispatches)
        // ==========================================
        //public List<DispatchRecordDto> Dispatches { get; set; } = new();
    }


    /// <summary>
    /// 版本摘要 (用於左側版本切換選單)
    /// </summary>
    public class VersionSummaryDto
    {
        public string V_Id { get; set; } = string.Empty; // 例: "v1", "v2"
        public DateTime SavedAt { get; set; }
        public string EditedBy { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
    }

    /// <summary>
    /// 流程與審核歷程
    /// </summary>
    public class WorkflowLogDto
    {
        public DateTime CreatedAt { get; set; }
        public string Actor { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;     // 例: "送出審核", "退回修改"
        public string Comment { get; set; } = string.Empty;    // 審核意見
    }

    /// <summary>
    /// 派發紀錄
    /// </summary>
    public class DispatchRecordDto
    {
        public long Id { get; set; }
        public string? DirectorSign { get; set; } = string.Empty;
        public DateTime? DirectorSignAt { get; set; }
        public string? OfficerSign { get; set; } = string.Empty;
        public DateTime? OfficerSignAt { get; set; }
        public List<string> Depts { get; set; } = new();       // 被派發的系所清單
        public short Status { get; set; }
    }

}