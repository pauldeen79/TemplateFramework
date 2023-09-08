namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public sealed class ComponentRegistrationContext
{
    public IList<IPlaceholderProcessor> PlaceholderProcessors { get; } = new List<IPlaceholderProcessor>();
    public IList<IFunctionResultParser> FunctionResultParsers { get; } = new List<IFunctionResultParser>();
}
