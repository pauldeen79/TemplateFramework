namespace TemplateFramework.Runtime;

[ExcludeFromCodeCoverage]
public class CustomAssemblyService : IAssemblyService
{
    private readonly IAssemblyInfoContextService _assemblyInfoContextService;

    public CustomAssemblyService(IAssemblyInfoContextService assemblyInfoContextService)
    {
        Guard.IsNotNull(assemblyInfoContextService);

        _assemblyInfoContextService = assemblyInfoContextService;
    }

    public Assembly GetAssembly(string assemblyName, string currentDirectory)
    {
        Guard.IsNotNull(assemblyName);
        Guard.IsNotNull(currentDirectory);

        var context = new CustomAssemblyLoadContext("TemplateFramework.Core.CodeGeneration", true, _assemblyInfoContextService, () => new[] { currentDirectory });
        return LoadAssembly(context, assemblyName, currentDirectory);
    }

    private static Assembly LoadAssembly(AssemblyLoadContext context, string assemblyName, string currentDirectory)
    {
        try
        {
            return context.LoadFromAssemblyName(new AssemblyName(assemblyName));
        }
        catch (Exception e) when (e.Message.StartsWith("The given assembly name was invalid.", StringComparison.InvariantCulture) || e.Message.EndsWith("The system cannot find the file specified.", StringComparison.InvariantCulture))
        {
            if (assemblyName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase) && !Path.IsPathRooted(assemblyName))
            {
                return context.LoadFromAssemblyPath(Path.Combine(currentDirectory, Path.GetFileName(assemblyName)));
            }

            return context.LoadFromAssemblyPath(assemblyName);
        }
    }
}
