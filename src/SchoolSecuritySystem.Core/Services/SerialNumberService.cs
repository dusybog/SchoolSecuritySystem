using SchoolSecuritySystem.Core.Interfaces.Repositories;
using SchoolSecuritySystem.Core.Interfaces.Services;

namespace SchoolSecuritySystem.Core.Services
{
    public class SerialNumberService : ISerialNumberService
    {
        private readonly ISequenceRepository _sequenceRepo;

        public SerialNumberService(ISequenceRepository sequenceRepo)
        {
            _sequenceRepo = sequenceRepo;
        }

        public async Task<string> GenerateTraceCodeAsync(DateTime targetDate)
        {
            var datePart = targetDate.ToString("yyyyMMdd"); // 例如 "20260408"

            int nextSeq = await _sequenceRepo.GetNextSequenceAsync(datePart);

            return $"{datePart}-{nextSeq:D3}";
        }
    }
}