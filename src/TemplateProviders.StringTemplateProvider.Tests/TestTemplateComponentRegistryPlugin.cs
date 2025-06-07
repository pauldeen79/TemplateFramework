namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests;

public sealed class TestTemplateComponentRegistryPlugin : ITemplateComponentRegistryPlugin
{
    public ComponentRegistrationContext ComponentRegistrationContext { get; }

    public TestTemplateComponentRegistryPlugin(ComponentRegistrationContext componentRegistrationContext)
    {
        Guard.IsNotNull(componentRegistrationContext);

        ComponentRegistrationContext = componentRegistrationContext;
    }

    public Task<Result> Initialize(ITemplateComponentRegistry registry, CancellationToken cancellationToken)
    {
        var processorProcessorMock = Substitute.For<INonGenericMember>();
        processorProcessorMock
            .EvaluateAsync(Arg.Any<FunctionCallContext>(), Arg.Any<CancellationToken>())
            .Returns(args => args.ArgAt<string>(0) == "__test"
                ? Result.Success<GenericFormattableString>("Hello world!")
                : Result.Continue<GenericFormattableString>());

        var functionMock = new FunctionMock();

        ComponentRegistrationContext.Expressions.Add(processorProcessorMock);
        ComponentRegistrationContext.AddFunction(functionMock);

        return Task.FromResult(Result.Success());
    }

    [MemberName("MyFunction")]
    private sealed class FunctionMock : IFunction
    {
        public Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
        {
            return Task.FromResult(Result.Success<object?>("Hello world!"));
        }
    }
}
