using DataStoreBenchmark.Contexts;
using DataStoreBenchmark.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DataStoreBenchmark.Repositories;

public class EfCoreRepository(ApplicationDbContext dbContext) : IEfCoreRepository
{
    public async Task<long> AddAndSave1By1Async(IEnumerable<Movie> movies)
        => await MeasureElapsedMillisecondsAsync(
            async () =>
            {
                foreach (var movie in movies)
                {
                    await dbContext.Movies.AddAsync(movie);
                    await dbContext.SaveChangesAsync();
                }
            },
            TruncateMoviesAsync
        );

    public async Task<long> Add1By1SaveAsync(IEnumerable<Movie> movies)
        => await MeasureElapsedMillisecondsAsync(
            async () =>
            {
                foreach (var movie in movies)
                {
                    await dbContext.Movies.AddAsync(movie);
                }

                await dbContext.SaveChangesAsync();
            },
            TruncateMoviesAsync
        );

    public async Task<long> AddRangeSaveAsync(IEnumerable<Movie> movies)
        => await MeasureElapsedMillisecondsAsync(
            async () =>
            {
                await dbContext.Movies.AddRangeAsync(movies);
                await dbContext.SaveChangesAsync();
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
        await dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Movies");
    }
}
