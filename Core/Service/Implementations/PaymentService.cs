using Stripe;
using Product = Domain.Entities.ProductModule.Product;


namespace Service.Implementations
{
    public class PaymentService(IConfiguration _configuration, IBasketRepository _basketRepository,
        IUnitOfWork _unitOfWork, IMapper _mapper) : IPaymentService
    {

        public async Task<BasketDto> CreateOrUpdatePaymentIntentIdAsync(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripSettings:SecretKey"];
            var basket = await GetBasket(basketId);
            await ValidateOnBasketItemAndShippingPrice(basket);
            var Total = CalculateTotal(basket);
            await CreateOrUpdatePaymentIntendHelperAsync(basket, Total);
            await _basketRepository.CreateOrUpdateAsync(basket);
            return _mapper.Map<BasketDto>(basket);
        }

        private async Task CreateOrUpdatePaymentIntendHelperAsync(CustomerBasket basket, long Total)
        {
            var StripeService = new PaymentIntentService();
            if (string.IsNullOrEmpty(basket.PaymentIndentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = Total,
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]
                };

                var PaymentIntent = await StripeService.CreateAsync(options);
                basket.PaymentIndentId = PaymentIntent.Id;
                basket.ClientSecret = PaymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = Total
                };

                await StripeService.UpdateAsync(basket.PaymentIndentId, options);
            }
        }

        private long CalculateTotal(CustomerBasket basket)
        {
            return (long)(basket.Items.Sum(b => b.Quantity * b.Price) + basket.ShippingPrice) * 100;
        }

        private async Task ValidateOnBasketItemAndShippingPrice(CustomerBasket basket)
        {

            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id)
                    ?? throw new GenericNotFoundException<Product, int>(item.Id);
                item.Price = product.Price;
            }

            if (!basket.DeliveryMethodId.HasValue) throw new GenericNotFoundException<DeliveryMethod, int>(basket.DeliveryMethodId);
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                .GetByIdAsync(basket.DeliveryMethodId.Value) ?? throw new GenericNotFoundException<DeliveryMethod, int>(basket.DeliveryMethodId.Value);
            basket.ShippingPrice = deliveryMethod.Price;
        }

        private async Task<CustomerBasket> GetBasket(string basketId)
        {
            return await _basketRepository.GetBasketAsync(basketId) ?? throw new GenericNotFoundException<CustomerBasket, int>(basketId);
        }

        public async Task UpdatePaymentsStatusAsync(string json, string signatureHeader)
        {
            string endpointSecret = _configuration["StripSettings:EndPointSecret"];
            var stripeEvent = EventUtility.ParseEvent(json, throwOnApiVersionMismatch: false);
            stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, endpointSecret, throwOnApiVersionMismatch: false);
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                await UpdatePaymentIntentRecievedAsync(paymentIntent.Id);
            }
            else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
            {
                await UpdatePaymentIntentFailedAsync(paymentIntent.Id);

            }
            else
            {
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }

        }

        private async Task UpdatePaymentIntentFailedAsync(string paymentIntentId)
        {
            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var order = await orderRepo.GetByIdAsync(new OrderWithPaymentInetntIdSpecifications(paymentIntentId));
            if (order is not null)
            {
                order.PaymentStatus = OrderPaymentStatus.PaymentFailed;
                orderRepo.Update(order);
                await _unitOfWork.SaveChangeAsync();
            }

        }

        private async Task UpdatePaymentIntentRecievedAsync(string paymentIntentId)
        {
            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var order = await orderRepo.GetByIdAsync(new OrderWithPaymentInetntIdSpecifications(paymentIntentId));
            if (order is not null)
            {
                order.PaymentStatus = OrderPaymentStatus.PaymentRecieved;
                orderRepo.Update(order);
                await _unitOfWork.SaveChangeAsync();
            }
        }
    }
}
