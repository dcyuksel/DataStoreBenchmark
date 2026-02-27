using Dapper;
using DataStoreBenchmark.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace DataStoreBenchmark.Repositories;

public class DapperRepository(IConfiguration configuration) : IDapperRepository
{
    private readonly string connectionString = configuration.GetConnectionString("DefaultConnection")!;

    public async Task<long> AddAsync(IEnumerable<Movie> movies)
        => await MeasureElapsedMillisecondsAsync(
            async () =>
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                const string sql =
                @"
                INSERT INTO Movies (Id, Title, Director, ReleaseYear, Genre, CreatedUtc)
                VALUES (@Id, @Title, @Director, @ReleaseYear, @Genre, @CreatedUtc);
                ";

                await connection.ExecuteAsync(sql, movies);
            },
            TruncateMoviesAsync
        );

    private static async Task<long> MeasureElapsedMillisecondsAsync(Func<Task> saveAction, Func<Task> truncateAction)
    {
        await truncateAction();
        var stopwatch = Stopwatch.StartNew();
        await saveAction();
        stopwatch.Stop();
        await truncateAction();

        return stopwatch.ElapsedMilliseconds;
    }

    private async Task TruncateMoviesAsync()
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        using var command = new SqlCommand("TRUNCATE TABLE Movies", connection);
        await command.ExecuteNonQueryAsync();
    }
}
