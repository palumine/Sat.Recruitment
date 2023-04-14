using Microsoft.EntityFrameworkCore;
using Sat.Recruitment.Domain;
using System.Reflection;

namespace Sat.Recruitment.Repositories.EF
{
    public class SatRecruitmentDBContext: DbContext 
    {
        public SatRecruitmentDBContext(DbContextOptions options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>(entity =>
                {
                    entity.HasKey(user => user.Id);
                }
            );

            base.OnModelCreating(modelBuilder);
        }

    }
}