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

        var functionResultParserMock = Substitute.For<IFunction>();
        functionResultParserMock.Evaluate(Arg.Any<FunctionCallContext>())
            .Returns(args =>
            {
                if (args.ArgAt<FunctionCallContext>(0).FunctionCall.Name == "MyFunction")
                {
                    return Result.Success<object?>("Hello world!");
                }

                return Result.Continue<object?>();
            });

        ComponentRegistrationContext.Placeholders.Add(processorProcessorMock);
        ComponentRegistrationContext.Functions.Add(functionResultParserMock);

        return Task.FromResult(Result.Success());
    }
}
