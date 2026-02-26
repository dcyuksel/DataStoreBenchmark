using DataStoreBenchmark.Contexts;
using DataStoreBenchmark.Repositories;
using DataStoreBenchmark.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var services = RegisterServices();
var provider = services.BuildServiceProvider();
await InitiliazeDatabase(provider);
await Benchmark(provider);


IServiceCollection RegisterServices()
{
    var services = new ServiceCollection();
    services.AddScoped<IBenchmarkService, BenchmarkService>();
    services.AddScoped<IEfCoreRepository, EfCoreRepository>();
    services.AddScoped<ISqlBulkCopyRepository, SqlBulkCopyRepository>();
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    services.AddSingleton<IConfiguration>(configuration);

    return services;
}

async Task InitiliazeDatabase(IServiceProvider serviceProvider)
{
    using var scope = provider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await context.Database.MigrateAsync();
}

async Task Benchmark(IServiceProvider serviceProvider)
{
    using var scope = provider.CreateScope();
    var benchmarkService = scope.ServiceProvider.GetRequiredService<IBenchmarkService>();
    await benchmarkService.Run();
}