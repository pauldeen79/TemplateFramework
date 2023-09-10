namespace TemplateFramework.Core.Tests.Extensions;

public partial class TemplateEngineExtensionsTests
{
    protected ITemplateEngine TemplateEngineMock { get; } = Substitute.For<ITemplateEngine>();
    protected ITemplateProvider TemplateProviderMock { get; } = Substitute.For<ITemplateProvider>();
    protected IGenerationEnvironment GenerationEnvironmentMock { get; } = Substitute.For<IGenerationEnvironment>();
    protected ITemplateContext ContextMock { get; } = Substitute.For<ITemplateContext>();
    protected ITemplateIdentifier TemplateIdentifierMock { get; } = Substitute.For<ITemplateIdentifier>();

    protected object Template { get; } = new object();
    protected ITemplateIdentifier Identifier => TemplateIdentifierMock;
    protected IEnumerable<object?> Models { get; } = new[] { new object(), new object(), new object() };
    protected object Model { get; } = new object();

    protected const string DefaultFilename = "DefaultFilename.txt";
}
