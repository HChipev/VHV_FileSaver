using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using VHV_FileSaver.Data.Models;

namespace VHV_FileSaver.Data.ModelBuilding
{
    public class UserRolesConfigurator : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles");

            builder.HasOne(x => x.ModifiedBy)
            .WithMany(x => x.ModifiedUserRoles)
            .HasForeignKey(x => x.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}