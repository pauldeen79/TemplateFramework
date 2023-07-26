namespace TemplateFramework.Core.GenerationEnvironments;

public abstract class GenerationEnvironmentBase : IGenerationEnvironment
{
    protected GenerationEnvironmentBase(GenerationEnvironmentType type)
    {
        Type = type;
    }

    public GenerationEnvironmentType Type { get; }

    public abstract void Process(ICodeGenerationProvider provider, bool dryRun);
}
