namespace U_VoluntApp_Backend.Src.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using U_VoluntApp_Backend.Src.Infrastructure.Persistence.Models.Auth;

public partial class AppDbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("refresh_tokens_pkey");

            entity.ToTable("refresh_tokens");

            entity.HasIndex(e => e.UvaCode).IsUnique();
            entity.HasIndex(e => e.TokenHash).IsUnique();
            entity.HasIndex(e => e.IdentityUserId);
            entity.HasIndex(e => e.ProfileCode);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UvaCode).HasColumnName("uva_code");
            entity.Property(e => e.IdentityUserId).HasMaxLength(450).HasColumnName("identity_user_id");
            entity.Property(e => e.ProfileCode).HasColumnName("profile_code");
            entity.Property(e => e.TokenHash).HasMaxLength(128).HasColumnName("token_hash");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.RevokedAt).HasColumnName("revoked_at");
            entity.Property(e => e.ReplacedByTokenHash)
                .HasMaxLength(128)
                .HasColumnName("replaced_by_token_hash");
            entity.Property(e => e.CreatedByIp)
                .HasMaxLength(100)
                .HasColumnName("created_by_ip");
            entity.Property(e => e.RevokedByIp)
                .HasMaxLength(100)
                .HasColumnName("revoked_by_ip");
            entity.Property(e => e.UserAgent)
                .HasMaxLength(512)
                .HasColumnName("user_agent");
            entity.Property(e => e.ReasonRevoked)
                .HasMaxLength(200)
                .HasColumnName("reason_revoked");
        });
    }
}
