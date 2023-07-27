namespace TemplateFramework.Runtime;

[ExcludeFromCodeCoverage]
public sealed class CustomAssemblyLoadContext : AssemblyLoadContext
{
    public CustomAssemblyLoadContext(string name, bool isCollectible, Func<IEnumerable<string>> customPathsDelegate)
        : base(name, isCollectible)
    {
        Guard.IsNotNull(customPathsDelegate);
        CustomPathsDelegate = customPathsDelegate;
    }

    private Func<IEnumerable<string>> CustomPathsDelegate { get; }

    protected override Assembly Load(AssemblyName assemblyName)
    {
        if (assemblyName is null)
        {
            return null!;
        }

        if (assemblyName.Name == "netstandard")
        {
            return null!;
        }

        var customPath = CustomPathsDelegate.Invoke()
            .Select(directory => Path.Combine(directory, assemblyName.Name + ".dll"))
            .FirstOrDefault(File.Exists);

        if (customPath is null)
        {
            return null!;
        }

        return LoadFromAssemblyPath(customPath);
    }
}
