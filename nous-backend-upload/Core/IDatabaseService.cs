namespace nous_backend_upload.Core
{
    public interface IDatabaseService
    {
        Task<int> ExecuteNonQueryAsync(string sql, object? param = null);
        Task<T?> ExecuteRowAsync<T>(string sql, object? param = null);
        Task<List<T>> ExecuteTableAsync<T>(string sql, object? param = null);
        Task<T?> ExecuteScalarAsync<T>(string sql, object? param = null);
    }
}
