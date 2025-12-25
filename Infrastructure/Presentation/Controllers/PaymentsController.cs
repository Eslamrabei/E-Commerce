using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers
{
    [Authorize]
    public class PaymentsController(IServiceManager _serviceManager) : ApiController
    {
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentInetnt(string basketId)
            => Ok(await _serviceManager.PaymentService.CreateOrUpdatePaymentIntentIdAsync(basketId));

        [HttpPost("Webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signatureHeader = Request.Headers["Stripe-Signature"];
            await _serviceManager.PaymentService.UpdatePaymentsStatusAsync(json, signatureHeader);
            return new EmptyResult();
        }
    }
}
