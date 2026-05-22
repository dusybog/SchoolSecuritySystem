using SchoolSecuritySystem.Core.Dtos.Common;
using SchoolSecuritySystem.Core.Dtos.Dispatch;
using SchoolSecuritySystem.Core.Dtos.Submission;
using SchoolSecuritySystem.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolSecuritySystem.Core.Interfaces.Services
{
    public interface ISubmissionService
    {
        // ==========================================
        // 通報單主要業務邏輯
        // ==========================================
        Task<Result<long>> CreateAsync(CreateSubmissionDto createDto);

        Task<Result<PagedResultDto<SubmissionListDto>>> GetPagedListAsync(string queryType, int page, int pageSize);

        Task<Result<SubmissionDetailDto>> GetDetailAsync(long submissionId);

        Task<Result<bool>> AuditAndEditAsync(long submissionId, PatchSubmissionDto patchdto);

        // 供前端檢視特定版本資料
        Task<Result<CreateSubmissionDto>> GetVersionDetailAsync(long submissionId, int versionId);

        // ==========================================
        // 派發單管理
        // ==========================================
        Task<Result<IEnumerable<DispatchListDto>>> GetDispatchesAsync(long submissionId);

        Task<Result<bool>> CreateDispatchAsync(long submissionId);

        Task<Result<bool>> SignDispatchAsync(long submissionId, long dispatchId, SignDispatchDto dto);

        Task<Result<bool>> DeleteDispatchAsync(long submissionId, long dispatchId);

        Task<Result<bool>> SelectDispatchDepartmentsAsync(long submissionId, long dispatchId, List<int> departmentIds);

        Task<Result<byte[]>> GetDispatchReportPdfAsync(long submissionId, long dispatchId, string webRootPath);

        Task<Result<bool>> SendDispatchEmailsAsync(long submissionId, long dispatchId, string webRootPath);
    }
}