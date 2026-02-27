using DataStoreBenchmark.Entities;

namespace DataStoreBenchmark.Repositories;

public interface IDapperRepository
{
    Task<long> AddAsync(IEnumerable<Movie> movies);
}
