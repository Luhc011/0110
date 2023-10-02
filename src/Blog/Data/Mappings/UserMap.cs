using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Table
            builder.ToTable("User");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Autoincrement/Identity
            builder
                .Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            // Properties
            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasColumnType("NVARCHAR")
                .HasMaxLength(80);

            builder
                .Property(x => x.Email)
                .IsRequired()
                .HasColumnType("VARCHAR")
                .HasMaxLength(180);

            builder
                .Property(x => x.Bio)
                .HasColumnType("VARCHAR")
                .HasMaxLength(200);

            builder
                .Property(x => x.Image)
                .HasColumnType("VARCHAR")
                .HasMaxLength(200);

            builder
                .Property(x => x.PasswordHash)
                .IsRequired()
                .HasColumnType("VARCHAR")
                .HasMaxLength(200);

            builder
                .Property(x => x.Slug)
                .IsRequired()
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            // Indexes
            builder
                .HasIndex(x => x.Slug, "IX_User_Slug")
                .IsUnique();

            builder
                .HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    role => role
                        .HasOne<Role>()
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_UserRole_RoleId")
                        .OnDelete(DeleteBehavior.Restrict),
                    user => user
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_UserRole_UserId")
                        .OnDelete(DeleteBehavior.Restrict));
        }
    }
}
