using System;

namespace CCM.Data
{
    public interface IUnitOfWork :
        IDisposable
    {
        string Id { get; }

        void Begin();
        void Commit();
        void Rollback();
    }
}
