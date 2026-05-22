using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolSecuritySystem.Core.Dtos.Dispatch
{
    public class DispatchListDto
    {
        public long Id { get; set; }
        public short Status { get; set; }
        public string? DirectorSign { get; set; }
        public string? DirectorSignAt { get; set; } // 轉成易讀的字串格式
        public string? OfficerSign { get; set; }
        public string? OfficerSignAt { get; set; }

        public List<DispatchSelectDto> Selects { get; set; } = new List<DispatchSelectDto>();

        public List<DispatchLogDto> Logs { get; set; } = new List<DispatchLogDto>();

    }
}
