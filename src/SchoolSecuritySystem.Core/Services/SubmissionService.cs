using SchoolSecuritySystem.Core.Constants;
using SchoolSecuritySystem.Core.Dtos.Common;
using SchoolSecuritySystem.Core.Dtos.Dispatch;
using SchoolSecuritySystem.Core.Dtos.Submission;
using SchoolSecuritySystem.Core.DTOs.Email;
using SchoolSecuritySystem.Core.Entities;
using SchoolSecuritySystem.Core.Enums;
using SchoolSecuritySystem.Core.Interfaces.Repositories;
using SchoolSecuritySystem.Core.Interfaces.Services;
using SchoolSecuritySystem.Core.Models;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;


namespace SchoolSecuritySystem.Core.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly ISerialNumberService _serialNumberService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IReportService _reportService;
        private readonly IEmailService _emailService;

        public SubmissionService(
            ISubmissionRepository submissionRepository,
            IEncryptionService encryptionService,
            ISerialNumberService serialNumberService,
            ICurrentUserService currentUserService,
            IReportService reportService,
            IEmailService emailService)
        {
            _submissionRepository = submissionRepository;
            _encryptionService = encryptionService;
            _serialNumberService = serialNumberService;
            _currentUserService = currentUserService;
            _reportService = reportService;
            _emailService = emailService;
        }

        // ==========================================
        // 業務邏輯實作
        // ==========================================
        public async Task<Result<long>> CreateAsync(CreateSubmissionDto createDto)
        {
            var now = DateTime.UtcNow;
            string traceCode = await _serialNumberService.GenerateTraceCodeAsync(now);
            string creatorId = _currentUserService.Email;

            var newSubmission = new submission
            {
                trace_code = traceCode,
                title = createDto.Title,
                reporter_name = createDto.Reporter,
                reporter_phone = createDto.Phone,
                department_id = createDto.Department,
                status = (short)SubmissionStatus.Created,
                created_by = creatorId,
                created_at = now,
                is_deleted = 0
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };

            string rawJsonString = JsonSerializer.Serialize(createDto, jsonOptions);
            byte[] plainDek = new byte[32];
            RandomNumberGenerator.Fill(plainDek);

            newSubmission.submission_versions.Add(new submission_version
            {
                version = 1,
                encrypted_content = _encryptionService.Encrypt(rawJsonString, plainDek),
                encrypted_dek = _encryptionService.Encrypt(Convert.ToBase64String(plainDek)),
                kek_version = 1,
                key_updated_at = now,
                created_by = creatorId,
                created_at = now
            });

            newSubmission.submission_workflows.Add(new submission_workflow
            {
                previous_status = (short)SubmissionStatus.Created,
                current_status = (short)SubmissionStatus.Created,
                comment = "案件建立",
                created_by = creatorId,
                created_at = now
            });

            await _submissionRepository.AddAsync(newSubmission);
            return Result<long>.Success(newSubmission.id);
        }

        public async Task<Result<PagedResultDto<SubmissionListDto>>> GetPagedListAsync(string queryType, int page, int pageSize)
        {
            string? exactCreatorEmail = null;
            List<short>? targetStatuses = null;

            // Service 決策業務過濾條件 (Repository 已在底層鋪好安全網，這裡只需給業務條件)
            if (queryType == "MySubmissions")
            {
                exactCreatorEmail = _currentUserService.Email;
            }
            else if (queryType == "Pending")
            {
                targetStatuses = _currentUserService.Role switch
                {
                    AppRoles.DepartmentOfficer => new List<short> { (short)SubmissionStatus.Created },
                    AppRoles.CenterDirector or AppRoles.CenterOfficer => new List<short> { (short)SubmissionStatus.DeptAudited, (short)SubmissionStatus.CenterAudited},
                    _ => new List<short>()
                };
            }
            else if (queryType == "Closed")
            {
                targetStatuses = new List<short> { (short)SubmissionStatus.Closed };
            }

            var (entities, totalCount) = await _submissionRepository.GetPagedEntitiesAsync(exactCreatorEmail, targetStatuses, page, pageSize);

            var dtos = entities.Select(e => new SubmissionListDto
            {
                Id = e.id,
                TraceCode = e.trace_code,
                Title = e.title,
                ReporterName = e.reporter_name,
                DepartmentName = e.department?.name ?? "未知部門",
                Status = e.status,
                CreatedBy = e.created_by,
                CreatedAt = e.created_at
            }).ToList();

            return Result<PagedResultDto<SubmissionListDto>>.Success(new PagedResultDto<SubmissionListDto>
            {
                Data = dtos,
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize
            });
        }

        public async Task<Result<SubmissionDetailDto>> GetDetailAsync(long submissionId)
        {
            var entity = await _submissionRepository.GetEntityWithDetailsAsync(submissionId);

            if (entity == null)
                return Result<SubmissionDetailDto>.Failure(Error.NotFound($"找不到追蹤碼為 {submissionId} 的通報單，或您無權限查看。"));

            var dto = new SubmissionDetailDto
            {
                Id = entity.id,
                TraceCode = entity.trace_code,
                Status = entity.status,
                LatestVersion = entity.submission_versions.Max(v => (int?)v.version) ?? 0,
                Versions = entity.submission_versions.OrderByDescending(v => v.created_at)
                    .Select(v => new VersionSummaryDto
                    {
                        V_Id = "v" + v.version,
                        CreatedAt = v.created_at,
                        CreatedBy = v.created_by,
                    }).ToList(),
                Workflows = entity.submission_workflows.OrderByDescending(w => w.created_at)
                    .Select(w => new WorkflowLogDto
                    {
                        CreatedAt = w.created_at,
                        CreatedBy = w.created_by,
                        Comment = w.comment
                    }).ToList(),
            };

            return Result<SubmissionDetailDto>.Success(dto);
        }

        public async Task<Result<bool>> AuditAndEditAsync(long submissionId, PatchSubmissionDto patchdto)
        {
            var submission = await _submissionRepository.GetByIdAsync(submissionId);

            if (submission == null)
                return Result<bool>.Failure(Error.NotFound("找不到指定的通報單"));

            short newStatus = submission.status;

            if (patchdto.Action == "Approve")
            {
                switch (_currentUserService.Role)
                {
                    case AppRoles.CenterDirector:
                    case AppRoles.CenterOfficer:
                        if (submission.status == 10) newStatus = 20;
                        if (submission.status == 20) newStatus = 30;
                        break;
                    case AppRoles.DepartmentOfficer:
                        if (submission.status == 0) newStatus = 10;
                        break;
                }

                if (newStatus == submission.status)
                    return Result<bool>.Failure(Error.Invalid("未知的操作動作或狀態不符"));
            }
            else
            {
                return Result<bool>.Failure(Error.Invalid("未知的操作動作"));
            }

            DateTime now = DateTime.UtcNow;

            // 處理表單內容更新
            if (patchdto.FormData != null)
            {
                int nextVersion = await _submissionRepository.GetMaxVersionAsync(submissionId) + 1;
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                };

                string rawJsonString = JsonSerializer.Serialize(patchdto.FormData, jsonOptions);
                byte[] plainDek = new byte[32];
                RandomNumberGenerator.Fill(plainDek);

                submission.submission_versions.Add(new submission_version
                {
                    version = nextVersion,
                    encrypted_content = _encryptionService.Encrypt(rawJsonString, plainDek),
                    encrypted_dek = _encryptionService.Encrypt(Convert.ToBase64String(plainDek)),
                    kek_version = 1,
                    key_updated_at = now,
                    created_by = _currentUserService.Email,
                    created_at = now
                });
            }

            submission.submission_workflows.Add(new submission_workflow
            {
                previous_status = submission.status,
                current_status = newStatus,
                comment = patchdto.Comment,
                created_by = _currentUserService.Email,
                created_at = now
            });

            submission.status = newStatus;
            await _submissionRepository.UpdateAsync(submission);

            return Result<bool>.Success(true);
        }

        // ==========================================
        // 派發單管理
        // ==========================================
        public async Task<Result<IEnumerable<DispatchListDto>>> GetDispatchesAsync(long submissionId)
        {
            var dispatches = await _submissionRepository.GetDispatchesBySubmissionIdAsync(submissionId);

            // 確保主單存在 (權限防呆)
            if (!dispatches.Any())
            {
                var submission = await _submissionRepository.GetByIdAsync(submissionId);
                if (submission == null)
                    return Result<IEnumerable<DispatchListDto>>.Failure(Error.NotFound("找不到通報單或權限不足"));
            }

            var dtoList = dispatches.Select(d => new DispatchListDto
            {
                Id = d.id,
                Status = d.status,
                DirectorSign = d.director_sign,
                DirectorSignAt = d.director_sign_at?.ToString("yyyy-MM-dd HH:mm"),
                OfficerSign = d.officer_sign,
                OfficerSignAt = d.officer_sign_at?.ToString("yyyy-MM-dd HH:mm"),

                Selects = d.dispatch_selects.Select(s => new DispatchSelectDto
                {
                    DeptName = s.department?.name ?? "未知系所",
                    ContactEmail = s.department?.contact_email ?? "未知信箱",
                    Status = s.status
                }).ToList(),

                Logs = d.dispatch_logs
                    .OrderByDescending(l => l.created_at)
                    .Select(l => new DispatchLogDto
                    {
                        Time = l.created_at.ToString("yyyy-MM-dd HH:mm:ss"),
                        Status = l.status,
                        Email = l.recipient_email,
                        Message = l.message,
                        Creator = l.created_by
                    }).ToList()
            });

            return Result<IEnumerable<DispatchListDto>>.Success(dtoList);
        }

        public async Task<Result<bool>> CreateDispatchAsync(long submissionId)
        {
            var submission = await _submissionRepository.GetByIdAsync(submissionId);
            if (submission == null) return Result<bool>.Failure(Error.NotFound("找不到通報單或權限不足"));
            if (submission.status < 20) return Result<bool>.Failure(Error.Forbidden("此通報單尚未進入派發階段！"));

            var newDispatch = new submission_dispatch
            {
                submission_id = submissionId,
                status = 0,
                total_count = 0,
                success_count = 0,
                created_by = _currentUserService.Email,
                created_at = DateTime.UtcNow
            };

            await _submissionRepository.AddDispatchAsync(newDispatch);
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> SignDispatchAsync(long submissionId, long dispatchId, SignDispatchDto dto)
        {
            var dispatch = await _submissionRepository.GetDispatchAsync(submissionId, dispatchId);
            if (dispatch == null) return Result<bool>.Failure(Error.NotFound("找不到派發單或您無權限操作"));
            if (dispatch.status != 0) return Result<bool>.Failure(Error.Invalid("此派發單已寄出，無法修改簽章！"));

            switch (_currentUserService.Role)
            {
                case AppRoles.CenterDirector:
                    dispatch.director_sign = dto.Name;
                    dispatch.director_sign_at = DateTime.UtcNow;
                    break;
                case AppRoles.CenterOfficer:
                    dispatch.officer_sign = dto.Name;
                    dispatch.officer_sign_at = DateTime.UtcNow;
                    break;
                default:
                    return Result<bool>.Failure(Error.Forbidden("無權進行此操作"));
            }

            await _submissionRepository.UpdateDispatchAsync(dispatch);
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteDispatchAsync(long submissionId, long dispatchId)
        {
            var dispatch = await _submissionRepository.GetDispatchAsync(submissionId, dispatchId);

            if (dispatch == null) return Result<bool>.Failure(Error.NotFound("找不到指定的派發單，或您無權限操作。"));
            if (dispatch.dispatch_logs != null && dispatch.dispatch_logs.Any())
                return Result<bool>.Failure(Error.Conflict("此派發單已產生寄送信件紀錄，禁止刪除！"));
            if (dispatch.status != 0)
                return Result<bool>.Failure(Error.Conflict("此派發單狀態為已寄出，無法刪除！"));

            _submissionRepository.DeleteDispatch(dispatch);
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> SelectDispatchDepartmentsAsync(long submissionId, long dispatchId, List<int> departmentIds)
        {
            var dispatch = await _submissionRepository.GetDispatchAsync(submissionId, dispatchId);
            if (dispatch == null) return Result<bool>.Failure(Error.NotFound("找不到派發單"));
            if (dispatch.dispatch_selects.Any()) return Result<bool>.Failure(Error.Conflict("此派發單已完成系所選擇，不可再變更名單！"));

            var newSelects = departmentIds.Select(deptId => new dispatch_select
            {
                dispatch_id = dispatchId,
                department_id = deptId,
                status = 0
            }).ToList();

            if (newSelects.Any())
            {
                await _submissionRepository.AddDispatchSelectsAsync(newSelects);
            }

            return Result<bool>.Success(true);
        }

        // ==========================================
        // 內部私有方法：取得版本與 PDF 
        // ==========================================
        // ==========================================
        // 取得特定版本的通報單詳細內容 (供前端讀取)
        // ==========================================
        public async Task<Result<CreateSubmissionDto>> GetVersionDetailAsync(long submissionId, int versionId)
        {
            // 呼叫底層的私有解密方法
            var formData = await GetVersionDetailInternalAsync(submissionId, versionId);

            if (formData == null)
            {
                return Result<CreateSubmissionDto>.Failure(Error.NotFound("找不到該版本資料，或您無權限查看。"));
            }

            return Result<CreateSubmissionDto>.Success(formData);
        }

        // ==========================================
        // 內部私有方法：取得版本與 PDF 
        // ==========================================
        private async Task<CreateSubmissionDto?> GetVersionDetailInternalAsync(long submissionId, int versionId)
        {
            var versionEntity = await _submissionRepository.GetVersionAsync(submissionId, versionId);
            if (versionEntity == null) return null;

            string base64Dek = _encryptionService.Decrypt(versionEntity.encrypted_dek);
            byte[] dekBytes = Convert.FromBase64String(base64Dek);
            string decryptedJsonString = _encryptionService.Decrypt(versionEntity.encrypted_content, dekBytes);

            var readJsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<CreateSubmissionDto>(decryptedJsonString, readJsonOptions);
        }

        public async Task<Result<byte[]>> GetDispatchReportPdfAsync(long submissionId, long dispatchId, string webRootPath)
        {
            var dispatch = await _submissionRepository.GetDispatchAsync(submissionId, dispatchId);
            if (dispatch == null) return Result<byte[]>.Failure(Error.NotFound("找不到對應的派發單資料"));

            var submission = await _submissionRepository.GetByIdAsync(submissionId);
            int maxVersion = await _submissionRepository.GetMaxVersionAsync(submissionId);

            var verData = await GetVersionDetailInternalAsync(submissionId, maxVersion);
            if (verData == null) return Result<byte[]>.Failure(Error.NotFound("找不到該案件的內容版本資料"));

            var jsonContent = JsonSerializer.SerializeToNode(verData);
            if (jsonContent is System.Text.Json.Nodes.JsonObject jsonObj && submission != null)
            {
                jsonObj["trace_code"] = submission.trace_code;
            }

            if (jsonContent == null) return Result<byte[]>.Failure(Error.Invalid("無法將通報資料轉換為報表所需的格式。"));

            DateTime currentPrintTime = DateTime.UtcNow;
            var pdfBytes = await _reportService.GenerateReportAsync(jsonContent, dispatch, webRootPath, currentPrintTime);
            return Result<byte[]>.Success(pdfBytes);
        }

        public async Task<Result<bool>> SendDispatchEmailsAsync(long submissionId, long dispatchId, string webRootPath)
        {
            var dispatch = await _submissionRepository.GetDispatchAsync(submissionId, dispatchId);
            if (dispatch == null) return Result<bool>.Failure(Error.NotFound("找不到派發單"));

            var submission = await _submissionRepository.GetByIdAsync(submissionId);
            if (submission == null) return Result<bool>.Failure(Error.NotFound("找不到通報單主檔"));

            if (dispatch.status == 20) return Result<bool>.Failure(Error.Conflict("此派發單已全數寄出，請勿重複操作。"));
            if (string.IsNullOrEmpty(dispatch.director_sign) || string.IsNullOrEmpty(dispatch.officer_sign))
                return Result<bool>.Failure(Error.Forbidden("雙方皆完成簽章後才可寄出通知信。"));
            if (dispatch.dispatch_selects == null || !dispatch.dispatch_selects.Any())
                return Result<bool>.Failure(Error.Invalid("請至少選擇一個接收系所。"));

            var pdfResult = await GetDispatchReportPdfAsync(submissionId, dispatchId, webRootPath);
            if (pdfResult.IsFailure) return Result<bool>.Failure(pdfResult.Error);

            string attachmentName = $"校安通報派發單_{submission.trace_code}.pdf";
            var attachments = new List<EmailAttachment>
            {
                new EmailAttachment
                {
                    FileName = attachmentName,
                    ContentType = "application/pdf",
                    Content = pdfResult.Value!
                }
            };

            foreach (var select in dispatch.dispatch_selects)
            {
                string targetEmail = select.department?.contact_email;
                if (!string.IsNullOrEmpty(targetEmail))
                {
                    string subject = $"【校安通報中心】案件派發通知 - 序號:{submission.trace_code}";
                    string body = $"單位主管您好：<br/><br/>隨信附上最新校安通報案件派發單，請查收附件。<br/><br/>※本件為密件，請妥慎保管資料，恪遵保密規定。<br/>教育部校園安全暨災害防救通報處理中心 敬上";

                    await _emailService.SendEmailAsync(targetEmail, subject, body, attachments, dispatchId, select.department_id);
                    select.status = 10; // 改為排隊中
                }
                else
                {
                    select.status = 21; // 失敗
                    dispatch.dispatch_logs.Add(new dispatch_log
                    {
                        dispatch_id = dispatchId,
                        recipient_email = select.department?.name ?? "未知單位",
                        status = 21,
                        message = "該單位未設定電子信箱，無法發送通知",
                        created_by = "系統排程",
                        created_at = DateTime.UtcNow
                    });
                }
            }

            dispatch.status = 10;
            await _submissionRepository.UpdateDispatchAsync(dispatch);

            return Result<bool>.Success(true);
        }
    }
}