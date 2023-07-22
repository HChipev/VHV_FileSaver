using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VHV_FileSaver.Data.Models;

namespace VHV_FileSaver.Data.ModelBuilding
{
    public class UserTokensConfigurator : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable("UserTokens");

            builder.Property(e => e.Name).HasMaxLength(255);

            builder.HasOne(x => x.ModifiedBy)
            .WithMany(x => x.ModifiedUserTokens)
            .HasForeignKey(x => x.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}