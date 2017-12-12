using Npgsql;
using System.Data;
using System.Linq;

namespace CCM.Data
{
    public static class NpgsqlExtensions
    {
        public static NpgsqlConnection Init(this NpgsqlConnection connection)
        {
            connection.Open();
            return connection;
        }

        public static NpgsqlCommand FunctionCommand(this NpgsqlConnection connection,
            string function,
            params NpgsqlParameter[] parameters)
        {
            var command = new NpgsqlCommand(function, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters.Any())
                command.Parameters.AddRange(parameters);

            return command;
        }
    }
}
