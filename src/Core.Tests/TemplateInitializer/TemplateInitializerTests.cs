namespace TemplateFramework.Core.Tests;

public partial class TemplateInitializerTests
{
    protected TemplateInitializer CreateSut() => new(new[] { TemplateParameterConverterMock.Object });
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();
    protected Mock<ITemplateParameterConverter> TemplateParameterConverterMock { get; } = new();
    protected const string DefaultFilename = "DefaultFilename.txt";
}
