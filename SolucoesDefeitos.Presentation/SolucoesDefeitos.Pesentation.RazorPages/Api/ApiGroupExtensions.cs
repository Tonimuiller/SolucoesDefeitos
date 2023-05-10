namespace SolucoesDefeitos.Pesentation.RazorPages.Api;

public static class ApiGroupExtensions
{
    public static WebApplication MapApiGroups(this WebApplication application)
    {
        var apiGroups = GetAllApiGroups();
        foreach (var apiGroup in apiGroups)
        {
            apiGroup.RegisterGroupMappings(application);
        }

        return application;
    }

    private static IEnumerable<IApiGroup> GetAllApiGroups()
    {
        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
              .Where(x => typeof(IApiGroup).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
              .ToList();
        foreach (var type in types)
        {
            var apiGroup = (IApiGroup?) Activator.CreateInstance(type);
            if (apiGroup != null)
            {
                yield return apiGroup;
            }
        }
    }
}
