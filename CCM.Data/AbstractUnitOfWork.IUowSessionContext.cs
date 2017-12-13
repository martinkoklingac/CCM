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
        public void ExecFunction<TParam, TResult>(string function, TParam param, TResult result)
            where TResult : new()
        {
            throw new NotImplementedException();
        }

        public void ExecFunction<TParam, TResult>(string function, TParam param, ICollection<TResult> result)
            where TResult : new()
        {
            var parameters = param.GetParameterCollection().ToArray();
            using (var command = this._connection.FunctionCommand(function, parameters))
            using (var reader = command.ExecuteReader())
            {
                var reault = new TResult();
                result.Map(reader);
            }
        }
        #endregion
    }
}
