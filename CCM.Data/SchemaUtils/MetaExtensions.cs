using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace CCM.Data.SchemaUtils
{
    public static class MetaExtensions
    {
        public static ICollection<NpgsqlParameter> GetParameterCollection<TParam>(this TParam param)
        {
            if (param == null)
                throw new ArgumentNullException("param");


            var table = new Dictionary<Type, DbType>
            {
                { typeof(Int32), DbType.Int32 },
                { typeof(Int64), DbType.Int64 },
                { typeof(string), DbType.String }
            };




            var result = new List<NpgsqlParameter>();

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

        public static void Map<TResult>(this TResult result, NpgsqlDataReader reader)
        {
            if (result == null)
                throw new ArgumentNullException("result");

            var metadata = result
                .GetType()
                .GetProperties(BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Public)
                .Select(x => (a: x, b: x.GetCustomAttribute<MetaAttribute>()));

            foreach (var meta in metadata)
            {
                if(meta.a.MemberType.GetType()== typeof(Int32))
                {
                    var value = reader.GetInt32(reader.GetOrdinal(meta.b.Name));
                    meta.a.SetValue(result, value);
                }

                if (meta.a.MemberType.GetType() == typeof(string))
                {
                    var value = reader.GetString(reader.GetOrdinal(meta.b.Name));
                    meta.a.SetValue(result, value);
                }
            }
        }
    }
}
