namespace Persistence.Data.Configurations
{
    internal class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(P => P.Price).HasColumnType("decimal(18,4)");
            builder.OwnsOne(o => o.Product, p => p.WithOwner());
        }
    }
}
