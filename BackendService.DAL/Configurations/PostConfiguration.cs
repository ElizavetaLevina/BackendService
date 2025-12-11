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
        }
    }
}
