using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VHV_FileSaver.Data.Models;

namespace VHV_FileSaver.Data.ModelBuilding
{
    public class RoleConfigurator : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.Property(e => e.Name).HasMaxLength(255);

            builder.Property(e => e.NormalizedName).HasMaxLength(255);

            builder.HasOne(x => x.ModifiedBy)
            .WithMany(x => x.ModifiedRoles)
            .HasForeignKey(x => x.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}