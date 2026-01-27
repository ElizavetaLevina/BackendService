using BackendService.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendService.DAL.Configurations
{
    public class ImageConfigurations : IEntityTypeConfiguration<ImageEntity>
    {
        public void Configure(EntityTypeBuilder<ImageEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Post).WithMany(c => c.Images);
        }
    }
}
