using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using U.Game.Feedback.Repository.EntityConfigurations;

namespace U.Game.Feedback.Repository
{
    public class RepositoryDbContext : DbContext
    {
        public readonly IConfiguration configuration;

        public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options) : base(options)
        {
        }

        public DbSet<Domain.Entities.User> Users { get; set; }
        public DbSet<Domain.Entities.UserFeedback> UserFeedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserFeedbackEntityConfiguration());
        }
    }
}
