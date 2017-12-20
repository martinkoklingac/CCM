using Core.Utils;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace CCM.Data.SchemaUtils
{
    public static partial class MetaExtensions
    {
        #region PUBLIC METHODS
        public static ICollection<NpgsqlParameter> GetParameterCollection<TParam>(this TParam param)
        {
            var result = new List<NpgsqlParameter>();

            if (param == null)
                return result;

            var table = new Dictionary<Type, DbType>
            {
                { typeof(Int32), DbType.Int32 },
                { typeof(Int64), DbType.Int64 },
                { typeof(string), DbType.String }
            };

            var metadata = param
                .GetType()
                .GetProperties(BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Public)
                .Select(x => (a: x, b: x.GetCustomAttribute<MetaAttribute>()));

            foreach (var meta in metadata)
            {
                if (table.ContainsKey(meta.a.MemberType.GetType()))
                {
                    var dbtype = table[meta.a.MemberType.GetType()];
                    var val = meta.a.GetValue(param);
                    var name = meta.b.Name;

                    result.Add(new NpgsqlParameter(name, dbtype) { Value = val });
                }
            }

            return null;
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
