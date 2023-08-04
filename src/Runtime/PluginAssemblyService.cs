namespace TemplateFramework.Runtime;

[ExcludeFromCodeCoverage]
public class PluginAssemblyService : IAssemblyService
{
    public Assembly GetAssembly(string assemblyName, string currentDirectory)
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
            // Works as long as you are using the same package reference on both sides. (the host program and the plug-in assembly)
            return new CustomAssemblyService().GetAssembly(assemblyName, currentDirectory);
        }
    }
}
