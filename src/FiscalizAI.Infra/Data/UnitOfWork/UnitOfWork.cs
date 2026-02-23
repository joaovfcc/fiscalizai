using FiscalizAI.Core.Interfaces;

namespace FiscalizAI.Infra.Data.UnitOfWork;

public class UnitOfWork(FiscalizAIContext context) : IUnitOfWork
{
    public async Task<bool> CommitAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}