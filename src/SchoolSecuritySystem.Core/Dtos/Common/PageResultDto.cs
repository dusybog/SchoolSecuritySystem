namespace SchoolSecuritySystem.Core.Dtos.Common
{
    /// <summary>
    /// 全系統通用的分頁回傳包裝物件
    /// </summary>
    /// <typeparam name="T">清單資料的型別 (例如 SubmissionListDto)</typeparam>
    public class PagedResultDto<T>
    {
        /// <summary>
        /// 實際的資料清單
        /// </summary>
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// 資料總筆數 (用於前端計算總頁數)
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 每頁顯示筆數
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 目前所在的頁碼
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 總頁數 (唯讀屬性，由系統自動計算)
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}