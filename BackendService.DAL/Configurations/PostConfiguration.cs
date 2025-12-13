using BackendService.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendService.DAL.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<PostEntity>
    {
        public void Configure(EntityTypeBuilder<PostEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder
                .HasMany(c => c.Tags)
                .WithMany(c => c.Posts)
                .UsingEntity("PostTags");

            builder.Property(c => c.DateCreate).HasColumnType("timestamp without time zone");
            builder.Property(c => c.DateUpdate).HasColumnType("timestamp without time zone");
        }
    }
}
