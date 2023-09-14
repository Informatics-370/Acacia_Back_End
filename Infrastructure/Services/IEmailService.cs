using Acacia_Back_End.Core.ViewModels;

namespace Acacia_Back_End.Infrastructure.Services
{
    public interface IEmailService
    {
        void SendEmail(EmailVM request);
    }
}
