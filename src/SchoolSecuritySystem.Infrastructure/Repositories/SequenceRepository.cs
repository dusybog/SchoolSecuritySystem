using Microsoft.EntityFrameworkCore;
using SchoolSecuritySystem.Core.Interfaces.Repositories;
using SchoolSecuritySystem.Infrastructure.Data;
using SchoolSecuritySystem.Core.Entities;

namespace SchoolSecuritySystem.Infrastructure.Repositories
{
    public class SequenceRepository : ISequenceRepository
    {
        private readonly AppDbContext _db;

        public SequenceRepository(AppDbContext db) => _db = db;

        public async Task<int> GetNextSequenceAsync(string datePart)
        {
            // 開啟交易以確保併發時的資料一致性
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                // 1. 🌟 指定查詢「當天」的紀錄，並鎖定該列 (FOR UPDATE)
                // 使用 FromSqlInterpolated 可以安全地傳遞參數，防範 SQL Injection
                var tracker = await _db.submission_sequences
                    .FromSqlInterpolated($"SELECT * FROM submission_sequence WHERE date_part = {datePart} FOR UPDATE")
                    .FirstOrDefaultAsync();

                if (tracker == null)
                {
                    // 2. 如果今天還沒有人取號，就「新增」一筆今天的紀錄
                    tracker = new submission_sequence
                    {
                        date_part = datePart,
                        sequence = 1
                    };
                    await _db.submission_sequences.AddAsync(tracker);
                }
                else
                {
                    // 3. 如果今天已經有紀錄，單純將數字 +1
                    // 這裡絕對不碰 date_part (主鍵)，所以 EF Core 就不會再報錯了！
                    tracker.sequence += 1;
                }

                // 4. 儲存變更 (EF Core 會自動判斷是要發送 INSERT 還是 UPDATE)
                await _db.SaveChangesAsync();

                // 5. 提交交易，釋放鎖定
                await transaction.CommitAsync();

                return tracker.sequence;
            }
            catch
            {
                // 若發生任何意外 (例如兩個人在同一毫秒搶著 Insert)，則退回交易
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}