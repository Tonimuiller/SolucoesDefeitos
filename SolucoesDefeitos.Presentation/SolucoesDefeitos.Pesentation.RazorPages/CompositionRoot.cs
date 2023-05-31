using SolucoesDefeitos.BusinessDefinition;
using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.BusinessImplementation.Service;
using SolucoesDefeitos.DataAccess;
using SolucoesDefeitos.DataAccess.Database;
using SolucoesDefeitos.DataAccess.Repository;
using SolucoesDefeitos.Provider;

namespace SolucoesDefeitos.Pesentation.RazorPages;

public static class CompositionRoot
{
    public static IServiceCollection ConfigureApplicationDependencies(this IServiceCollection serviceCollection)
    {
        ConfigureDatabases(serviceCollection);
        ConfigureUnitOfWork(serviceCollection);
        ConfigureRepositories(serviceCollection);
        ConfigureServices(serviceCollection);
        ConfigureProviders(serviceCollection);
        return serviceCollection;
    }

    private static void ConfigureProviders(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IDateTimeProvider, DefaultDateTimeProvider>();
    }

    private static void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAnomalyProductSpecificationService, AnomalyProductSpecificationService>();
        serviceCollection.AddScoped<IAnomalyService, AnomalyService>();
        serviceCollection.AddScoped<IAttachmentService, AttachmentService>();
        serviceCollection.AddScoped<IManufacturerService, ManufacturerService>();
        serviceCollection.AddScoped<IProductGroupService, ProductGroupService>();
        serviceCollection.AddScoped<IProductService, ProductService>();
    }

    private static void ConfigureUnitOfWork(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void ConfigureRepositories(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAnomalyProductSpecificationRepository, AnomalyProductSpecificationRepository>();
        serviceCollection.AddScoped<IAnomalyRepository, AnomalyRepository>();
        serviceCollection.AddScoped<IAttachmentRepository, AttachmentRepository>();
        serviceCollection.AddScoped<IManufacturerRepository, ManufacturerRepository>();
        serviceCollection.AddScoped<IProductGroupRepository, ProductGroupRepository>();
        serviceCollection.AddScoped<IProductRepository, ProductRepository>();
    }

    private static void ConfigureDatabases(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IDatabase, SolucaoDefeitoMySqlDatabase>();
    }
}
