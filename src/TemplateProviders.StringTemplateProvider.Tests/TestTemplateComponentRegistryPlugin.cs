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
        var processorProcessorMock = Substitute.For<IPlaceholder>();
        processorProcessorMock
            .Evaluate(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>(), Arg.Any<IFormattableStringParser>())
            .Returns(args => args.ArgAt<string>(0) == "__test"
                ? Result.Success<GenericFormattableString>("Hello world!")
                : Result.Continue<GenericFormattableString>());

        var functionMock = new FunctionMock();

        ComponentRegistrationContext.Placeholders.Add(processorProcessorMock);
        ComponentRegistrationContext.AddFunction(functionMock);

        return Task.FromResult(Result.Success());
    }

    [FunctionName("MyFunction")]
    private sealed class FunctionMock : IFunction
    {
        public Result<object?> Evaluate(FunctionCallContext context)
        {
            return Result.Success<object?>("Hello world!");
        }
    }
}
