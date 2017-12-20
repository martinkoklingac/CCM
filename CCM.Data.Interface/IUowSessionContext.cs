using System.Collections.Generic;

namespace CCM.Data
{
    public interface IUowSessionContext
    {
        void ExecFunctionSingle<TParam, TResult>(string function, TParam param, TResult result)
            where TResult : new();
        void ExecFunctionCollection<TResult>(string function, ICollection<TResult> result)
            where TResult : new();
        void ExecFunctionCollection<TParam, TResult>(string function, TParam param, ICollection<TResult> result)
            where TResult : new();
    }
}
