namespace TemplateFramework.Abstractions.Tests.Extensions.TemplateContextExtensions;

public partial class TemplateContextExtensionsTests
{
    protected Mock<ITemplateContext> CreateSut() => new();
    protected object Template { get; } = new();
    protected object Model { get; } = new();
    protected int? IterationNumber { get; } = 0;
    protected int? IterationCount { get; } = 2;
}
