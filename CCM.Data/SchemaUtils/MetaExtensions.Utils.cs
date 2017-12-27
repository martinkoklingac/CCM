using Npgsql;
using System;
using System.Data;
using System.Reflection;

namespace CCM.Data.SchemaUtils
{
    public static partial class MetaExtensions
    {
        #region PRIVATE METHODS
        private static void MapField<TResult>(
            (PropertyInfo Info, MetaAttribute Meta) field,
            TResult result,
            NpgsqlDataReader reader)
        {
            var ordinal = reader.GetOrdinal(field.Meta.Name);
            var value = default(object);

            if (field.Info.PropertyType == typeof(Int32))
                value = reader.GetInt32(ordinal);
            else if (field.Info.PropertyType == typeof(Int64))
                value = reader.GetInt64(ordinal);
            else if (field.Info.PropertyType == typeof(string))
                value = reader.GetString(ordinal);

            field.Info.SetValue(result, value);
        }

        private static NpgsqlParameter MapParameter<TParam>(
            (PropertyInfo Info, MetaAttribute Meta) field,
            TParam parameter)
        {
            var dbType = default(DbType);
            if (field.Info.PropertyType == typeof(Boolean))
                dbType = DbType.Boolean;
            else if (field.Info.PropertyType == typeof(Int32)
                || field.Info.PropertyType == typeof(Int32?))
                dbType = DbType.Int32;
            else if (field.Info.PropertyType == typeof(Int64)
                || field.Info.PropertyType == typeof(Int64?))
                dbType = DbType.Int64;
            else if (field.Info.PropertyType == typeof(string))
                dbType = DbType.String;

            var val = field.Info.GetValue(parameter);
            var name = field.Meta.Name;

            return new NpgsqlParameter(name, dbType) { Value = val ?? DBNull.Value };
        }
        #endregion
    }
}
