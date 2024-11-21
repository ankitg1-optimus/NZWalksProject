using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{

    public class SQLWalkRepository : IWalkRepository
    {
        public SQLWalkRepository(DataContext dbContext) {
            _dbContext = dbContext;
        }

        private readonly DataContext _dbContext;

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _dbContext.Walks.AddAsync(walk);
            await _dbContext.SaveChangesAsync();
            return walk;
        }


        public async Task<List<Walk>> GetAllAsync()
        {
            return await _dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();          
        }
    }
}
