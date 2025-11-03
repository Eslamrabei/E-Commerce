namespace Persistence.Data.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Price).HasColumnType("decimal(15,2)");

            //Relations
            builder.HasOne(p => p.ProductBrand)
                .WithMany()
                .HasForeignKey(k => k.BrandId);

            builder.HasOne(p => p.ProductType)
                .WithMany()
                .HasForeignKey(k => k.TypeId);
        }
    }
}
