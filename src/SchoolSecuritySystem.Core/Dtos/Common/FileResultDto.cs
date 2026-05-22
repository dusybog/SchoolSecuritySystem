namespace SchoolSecuritySystem.Core.Dtos.Common
{
    /// <summary>
    /// 全系統通用的檔案下載回傳包裝物件
    /// </summary>
    public class FileResultDto
    {
        /// <summary>
        /// 檔案的二進位內容 (通常用 byte[] 最單純)
        /// </summary>
        public byte[] FileContent { get; set; }

        /// <summary>
        /// 檔案的 MIME 類型 (例如 "application/pdf" 或 "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 預設的下載檔案名稱 (包含副檔名，例如 "DispatchPreview_123.pdf")
        /// </summary>
        public string FileName { get; set; }
    }
}