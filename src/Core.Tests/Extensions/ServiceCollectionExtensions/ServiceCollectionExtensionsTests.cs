namespace TemplateFramework.Core.Tests.Extensions;

public partial class ServiceCollectionExtensionsTests
{
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();
    protected Mock<IGenerationEnvironment> GenerationEnvironmentMock { get; } = new();
    protected Mock<ITemplateContext> ContextMock { get; } = new();

    protected object Template { get; } = new object();
    protected object AdditionalParameters { get; } = new object();
    protected IEnumerable<object?> Models { get; } = new[] { new object(), new object(), new object() };
    protected object? Model { get; } = new object();

    protected const string DefaultFilename = "DefaultFilename.txt";
}
