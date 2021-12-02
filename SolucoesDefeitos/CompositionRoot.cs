using Microsoft.Extensions.DependencyInjection;
using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.BusinessImplementation.Service;
using SolucoesDefeitos.DataAccess.Database;
using SolucoesDefeitos.DataAccess.Repository;
using SolucoesDefeitos.DataAccess.UnitOfWork;
using SolucoesDefeitos.Model;
using SolucoesDefeitos.Provider;

namespace SolucoesDefeitos
{
    public static class CompositionRoot
    {
        public static IServiceCollection ConfigureApplicationDependencies(this IServiceCollection serviceCollection)
        {
            ConfigureDatabases(serviceCollection);
            ConfigureUnitOfWork(serviceCollection);
            ConfigureGenericRepositories(serviceCollection);
            ConfigureRepositories(serviceCollection);
            ConfigureGenericServices(serviceCollection);
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
            serviceCollection.AddScoped<IAnomalyService, AnomalyService >();
            serviceCollection.AddScoped<IAttachmentService, AttachmentService>();
            serviceCollection.AddScoped<IManufacturerService, ManufacturerService>();
            serviceCollection.AddScoped<IProductGroupService, ProductGroupService>();
            serviceCollection.AddScoped<IProductService, ProductService>();
        }

        private static void ConfigureGenericServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IService<AnomalyProductSpecification>, AnomalyProductSpecificationService>();
            serviceCollection.AddScoped<IService<Anomaly>, AnomalyService>();
            serviceCollection.AddScoped<IService<Attachment>, AttachmentService>();
            serviceCollection.AddScoped<IService<Manufacturer>, ManufacturerService>();
            serviceCollection.AddScoped<IService<ProductGroup>, ProductGroupService>();
            serviceCollection.AddScoped<IService<Product>, ProductService>();
        }

        private static void ConfigureUnitOfWork(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<DapperUnitOfWork<SolucaoDefeitoMySqlDatabase>, SolucaoDefeitoUnitOfWork>();
        }

        private static void ConfigureGenericRepositories(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IRepository<AnomalyProductSpecification>, AnomalyProductSpecificationRepository>();
            serviceCollection.AddScoped<IRepository<Anomaly>, AnomalyRepository>();
            serviceCollection.AddScoped<IRepository<Attachment>, AttachmentRepository>();
            serviceCollection.AddScoped<IRepository<Manufacturer>, ManufacturerRepository>();
            serviceCollection.AddScoped<IRepository<ProductGroup>, ProductGroupRepository>();
            serviceCollection.AddScoped<IRepository<Product>, ProductRepository>();
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
            serviceCollection.AddSingleton<SolucaoDefeitoMySqlDatabase>();
        }
    }
}
