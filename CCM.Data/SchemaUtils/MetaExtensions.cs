using Core.Utils;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CCM.Data.SchemaUtils
{
    public static partial class MetaExtensions
    {
        #region PUBLIC METHODS
        public static IEnumerable<NpgsqlParameter> GetParameters<TParam>(this TParam parameter)
        {
            if (parameter == null)
                return new NpgsqlParameter[0];

            return parameter
                .GetType()
                .GetProperties(BindingFlags.Instance
                    | BindingFlags.Public
                    | BindingFlags.GetProperty
                    | BindingFlags.GetField)
                .Select(x => (x, x.GetCustomAttribute<MetaAttribute>()))
                .Select(x => MapParameter(x, parameter));
        }

        public static TResult Map<TResult>(this TResult result, NpgsqlDataReader reader)
        {
            if (result == null)
                return result;

            result
                .GetType()
                .GetProperties(BindingFlags.Instance
                    | BindingFlags.Public
                    | BindingFlags.GetProperty
                    | BindingFlags.GetField)
                .Select(x => (x, x.GetCustomAttribute<MetaAttribute>()))
                .Apply(x => MapField(x, result, reader));

            return result;
        }
        #endregion
    }
}
