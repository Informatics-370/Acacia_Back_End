using Acacia_Back_End.Core.Specifications;
using Acacia_Back_End.Core.ViewModels;

namespace Acacia_Back_End.Core.Interfaces
{
    public interface IAuditRepository
    {
        // Audit Trail
        Task<IReadOnlyList<AuditTrailVM>> GetAuditTrailAsync(AuditSpecParams searchParams);
    }
}
