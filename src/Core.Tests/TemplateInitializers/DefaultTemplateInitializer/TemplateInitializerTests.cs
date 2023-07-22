namespace TemplateFramework.Core.Tests.TemplateInitializers;

public partial class TemplateInitializerTests
{
    protected DefaultTemplateInitializer CreateSut() => new(new[] { TemplateParameterConverterMock.Object });
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();
    protected Mock<ITemplateParameterConverter> TemplateParameterConverterMock { get; } = new();
    protected const string DefaultFilename = "DefaultFilename.txt";
}
