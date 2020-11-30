using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace U.Game.Feedback.Repository.EntityConfigurations
{
    class UserFeedbackEntityConfiguration : IEntityTypeConfiguration<Domain.Entities.UserFeedback>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.UserFeedback> builder)
        {
            builder
                .HasKey(k => k.Id);
        }
    }
}
