using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            // Table
            builder.ToTable("Post");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Autoincrement/Identity
            builder
                .Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            // Properties
            builder
                .Property(x => x.Title)
                .IsRequired()
                .HasColumnType("NVARCHAR")
                .HasMaxLength(80);

            builder
                .Property(x => x.Slug)
                .IsRequired()
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            builder
                .Property(x => x.Summary)
                .IsRequired()
                .HasColumnType("NVARCHAR")
                .HasMaxLength(200);

            builder
                .Property(x => x.Body)
                .IsRequired()
                .HasColumnType("NVARCHAR")
                .HasMaxLength(2000);

            builder
                .Property(x => x.Created)
                .IsRequired()
                .HasColumnName("CreateDate")
                .HasColumnType("SMALLDATETIME")
                .HasDefaultValue(DateTime.UtcNow);

            builder
                .Property(x => x.Updated)
                .IsRequired()
                .HasColumnName("LastUpdateDate")
                .HasColumnType("SMALLDATETIME")
                .HasDefaultValue(DateTime.UtcNow);

            // Indexes
            builder
                .HasIndex(x => x.Slug, "IX_Post_Slug")
                .IsUnique();

            // Relationships
            builder
                .HasOne(x => x.Author)
                .WithMany(x => x.Posts)
                .HasConstraintName("FK_Post_Author")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.Category)
                .WithMany(x => x.Posts)
                .HasConstraintName("FK_Post_Category")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(x => x.Tags)
                .WithMany(x => x.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    "PostTag",
                    post => post
                        .HasOne<Tag>()
                        .WithMany()
                        .HasForeignKey("PostId")
                        .HasConstraintName("FK_PostTag_PostId")
                        .OnDelete(DeleteBehavior.Cascade),
                    tag => tag
                        .HasOne<Post>()
                        .WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK_PostTag_TagId")
                        .OnDelete(DeleteBehavior.Cascade));
        }
    }
}
