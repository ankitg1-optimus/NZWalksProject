using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
        Task<Region?> GetByIdAsync(Guid id);
        Task<Region> AddRegionAsync(Region region);
        Task<Region?> DeleteRegionAsync(Guid id);
        Task<Region?> UpdateRegionAsync(Guid id, Region region);        
    }
}
