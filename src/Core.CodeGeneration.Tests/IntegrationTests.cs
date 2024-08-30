namespace TemplateFramework.Core.CodeGeneration.Tests;

public class IntegrationTests : TestBase
{
    [Fact]
    public async Task Can_Generate_Code_Using_CodeGenerationAssembly()
    {
        // Arrange
        var fileSystem = Fixture.Freeze<IFileSystem>();
        var templateProviderPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        using var serviceProvider = new ServiceCollection()
            .AddTemplateFramework()
            .AddTemplateFrameworkCodeGeneration()
            .AddTemplateFrameworkRuntime()
            .AddScoped(_ => fileSystem)
            .AddScoped(_ => templateProviderPluginFactory)
            .AddTransient<IntegrationTemplate>()
            .BuildServiceProvider(true);
        using var scope = serviceProvider.CreateScope();
        var sut = scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();
        var codeGenerationProvider = new IntegrationProvider();
        var builder = new MultipleContentBuilder();
        var generationEnvironment = new MultipleContentBuilderEnvironment<StringBuilder>(
            scope.ServiceProvider.GetRequiredService<IFileSystem>(),
            scope.ServiceProvider.GetRequiredService<IRetryMechanism>(),
            builder);

        // Act
        await sut.Generate(codeGenerationProvider, generationEnvironment, new CodeGenerationSettings(TestData.BasePath, "DefaultFilename.txt", false));

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

        public Task<object?> CreateAdditionalParameters() => Task.FromResult(default(object?));
        public Type GetGeneratorType() => typeof(IntegrationTemplate);
        public Task<object?> CreateModel() => Task.FromResult<object?>("Hello world!");
    }

    public sealed class IntegrationTemplate : IMultipleContentBuilderTemplate, IModelContainer<string>
    {
        public string? Model { get; set; }

        public Task Render(IMultipleContentBuilder<StringBuilder> builder, CancellationToken cancellationToken)
        {
            var content = builder.AddContent("Filename.txt");
            content.Builder.Append(CultureInfo.InvariantCulture, $"Model is: {Model}");

            return Task.CompletedTask;
        }
    }
}
