using DataStoreBenchmark.Entities;

namespace DataStoreBenchmark.Repositories;

public interface IEfCoreRepository
{
    Task<long> AddAndSave1By1Async(IEnumerable<Movie> movies);
    Task<long> Add1By1SaveAsync(IEnumerable<Movie> movies);
    Task<long> AddRangeSaveAsync(IEnumerable<Movie> movies);
}
