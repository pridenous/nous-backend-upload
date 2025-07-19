using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace nous_backend_upload.Core
{
    public class DatabaseService:IDatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostgresConnection")!;
        }

        private IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        public async Task<int> ExecuteNonQueryAsync(string sql, object? param = null)
        {
            using var conn = CreateConnection();
            return await conn.ExecuteAsync(sql, param);
        }

        public async Task<T?> ExecuteRowAsync<T>(string sql, object? param = null)
        {
            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<T>(sql, param);
        }

        public async Task<List<T>> ExecuteTableAsync<T>(string sql, object? param = null)
        {
            using var conn = CreateConnection();
            var result = await conn.QueryAsync<T>(sql, param);
            return result.ToList();
        }

        public async Task<T?> ExecuteScalarAsync<T>(string sql, object? param = null)
        {
            using var conn = CreateConnection();
            return await conn.ExecuteScalarAsync<T>(sql, param);
        }

    }
}
