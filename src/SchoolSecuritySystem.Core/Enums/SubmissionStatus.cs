namespace SchoolSecuritySystem.Core.Enums
{
    /// <summary>
    /// 通報單流程狀態列舉
    /// </summary>
    public enum SubmissionStatus
    {
        /// <summary>
        /// 0 - 新建：表單剛建立，尚未進入審核流程
        /// </summary>
        Created = 0,

        /// <summary>
        /// 10 - 系所已審核：系所端完成初步核對，此階段內容仍允許修改
        /// </summary>
        DeptAudited = 10,

        /// <summary>
        /// 20 - 中心已審核：通報中心完成最後核對，此階段內容定版
        /// </summary>
        CenterAudited = 20,

        /// <summary>
        /// 30 - 結案：後續送審完成後，手動結案
        /// </summary>
        Closed = 30
    }
}