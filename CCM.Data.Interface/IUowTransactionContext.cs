using System;

namespace CCM.Data
{
    public interface IUowTransactionContext :
        IDisposable
    {
        string Id { get; }

        void Begin();
        void Commit();
        void Rollback();
    }
}
