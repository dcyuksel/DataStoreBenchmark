using DataStoreBenchmark.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Diagnostics;

namespace DataStoreBenchmark.Repositories;

public class SqlBulkCopyRepository(IConfiguration configuration) : ISqlBulkCopyRepository
{
    private readonly string connectionString = configuration.GetConnectionString("DefaultConnection")!;

    public async Task<long> BulkCopyAsync(DataTable dataTable) => 
        await MeasureElapsedMillisecondsAsync(
            async () =>
            {
                using var bulkCopy = new SqlBulkCopy(connectionString);

                bulkCopy.DestinationTableName = "dbo.Movies";

                bulkCopy.ColumnMappings.Add(nameof(Movie.Id), "Id");
                bulkCopy.ColumnMappings.Add(nameof(Movie.Title), "Title");
                bulkCopy.ColumnMappings.Add(nameof(Movie.Director), "Director");
                bulkCopy.ColumnMappings.Add(nameof(Movie.ReleaseYear), "ReleaseYear");
                bulkCopy.ColumnMappings.Add(nameof(Movie.Genre), "Genre");
                bulkCopy.ColumnMappings.Add(nameof(Movie.CreatedUtc), "CreatedUtc");

                await bulkCopy.WriteToServerAsync(dataTable);
            },
            TruncateMoviesAsync
        );

    private async Task TruncateMoviesAsync()
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        using var command = new SqlCommand("TRUNCATE TABLE Movies", connection);
        await command.ExecuteNonQueryAsync();
    }

    private static async Task<long> MeasureElapsedMillisecondsAsync(Func<Task> action, Func<Task> truncateAction)
    {
        await truncateAction();
        var stopwatch = Stopwatch.StartNew();
        await action();
        stopwatch.Stop();
        await truncateAction();
        return stopwatch.ElapsedMilliseconds;
    }
}
