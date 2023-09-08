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
            .Setup(x => x.Process(It.IsAny<string>(), It.IsAny<IFormatProvider>(), It.IsAny<object?>()))
            .Returns<string, IFormatProvider, object?>((value, _, _) => value == "__test" ? Result<string>.Success("Hello world!") : Result<string>.Continue());

        ComponentRegistrationContext.Processors.Add(processorProcessorMock.Object);
    }
}
