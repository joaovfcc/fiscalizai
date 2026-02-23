namespace FiscalizAI.Core.Interfaces;

public interface IUnitOfWork
{
    Task<bool> CommitAsync();
}