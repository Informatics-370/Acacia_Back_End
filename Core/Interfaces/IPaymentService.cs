using Acacia_Back_End.Core.Models;

namespace Acacia_Back_End.Core.Interfaces
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrUpdatePayment(string basketId);
    }
}
