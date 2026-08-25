using MyBackend.Application.Contracts;
using MyBackend.Application.Interfaces;
using MyBackend.Domain.Interfaces;

namespace MyBackend.Application.Services
{
    /// <summary>
    /// Implements Designation catalog management and querying.
    /// </summary>
    public class DesignationService : IDesignationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDesignationRepository _designationRepository;

        public DesignationService(IUnitOfWork unitOfWork, IDesignationRepository designationRepository)
        {
            _unitOfWork = unitOfWork;
            _designationRepository = designationRepository;
        }

        public async Task<List<DesignationDto>> GetAllDesignationsAsync()
        {
            var designations = await _designationRepository.GetActiveDesignationsAsync();
            return designations.Select(d => new DesignationDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description ?? string.Empty,
                DeletedFlag = d.DeletedFlag
            }).ToList();
        }

        public async Task<DesignationDto?> GetDesignationByIdAsync(int id)
        {
            var designation = await _designationRepository.GetActiveDesignationByIdAsync(id);
            if (designation is null) return null;

            return new DesignationDto
            {
                Id = designation.Id,
                Name = designation.Name,
                Description = designation.Description ?? string.Empty,
                DeletedFlag = designation.DeletedFlag
            };
        }

        public async Task<DesignationDto> CreateDesignationAsync(CreateDesignationRequest request)
        {
            var trimmedName = request.Name.Trim();
            var existing = await _designationRepository.GetActiveDesignationsAsync();
            if (existing.Any(d => d.Name.Equals(trimmedName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A designation with the name '{trimmedName}' already exists.");
            }

            var designation = new MyBackend.Domain.Entities.Designation
            {
                Name = trimmedName,
                Description = request.Description?.Trim() ?? string.Empty,
                DeletedFlag = 1,
                CreatedAt = DateTime.UtcNow
            };

            await _designationRepository.AddAsync(designation);
            await _unitOfWork.SaveChangesAsync();

            return new DesignationDto
            {
                Id = designation.Id,
                Name = designation.Name,
                Description = designation.Description ?? string.Empty,
                DeletedFlag = designation.DeletedFlag
            };
        }
    }
}
