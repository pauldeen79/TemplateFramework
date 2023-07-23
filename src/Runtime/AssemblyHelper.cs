namespace TemplateFramework.Runtime;

[ExcludeFromCodeCoverage]
public static class AssemblyHelper
{
    public static Assembly GetAssembly(string assemblyName, string currentDirectory)
    {
        Guard.IsNotNull(assemblyName);
        Guard.IsNotNull(currentDirectory);

        if (assemblyName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
        {
            // This is plug-in architecture that conforms to Microsoft's documentation.
            var pluginLocation = Path.GetDirectoryName(assemblyName);
            if (string.IsNullOrEmpty(pluginLocation))
            {
                throw new InvalidOperationException($"Could not get directory from assembly name {assemblyName}");
            }

            var context = new PluginLoadContext(pluginLocation);
            return context.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }
        else
        {
            // This is kind of quirk mode, with an assembly name.
            // Works as long as you are using the same package reference on both sides. (the host program and hte plug-in assembly)
            var context = new CustomAssemblyLoadContext("TemplateFramework.Core.CodeGeneration", true, () => new[] { currentDirectory });
            return LoadAssembly(context, assemblyName, currentDirectory);
        }
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
