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
        var processorProcessorMock = new Mock<IPlaceholderProcessor>();
        processorProcessorMock
            .Setup(x => x.Process(It.IsAny<string>(), It.IsAny<IFormatProvider>(), It.IsAny<object?>(), It.IsAny<IFormattableStringParser>()))
            .Returns<string, IFormatProvider, object?, IFormattableStringParser>((value, _, _, _) => value == "__test" ? Result<string>.Success("Hello world!") : Result<string>.Continue());

        var functionResultParserMock = new Mock<IFunctionResultParser>();
        functionResultParserMock
            .Setup(x => x.Parse(It.IsAny<FunctionParseResult>(), It.IsAny<object?>(), It.IsAny<IFunctionParseResultEvaluator>(), It.IsAny<IExpressionParser>()))
            .Returns<FunctionParseResult, object?, IFunctionParseResultEvaluator, IExpressionParser>((result, _, _, _) =>
            {
                if (result.FunctionName == "MyFunction")
                {
                    return Result<object?>.Success("Hello world!");
                }

                return Result<object?>.Continue();
            });

        ComponentRegistrationContext.PlaceholderProcessors.Add(processorProcessorMock.Object);
        ComponentRegistrationContext.FunctionResultParsers.Add(functionResultParserMock.Object);
    }
}
