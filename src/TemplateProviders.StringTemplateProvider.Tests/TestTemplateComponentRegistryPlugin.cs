namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests;

public sealed class TestTemplateComponentRegistryPlugin : ITemplateComponentRegistryPlugin
{
    public ComponentRegistrationContext ComponentRegistrationContext { get; }

    public TestTemplateComponentRegistryPlugin(ComponentRegistrationContext componentRegistrationContext)
    {
        Guard.IsNotNull(componentRegistrationContext);

        ComponentRegistrationContext = componentRegistrationContext;
    }

    public void Initialize(ITemplateComponentRegistry registry)
    {
        var processorProcessorMock = Substitute.For<IPlaceholderProcessor>();
        processorProcessorMock.Process(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>(), Arg.Any<IFormattableStringParser>())
            .Returns(args => args.ArgAt<string>(0) == "__test"
                ? Result<string>.Success("Hello world!")
                : Result<string>.Continue());

        var functionResultParserMock = Substitute.For<IFunctionResultParser>();
        functionResultParserMock.Parse(Arg.Any<FunctionParseResult>(), Arg.Any<object?>(), Arg.Any<IFunctionParseResultEvaluator>(), Arg.Any<IExpressionParser>())
            .Returns(args =>
            {
                if (args.ArgAt<FunctionParseResult>(0).FunctionName == "MyFunction")
                {
                    return Result<object?>.Success("Hello world!");
                }

                return Result<object?>.Continue();
            });

        ComponentRegistrationContext.PlaceholderProcessors.Add(processorProcessorMock);
        ComponentRegistrationContext.FunctionResultParsers.Add(functionResultParserMock);
    }
}
