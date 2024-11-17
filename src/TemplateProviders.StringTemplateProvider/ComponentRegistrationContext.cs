namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public sealed class ComponentRegistrationContext
{
    public IList<IPlaceholderProcessor> PlaceholderProcessors { get; } = [];
    public IList<IFunctionResultParser> FunctionResultParsers { get; } = [];
}
