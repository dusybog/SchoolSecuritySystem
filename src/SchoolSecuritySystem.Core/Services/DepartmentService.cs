using SchoolSecuritySystem.Core.Dtos.Department;
using SchoolSecuritySystem.Core.Entities;
using SchoolSecuritySystem.Core.Interfaces.Repositories;
using SchoolSecuritySystem.Core.Interfaces.Services;
using SchoolSecuritySystem.Core.Models;

namespace SchoolSecuritySystem.Core.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<Result<IEnumerable<DepartmentListDto>>> GetListAsync()
        {
            var entities = await _departmentRepository.GetAllAsync();
            var dtos = entities.Select(x => new DepartmentListDto
            {
                Id = x.id,
                Name = x.name,
                Code = x.code,
                ContactEmail = x.contact_email
            });
            return Result<IEnumerable<DepartmentListDto>>.Success(dtos);
        }

        public async Task<Result<IEnumerable<DepartmentOptionDto>>> GetOptionsAsync()
        {
            var entities = await _departmentRepository.GetAllAsync();
            var dtos = entities.Select(x => new DepartmentOptionDto { Id = x.id, Name = x.name });
            return Result<IEnumerable<DepartmentOptionDto>>.Success(dtos);
        }

        public async Task<Result<long>> CreateAsync(CreateDepartmentDto createDto)
        {
            var entity = new department
            {
                name = createDto.Name,
                code = createDto.Code,
                contact_email = createDto.ContactEmail
            };
            await _departmentRepository.AddAsync(entity);
            return Result<long>.Success(entity.id);
        }

        public async Task<Result<bool>> UpdateAsync(long departmentId, UpdateDepartmentDto dto)
        {
            var department = await _departmentRepository.GetByIdAsync(departmentId);
            if (department == null)
                return Result<bool>.Failure(Error.NotFound($"找不到 ID 為 {departmentId} 的部門"));

            if (!string.IsNullOrWhiteSpace(dto.Name)) department.name = dto.Name;
            if (!string.IsNullOrWhiteSpace(dto.Code)) department.code = dto.Code;
            if (!string.IsNullOrWhiteSpace(dto.ContactEmail)) department.contact_email = dto.ContactEmail;

            await _departmentRepository.UpdateAsync(department);
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteAsync(long departmentId)
        {
            var department = await _departmentRepository.GetByIdAsync(departmentId);
            if (department == null)
                return Result<bool>.Failure(Error.NotFound($"找不到 ID 為 {departmentId} 的部門"));

            await _departmentRepository.DeleteAsync(department);
            return Result<bool>.Success(true);
        }
    }
}