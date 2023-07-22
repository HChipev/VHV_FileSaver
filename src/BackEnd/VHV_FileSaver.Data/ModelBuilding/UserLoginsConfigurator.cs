using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VHV_FileSaver.Data.Models;

namespace VHV_FileSaver.Data.ModelBuilding
{
    public class UserLoginsConfigurator : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable("UserLogins");

            builder.HasOne(x => x.ModifiedBy)
            .WithMany(x => x.ModifiedUserLogins)
            .HasForeignKey(x => x.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}