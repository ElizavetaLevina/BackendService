using BackendService.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendService.DAL.Configurations
{
    public class PostPendingConfiguration : IEntityTypeConfiguration<PostPendingEntity>
    {
        public void Configure(EntityTypeBuilder<PostPendingEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasIndex(c => c.PostId).IsUnique();

            builder.Property(c => c.DateCreate).HasColumnType("timestamp without time zone");
            builder.Property(c => c.DateModerate).HasColumnType("timestamp without time zone");
        }
    }
}
