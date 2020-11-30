using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace U.Game.Feedback.Repository.EntityConfigurations
{
    class UserEntityConfiguration : IEntityTypeConfiguration<Domain.Entities.User>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.User> builder)
        {
            builder
                .HasMany(uf => uf.UserFeedbacks)
                .WithOne(u => u.User);
        }
    }
}