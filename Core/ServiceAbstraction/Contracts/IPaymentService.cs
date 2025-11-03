

namespace ServiceAbstraction.Contracts
{
    public interface IPaymentService
    {
        Task<BasketDto> CreateOrUpdatePaymentIntentIdAsync(string BasketId);
        Task UpdatePaymentsStatusAsync(string json, string signatureHeader);
    }
}
