namespace TemplateFramework.Core.CodeGeneration.Tests;

public class IntegrationTests
{
    private readonly Mock<IFileSystem> _fileSystemMock = new();
    private readonly Mock<ITemplateFactory> _templateFactoryMock = new();
    private readonly Mock<ITemplateComponentRegistryPluginFactory> _templateProviderPluginFactoryMock = new();

    [Fact]
    public void Can_Generate_Code_Using_CodeGenerationAssembly()
    {
        // Arrange
        using var serviceProvider = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkCodeGeneration()
            .AddScoped(_ => _fileSystemMock.Object)
            .AddScoped(_ => _templateFactoryMock.Object)
            .AddScoped(_ => _templateProviderPluginFactoryMock.Object)
            .BuildServiceProvider();
        var sut = serviceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = new IntegrationProvider();
        var builder = new MultipleContentBuilder();
        var generationEnvironment = new MultipleContentBuilderEnvironment(serviceProvider.GetRequiredService<IFileSystem>(), serviceProvider.GetRequiredService<IRetryMechanism>(), builder);
        _templateFactoryMock.Setup(x => x.Create(It.IsAny<Type>())).Returns<Type>(t => Activator.CreateInstance(t)!);

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
        public Type GetGeneratorType() => typeof(IntegrationTemplate);
        public object? CreateModel() => "Hello world!";
    }

    public sealed class IntegrationTemplate : IMultipleContentBuilderTemplate, IModelContainer<string>
    {
        public string? Model { get; set; }

        public void Render(IMultipleContentBuilder builder)
        {
            var content = builder.AddContent("Filename.txt");
            content.Builder.Append(CultureInfo.InvariantCulture, $"Model is: {Model}");
        }
    }
}
