using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        public SQLRegionRepository(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public DataContext dbContext { get; }

        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }
        public async Task<Region> GetByIdAsync(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> AddRegionAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteRegionAsync(Guid id)
        {
            var result = await dbContext.Regions.FindAsync(id);
            if (result == null)
            {
                return null;
            }
            dbContext.Regions.Remove(result);
            await dbContext.SaveChangesAsync();
            return result;
        }

        public async Task<Region> UpdateRegionAsync(Guid id, Region region)
        {
            var existingEntry = await dbContext.Regions.FindAsync(id);
            if (existingEntry == null)
            {
                return null;
            }

            existingEntry.Code = region.Code;
            existingEntry.Name = region.Name;
            existingEntry.RegionImageUrl = region.RegionImageUrl;
            await dbContext.SaveChangesAsync();
            return existingEntry;
        }
    }
}
