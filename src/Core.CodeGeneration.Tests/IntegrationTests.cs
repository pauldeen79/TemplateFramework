namespace TemplateFramework.Core.CodeGeneration.Tests;

public class IntegrationTests : TestBase
{
    [Fact]
    public void Can_Generate_Code_Using_CodeGenerationAssembly()
    {
        // Arrange
        var fileSystem = Fixture.Freeze<IFileSystem>();
        var templateFactory = Fixture.Freeze<ITemplateFactory>();
        var templateProviderPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        using var serviceProvider = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkCodeGeneration()
            .AddScoped(_ => fileSystem)
            .AddScoped(_ => templateFactory)
            .AddScoped(_ => templateProviderPluginFactory)
            .BuildServiceProvider(true);
        using var scope = serviceProvider.CreateScope();
        var sut = scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = new IntegrationProvider();
        var builder = new MultipleContentBuilder();
        var generationEnvironment = new MultipleContentBuilderEnvironment(
            scope.ServiceProvider.GetRequiredService<IFileSystem>(),
            scope.ServiceProvider.GetRequiredService<IRetryMechanism>(),
            builder);
        templateFactory.Create(Arg.Any<Type>()).Returns(x => Activator.CreateInstance(x.ArgAt<Type>(0))!);

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
