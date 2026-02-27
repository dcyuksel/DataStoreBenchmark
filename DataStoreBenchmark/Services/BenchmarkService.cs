using DataStoreBenchmark.Extensions;
using DataStoreBenchmark.Repositories;

namespace DataStoreBenchmark.Services;

public class BenchmarkService(
    IEfCoreRepository efCoreRepository,
    ISqlBulkCopyRepository sqlBulkCopyRepository,
    IDapperRepository dapperRepository) : IBenchmarkService
{
    private readonly int[] Numbers = [100, 1000, 10000];

    public async Task RunAsync()
    {
        foreach (var number in Numbers)
        {
            await RunAsync(number);
        }
    }

    private async Task RunAsync(int count)
    {
        Console.WriteLine($"Running benchmark for {count} movies...");

        var movies = MovieGeneratorExtensions.Generate(count);
        await RunBenchmarkOperation(
            () => efCoreRepository.AddAndSave1By1Async(movies),
            "ef core - adding and saving 1 by 1"
        );

        await RunBenchmarkOperation(
            () => efCoreRepository.Add1By1SaveAsync(movies),
            "ef core - adding 1 by 1 and saving once"
        );

        await RunBenchmarkOperation(
            () => efCoreRepository.AddRangeSaveAsync(movies),
            "ef core - adding range and saving once"
        );

        await RunBenchmarkOperation(
            () => dapperRepository.AddAsync(movies),
            "dapper"
        );

        await RunBenchmarkOperation(
            () => sqlBulkCopyRepository.BulkCopyAsync(MovieGeneratorExtensions.GenerateDataTable(count)),
            "sql bulk copy"
        );

        Console.WriteLine($"Running benchmark for {count} movies is completed.");
        Console.WriteLine();
    }

    private static async Task RunBenchmarkOperation(
        Func<Task<long>> operation,
        string description)
    {
        var elapsed = await operation();
        Console.WriteLine($"Elapsed time for {description}: {elapsed} ms.");
    }
}
