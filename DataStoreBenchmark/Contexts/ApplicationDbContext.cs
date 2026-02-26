using DataStoreBenchmark.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataStoreBenchmark.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public virtual DbSet<Movie> Movies { get; set; }
}
