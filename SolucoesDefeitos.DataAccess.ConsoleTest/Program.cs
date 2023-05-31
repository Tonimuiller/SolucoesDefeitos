using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SolucoesDefeitos.DataAccess.Database;
using SolucoesDefeitos.DataAccess.Repository;
using SolucoesDefeitos.Provider;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.DataAccess.ConsoleTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            var configuration = host.Services.GetService(typeof(IConfiguration)) as IConfiguration;
            var database = new SolucaoDefeitoMySqlDatabase(configuration);
            var solucaoDefeitoUnitOfWork = new UnitOfWork(database);
            var productGroupRepository = new ProductGroupRepository(database, new DefaultDateTimeProvider());
            var productGroups = await productGroupRepository.GetAllAsync(CancellationToken.None);
            foreach(var productGroup in productGroups)
            {
                Console.WriteLine($"Product Group - {productGroup.ProductGroupId} - {productGroup.Description} - {productGroup.CreationDate}");
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args);
    }
}
