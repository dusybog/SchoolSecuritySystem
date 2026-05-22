namespace SchoolSecuritySystem.Core.Interfaces.Repositories
{
    public interface ISequenceRepository
    {
        Task<int> GetNextSequenceAsync(string datePart);
    }
}