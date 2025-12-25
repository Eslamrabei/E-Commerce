using Address = Domain.Entities.OrderModule.Address;
namespace Service.Implementations
{
    public class OrderService(IMapper _mapper, IBasketRepository _basketRepository, IUnitOfWork _unitOfWork) : IOrderService
    {
        public async Task<OrderResultDto> CreateOrderAsync(OrderRequest order, string userEmail)
        {
            var address = _mapper.Map<Address>(order.ShipToAddress);

            var basket = await _basketRepository.GetBasketAsync(order.BasketId)
                ?? throw new GenericNotFoundException<CustomerBasket, int>(order.BasketId, "BasketId");
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id)
                ?? throw new GenericNotFoundException<Product, int>(item.Id, "BasketId");
                orderItems.Add(CreateOrderProductItem(product, item));
            }

            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();

            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                .GetByIdAsync(order.DeliveryMethodId) ?? throw new GenericNotFoundException<DeliveryMethod, int>(order.DeliveryMethodId, "DeliveryMethodId");

            var orderExist = await orderRepo.GetByIdAsync(new OrderWithPaymentInetntIdSpecifications(basket.PaymentIndentId));
            if (orderExist != null)
            {
                orderRepo.Delete(orderExist);

            }

            var Subtotal = orderItems.Sum(o => o.Quantity * o.Price);

            var CreateOrder = new Order(userEmail, address, orderItems, deliveryMethod, Subtotal, basket.PaymentIndentId);
            await orderRepo.AddAsync(CreateOrder);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<OrderResultDto>(CreateOrder);
        }
        private OrderItem CreateOrderProductItem(Product product, BasketItems item)
        {
            var productInOrder = new ProductInOrderItem(product.Id, product.Name, product.PictureUrl);
            return new OrderItem(productInOrder, item.Quantity, product.Price);
        }
        public async Task<IEnumerable<DeliverMethodResult>> GetDeliverMethodAsync()
        {
            var deliverMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<DeliverMethodResult>>(deliverMethod);
        }


        public async Task<IEnumerable<OrderResultDto>> GetAllOrdersByEmailAsync(string userEmail)
        {
            var orderSpecifications = new OrderWithIncludesSpecifications(userEmail) ?? throw new GenericNotFoundException<Order, Guid>(userEmail, "userEmail");
            var orderResult = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(orderSpecifications);
            return _mapper.Map<IEnumerable<OrderResultDto>>(orderResult);

        }


        public async Task<OrderResultDto> GetOrderByIdAsync(Guid id)
        {
            var orderSpecifications = new OrderWithIncludesSpecifications(id) ?? throw new GenericNotFoundException<Order, Guid>(id, "Id");
            var ordreResult = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(orderSpecifications);
            return _mapper.Map<OrderResultDto>(ordreResult);
        }
    }
}
