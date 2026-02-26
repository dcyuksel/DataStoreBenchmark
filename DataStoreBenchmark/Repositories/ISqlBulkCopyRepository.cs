using System.Data;

namespace DataStoreBenchmark.Repositories;

public interface ISqlBulkCopyRepository
{
    Task<long> BulkCopyAsync(DataTable dataTable);
}
