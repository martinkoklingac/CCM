using System;
using System.Collections.Generic;

namespace CCM.Data
{
    public interface IUowTransactionContext :
        IDisposable
    {
        void Begin();
        void Commit();
        void Rollback();
    }
}
