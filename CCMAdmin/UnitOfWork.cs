using System;

namespace CCMAdmin
{
    public class UnitOfWork
    {
        public string Id { get; }
        public string Trace { get; }

        public UnitOfWork(string trace)
        {
            Id = Guid.NewGuid().ToString();
            Trace = trace;
        }
    }
}
