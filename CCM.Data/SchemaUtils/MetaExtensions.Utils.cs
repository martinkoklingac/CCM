using Npgsql;
using System;
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
        #endregion
    }
}
