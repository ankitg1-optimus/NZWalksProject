using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalksAPI.Data
{
    public class AuthDbContext: IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options): base(options){ }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "8e8feedb-aeac-4b20-99e9-be9e7c266faf";
            var writerRoleId = "e93e9643-87a1-41ec-a699-654eee983314";

            var roles = new List<IdentityRole>()
            {
                new IdentityRole
                {
                    Id = readerRoleId,ConcurrencyStamp= readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id = writerRoleId,ConcurrencyStamp=writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

        }
    }
}
