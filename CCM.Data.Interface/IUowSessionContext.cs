using System.Collections.Generic;

namespace CCM.Data
{
    public interface IUowSessionContext
    {
        void ExecFunction<TParam, TResult>(string function, TParam param, TResult result)
            where TResult : new();
        void ExecFunction<TParam, TResult>(string function, TParam param, ICollection<TResult> result)
            where TResult : new();
    }
}
