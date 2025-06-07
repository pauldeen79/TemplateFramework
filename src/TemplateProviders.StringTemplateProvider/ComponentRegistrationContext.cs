namespace TemplateFramework.TemplateProviders.StringTemplateProvider;

public sealed class ComponentRegistrationContext
{
    public IList<INonGenericMember> Expressions { get; } = [];
    public IList<IMember> Functions { get; } = [];
    public ComponentRegistrationContextFunction DescriptorsFunction { get; }

    public ComponentRegistrationContext(IEnumerable<IMember> functions)
    {
        Guard.IsNotNull(functions);

        // Want to use single here, but this makes unit test fail because of dynamic argument null checks on constructor tests...
        DescriptorsFunction = functions.OfType<ComponentRegistrationContextFunction>().SingleOrDefault()!;
    }

    public void ClearFunctions()
    {
        Functions.Clear();
        DescriptorsFunction.ClearFunctions();
    }

    public void AddFunction(IFunction function)
    {
        Functions.Add(function);
        DescriptorsFunction.AddFunction(function);
    }
}
