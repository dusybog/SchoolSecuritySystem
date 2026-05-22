namespace SchoolSecuritySystem.Core.Enums
{
    /// <summary>
    /// 寄信批次/系所派送狀態列舉
    /// </summary>
    public enum DispatchStatus
    {
        /// <summary>
        /// 0 - 未寄送：初始建立狀態
        /// </summary>
        NotSent = 0,

        /// <summary>
        /// 10 - 寄送中：正在呼叫 Graph API 或等待伺服器回應
        /// </summary>
        Sending = 10,

        /// <summary>
        /// 20 - 寄送成功：已確認交給郵件伺服器
        /// </summary>
        Success = 20,

        /// <summary>
        /// 21 - 寄送失敗：發生 Exception 或郵件伺服器拒絕請求
        /// </summary>
        Failed = 21
    }
}