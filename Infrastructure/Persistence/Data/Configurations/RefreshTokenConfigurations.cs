namespace Persistence.Data.Configurations
{
    internal class RefreshTokenConfigurations : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasOne(u => u.User)
                .WithMany(t => t.RefreshTokens)
                .HasForeignKey(k => k.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
