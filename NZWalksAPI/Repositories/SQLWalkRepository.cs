using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{

    public class SQLWalkRepository : IWalkRepository
    {
        public SQLWalkRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly DataContext _dbContext;

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _dbContext.Walks.AddAsync(walk);
            await _dbContext.SaveChangesAsync();
            return walk;
        }


        public async Task<List<Walk>> GetAllAsync(string? filterOn, string? filterQuery, string? sortBy, bool? isAscending, int pageNumber, int pageSize)
        {
            var walks = _dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //Filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                walks = walks.Where(w => EF.Property<string>(w, filterOn).Contains(filterQuery));
            }
            //sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {

                walks = isAscending == true ?
                    walks.OrderBy(w => EF.Property<string>(w, sortBy))
                    : walks.OrderByDescending(w => EF.Property<string>(w, sortBy))
                    ;
            }
            //pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingModel = await _dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
            if (existingModel == null) return null;
            existingModel.Name = walk.Name;
            existingModel.Description = walk.Description;
            existingModel.LengthInKm = walk.LengthInKm;
            existingModel.RegionId = walk.RegionId;
            existingModel.DifficultyId = walk.DifficultyId;
            await _dbContext.SaveChangesAsync();
            return walk;
        }
        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingModel = await _dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
            if (existingModel == null) return null;
            _dbContext.Walks.Remove(existingModel);
            await _dbContext.SaveChangesAsync();
            return existingModel;
        }
    }
}
