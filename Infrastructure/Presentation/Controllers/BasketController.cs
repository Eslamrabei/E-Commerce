using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers
{
    [Authorize]
    public class BasketController(IServiceManager _serviceManager) : ApiController
    {
        // 1] Get BasketAsync 
        [HttpGet]
        public async Task<ActionResult<BasketDto?>> GetBasketAsync(string key)
            => Ok(await _serviceManager.BasketService.GetBasketAsync(key));

        // 2] Create Or UpdateAsync 
        [HttpPost]
        public async Task<ActionResult<BasketDto?>> CreateOrUpdateAsync([FromBody] BasketDto customerBasket)
            => Ok(await _serviceManager.BasketService.CreateOrUpdateAsync(customerBasket));

        // 3] DeleteAsync
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteAsync(string id)
            => Ok(await _serviceManager.BasketService.DeleteBasketAsync(id));

    }
}
