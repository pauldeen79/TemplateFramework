namespace TemplateFramework.Core.CodeGeneration.Tests;

public class IntegrationTests
{
    private readonly Mock<IFileSystem> _fileSystemMock = new();

    [Fact]
    public void Can_Generate_Code_Using_CodeGenerationAssembly()
    {
        // Arrange
        using var serviceProvider = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkCodeGeneration()
            .AddSingleton(_fileSystemMock.Object)
            .BuildServiceProvider();
        var sut = serviceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = new IntegrationProvider();
        var builder = new MultipleContentBuilder();
        var generationEnvironment = new MultipleContentBuilderEnvironment(serviceProvider.GetRequiredService<IFileSystem>(), builder);

        // Act
        sut.Generate(codeGenerationProvider, generationEnvironment, new CodeGenerationSettings(TestData.BasePath, "DefaultFilename.txt", false));

        // Assert
        builder.Contents.Should().ContainSingle();
        builder.Contents.Single().Build().Contents.Should().Be("Model is: Hello world!");
    }

    private sealed class IntegrationProvider : ICodeGenerationProvider
    {
        public string Path => string.Empty;
        public bool RecurseOnDeleteGeneratedFiles => false;
        public string LastGeneratedFilesFilename => "*.generated.txt";
        public Encoding Encoding => Encoding.UTF8;

        public object? CreateAdditionalParameters() => null;
        public object CreateGenerator() => new IntegrationTemplate();
        public object? CreateModel() => "Hello world!";
    }

    private sealed class IntegrationTemplate : IMultipleContentBuilderTemplate, IModelContainer<string>
    {
        public string? Model { get; set; }

        public void Render(IMultipleContentBuilder builder)
        {
            var content = builder.AddContent("Filename.txt");
            content.Builder.Append(CultureInfo.InvariantCulture, $"Model is: {Model}");
        }
    }
}
