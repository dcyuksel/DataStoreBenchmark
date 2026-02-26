using DataStoreBenchmark.Entities;
using System.Data;

namespace DataStoreBenchmark.Extensions;

public static class MovieGeneratorExtensions
{
    public static IEnumerable<Movie> Generate(int count)
    {
        for (var i = 1; i <= count; i++)
        {
            yield return new Movie
            {
                Id = Guid.CreateVersion7(),
                Title = $"Movie {i}",
                Director = $"Director {i}",
                ReleaseYear = 2000 + (i % 25),
                Genre = "Drama",
                CreatedUtc = DateTime.UtcNow
            };
        }
    }

    public static DataTable GenerateDataTable(int count)
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(Guid));
        table.Columns.Add("Title", typeof(string));
        table.Columns.Add("Director", typeof(string));
        table.Columns.Add("ReleaseYear", typeof(int));
        table.Columns.Add("Genre", typeof(string));
        table.Columns.Add("CreatedUtc", typeof(DateTime));
        for (var i = 1; i <= count; i++)
        {
            var row = table.NewRow();
            row["Id"] = Guid.CreateVersion7();
            row["Title"] = $"Movie {i}";
            row["Director"] = $"Director {i}";
            row["ReleaseYear"] = 2000 + (i % 25);
            row["Genre"] = "Drama";
            row["CreatedUtc"] = DateTime.UtcNow;
            table.Rows.Add(row);
        }
        return table;
    }
}
