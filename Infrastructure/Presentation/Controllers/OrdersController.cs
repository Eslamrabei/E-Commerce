using Microsoft.AspNetCore.Authorization;
using Shared.Dtos.OrderDto;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Authorize]
    public class OrdersController(IServiceManager _serviceManager) : ApiController
    {
        // Create Order 
        [HttpPost]
        public async Task<ActionResult<OrderResultDto>> CreateOrderAsync(OrderRequest orderRequest)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var CreateOrder = await _serviceManager.OrderService.CreateOrderAsync(orderRequest, userEmail);
            return Ok(CreateOrder);
        }

        // GetOrder By ID
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderResultDto>> GetOrderByIdAsync(Guid id)
         => Ok(await _serviceManager.OrderService.GetOrderByIdAsync(id));


        // Get All Order By Email
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResultDto>>> GetAllOrderByEmailAsync()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await _serviceManager.OrderService.GetAllOrdersByEmailAsync(userEmail);
            return Ok(Orders);
        }

        // Get DeliveryMethod
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliverMethodResult>>> GetDeliveryMethods()
        => Ok(await _serviceManager.OrderService.GetDeliverMethodAsync());

    }
}
