namespace Service.Specifications
{
    public class OrderWithIncludesSpecifications : BaseSpecification<Order, Guid>
    {
        public OrderWithIncludesSpecifications(Guid id)
            : base(o => o.Id == id)
        {
            AddInclude(dm => dm.DeliveryMethod);
            AddInclude(oi => oi.OrderItems);
        }

        public OrderWithIncludesSpecifications(string userEmail)
            : base(ue => ue.UserEmail == userEmail)
        {
            AddInclude(dm => dm.DeliveryMethod);
            AddInclude(ue => ue.OrderItems);
            AddOrderBy(o => o.OrderDate);

        }

    }
}
