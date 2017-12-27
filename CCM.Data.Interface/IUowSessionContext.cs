using System.Collections.Generic;

namespace CCM.Data
{
    public interface IUowSessionContext
    {
        void ExecFunctionSingle<TResult, TParam>(string function, out TResult result, TParam param)
            where TResult : new();
        void ExecFunctionCollection<TResult>(string function, out IReadOnlyCollection<TResult> result)
            where TResult : new();
        void ExecFunctionCollection<TResult, TParam>(string function, out IReadOnlyCollection<TResult> result, TParam param)
            where TResult : new();
    }
}
