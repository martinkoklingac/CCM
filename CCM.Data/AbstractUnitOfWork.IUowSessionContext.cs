using CCM.Data.SchemaUtils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CCM.Data
{
    public abstract partial class AbstractUnitOfWork :
        IUowSessionContext
    {
        #region PUBLIC METHODS
        public void ExecFunctionSingle<TParam, TResult>(string function, TParam param, TResult result)
            where TResult : new()
        {
            throw new NotImplementedException();
        }

        public void ExecFunctionCollection<TParam, TResult>(string function, TParam param, ICollection<TResult> result)
            where TResult : new()
        {
            var parameters = param.GetParameters().ToArray();
            using (var command = this._connection.FunctionCommand(function, parameters))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var item = new TResult().Map(reader);
                    result.Add(item);
                }
            }
        }

        public void ExecFunctionCollection<TResult>(string function, ICollection<TResult> result)
            where TResult : new()
        {
            ExecFunctionCollection(function, default(object), result);
        }
        #endregion
    }
}
