using MyBackend.Application.Contracts;

namespace MyBackend.Application.Interfaces
{
    /// <summary>
    /// Service contract for managing workspace designations.
    /// </summary>
    public interface IDesignationService
    {
        /// <summary>
        /// Retrieves all active designations formatted as DesignationDto.
        /// </summary>
        Task<List<DesignationDto>> GetAllDesignationsAsync();

        /// <summary>
        /// Retrieves a designation by ID.
        /// </summary>
        Task<DesignationDto?> GetDesignationByIdAsync(int id);

        /// <summary>
        /// Creates and saves a new designation in the system.
        /// </summary>
        Task<DesignationDto> CreateDesignationAsync(CreateDesignationRequest request);
    }
}
