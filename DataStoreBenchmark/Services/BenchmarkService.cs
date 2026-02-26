using DataStoreBenchmark.Extensions;
using DataStoreBenchmark.Repositories;

namespace DataStoreBenchmark.Services;

public class BenchmarkService(
    IEfCoreRepository efCoreRepository,
    ISqlBulkCopyRepository sqlBulkCopyRepository) : IBenchmarkService
{
    private readonly int[] Numbers = [100, 1000, 10000, 100000];

    public async Task Run()
    {
        foreach (var number in Numbers)
        {
            await Run(number);
        }
    }

    private async Task Run(int count)
    {
        Console.WriteLine($"Running benchmark for {count} movies...");

        await RunBenchmarkOperation(
            () => efCoreRepository.AddAndSave1By1Async(MovieGeneratorExtensions.Generate(count)),
            "adding and saving 1 by 1"
        );

        await RunBenchmarkOperation(
            () => efCoreRepository.Add1By1SaveAsync(MovieGeneratorExtensions.Generate(count)),
            "adding 1 by 1 and saving once"
        );

        await RunBenchmarkOperation(
            () => efCoreRepository.AddRangeSaveAsync(MovieGeneratorExtensions.Generate(count)),
            "adding range and saving once"
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
