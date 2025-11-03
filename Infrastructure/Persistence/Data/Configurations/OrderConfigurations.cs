namespace Persistence.Data.Configurations
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            //Property
            builder.Property(S => S.SubTotal).HasColumnType("decimal(18,4)");

            // Enum Conversion 
            builder.Property(ps => ps.PaymentStatus)
                .HasConversion(st => st.ToString(),
                ps => Enum.Parse<OrderPaymentStatus>(ps));

            // Owner
            builder.OwnsOne(sa => sa.ShippingAddress, sa => sa.WithOwner());


            //Relationship
            builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(d => d.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.SetNull);

        }
    }
}
