using BackendService.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendService.DAL.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<TagEntity>
    {
        public void Configure(EntityTypeBuilder<TagEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.DateCreate).HasColumnType("timestamp without time zone");

            builder.HasIndex(c => c.Name).IsUnique();
        }
    }
}
