namespace TemplateFramework.Runtime;

[ExcludeFromCodeCoverage]
public class CustomAssemblyService : IAssemblyService
{
    public Assembly GetAssembly(string assemblyName, string currentDirectory)
    {
        Guard.IsNotNull(assemblyName);
        Guard.IsNotNull(currentDirectory);

        // This is kind of quirk mode, with an assembly name.
        // Works as long as you are using the same package reference on both sides. (the host program and the plug-in assembly)
        var context = new CustomAssemblyLoadContext("TemplateFramework.Core.CodeGeneration", true, () => new[] { currentDirectory });
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
