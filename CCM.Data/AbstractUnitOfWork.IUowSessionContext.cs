using CCM.Data.SchemaUtils;
using System.Collections.Generic;
using System.Linq;

namespace CCM.Data
{
    public abstract partial class AbstractUnitOfWork :
        IUowSessionContext
    {
        #region PUBLIC METHODS
        public void ExecFunctionSingle<TResult, TParam>(string function, out TResult result, TParam param)
            where TResult : new()
        {
            result = default(TResult);
            var parameters = param.GetParameters().ToArray();
            using (var command = this._connection.FunctionCommand(function, parameters))
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                    result = new TResult().Map(reader);
            }
        }

        public void ExecFunctionCollection<TResult, TParam>(string function, out IReadOnlyCollection<TResult> result, TParam param)
            where TResult : new()
        {
            var parameters = param.GetParameters().ToArray();
            using (var command = this._connection.FunctionCommand(function, parameters))
            using (var reader = command.ExecuteReader())
            {
                var list = new List<TResult>();
                while (reader.Read())
                {
                    var item = new TResult().Map(reader);
                    list.Add(item);
                }

                result = list;
            }
        }

        public void ExecFunctionCollection<TResult>(string function, out IReadOnlyCollection<TResult> result)
            where TResult : new()
        {
            ExecFunctionCollection(function, out result, default(object));
        }
        #endregion
    }
}
