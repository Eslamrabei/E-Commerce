namespace Service.Specifications
{
    public class OrderWithPaymentInetntIdSpecifications : BaseSpecification<Order, Guid>
    {
        public OrderWithPaymentInetntIdSpecifications(string paymentIntentId)
            : base(p => p.PaymentIntentId == paymentIntentId)
        {

        }
    }
}
