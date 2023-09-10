namespace TemplateFramework.Core.CodeGeneration.Tests;

public class IntegrationTests
{
    private readonly IFileSystem _fileSystemMock = Substitute.For<IFileSystem>();
    private readonly ITemplateFactory _templateFactoryMock = Substitute.For<ITemplateFactory>();
    private readonly ITemplateComponentRegistryPluginFactory _templateProviderPluginFactoryMock = Substitute.For<ITemplateComponentRegistryPluginFactory>();

    [Fact]
    public void Can_Generate_Code_Using_CodeGenerationAssembly()
    {
        // Arrange
        using var serviceProvider = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkCodeGeneration()
            .AddScoped(_ => _fileSystemMock)
            .AddScoped(_ => _templateFactoryMock)
            .AddScoped(_ => _templateProviderPluginFactoryMock)
            .BuildServiceProvider(true);
        using var scope = serviceProvider.CreateScope();
        var sut = scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = new IntegrationProvider();
        var builder = new MultipleContentBuilder();
        var generationEnvironment = new MultipleContentBuilderEnvironment(scope.ServiceProvider.GetRequiredService<IFileSystem>(), scope.ServiceProvider.GetRequiredService<IRetryMechanism>(), builder);
        _templateFactoryMock.Create(Arg.Any<Type>()).Returns(x => Activator.CreateInstance(x.ArgAt<Type>(0))!);

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
