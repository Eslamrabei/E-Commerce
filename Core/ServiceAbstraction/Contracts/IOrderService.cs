namespace ServiceAbstraction.Contracts
{
    public interface IOrderService
    {
        // GetOrderByID 
        Task<OrderResultDto> GetOrderByIdAsync(Guid id);
        // GetAllOrderByEmail
        Task<IEnumerable<OrderResultDto>> GetAllOrdersByEmailAsync(string userEmail);
        // CreateOrder
        Task<OrderResultDto> CreateOrderAsync(OrderRequest order, string userEmail);
        // GetAllDeliveryMethod
        Task<IEnumerable<DeliverMethodResult>> GetDeliverMethodAsync();
    }
}

