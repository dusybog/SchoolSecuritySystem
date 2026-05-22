using SchoolSecuritySystem.Core.Dtos.Department;
using SchoolSecuritySystem.Core.Models;


namespace SchoolSecuritySystem.Core.Interfaces.Services
{
    public interface IDepartmentService
    {
        Task<Result<IEnumerable<DepartmentListDto>>> GetListAsync();
        Task<Result<IEnumerable<DepartmentOptionDto>>> GetOptionsAsync();
        Task<Result<long>> CreateAsync(CreateDepartmentDto createDto);
        Task<Result<bool>> UpdateAsync(long departmentId, UpdateDepartmentDto dto);
        Task<Result<bool>> DeleteAsync(long departmentId);
    }
}