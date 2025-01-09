using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DiscountCodes.SignalRServer.Infrastructure;

public class DatabaseContext : DbContext, IDatabaseContext
{
    private readonly string _connectionString;

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        _connectionString = Database.GetDbConnection().ConnectionString;
        Console.WriteLine($"Connection String: {_connectionString}");
    }

    public async Task<bool> GenerateCodesAsync(int number, byte length)
    {
        if (number < 0 || number > 2000)
        {
            return false;
        }

        if (length < 7 || length > 8)
        {
            return false;
        }

        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            using (var command = new SqlCommand("GenerateCodes", conn))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Number", SqlDbType.Int) { Value = number });
                command.Parameters.Add(new SqlParameter("@Length", SqlDbType.Int) { Value = length });

                var result = new SqlParameter("@Success", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(result);

                await command.ExecuteNonQueryAsync();

                return Convert.ToBoolean(result.Value);
            }
        }
    }

    public async Task<byte> ApplyCodeAsync(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return 0;
        }

        if (code.Trim().Length < 7 || code.Trim().Length > 8)
        {
            return 0;
        }

        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            using (var command = new SqlCommand("ApplyCode", conn))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Code", SqlDbType.VarChar, 8) { Value = code });

                var result = new SqlParameter("@Result", SqlDbType.TinyInt)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(result);

                await command.ExecuteNonQueryAsync();

                return (byte)result.Value;
            }
        }
    }
}