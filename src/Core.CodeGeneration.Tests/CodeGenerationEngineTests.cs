namespace TemplateFramework.Core.CodeGeneration.Tests;

public class CodeGenerationEngineTests : TestBase<CodeGenerationEngine>
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(CodeGenerationEngine).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }

    public class Generate : CodeGenerationEngineTests
    {
        [Fact]
        public void Throws_On_Null_CodeGenerationProvider()
        {
            // Arrange
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var sut = CreateSut();

            // Act
            sut.Awaiting(x => x.Generate(codeGenerationProvider: null!, generationEnvironment, codeGenerationSettings))
               .Should().ThrowAsync<ArgumentNullException>().WithParameterName("codeGenerationProvider");
        }

        [Fact]
        public void Throws_On_Null_GenerationEnvironment()
        {
            // Arrange
            var codeGenerationProvider = Fixture.Freeze<ICodeGenerationProvider>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var sut = CreateSut();

            // Act
            sut.Awaiting(x => x.Generate(codeGenerationProvider, generationEnvironment: null!, codeGenerationSettings))
               .Should().ThrowAsync<ArgumentNullException>().WithParameterName("generationEnvironment");
        }

        [Fact]
        public void Throws_On_Null_CodeGenerationSettings()
        {
            // Arrange
            var codeGenerationProvider = Fixture.Freeze<ICodeGenerationProvider>();
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var sut = CreateSut();

            // Act
            sut.Awaiting(x => x.Generate(codeGenerationProvider, generationEnvironment, settings: null!))
               .Should().ThrowAsync<ArgumentNullException>().WithParameterName("settings");
        }

        [Fact]
        public async Task Saves_Generated_Content_When_BasePath_Is_Filled_And_DryRun_Is_False()
        {
            // Arrange
            var codeGenerationProvider = Fixture.Freeze<ICodeGenerationProvider>();
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var templateEngine = Fixture.Freeze<ITemplateEngine>();
            codeGenerationProvider.Encoding.Returns(Encoding.Latin1);
            codeGenerationProvider.Path.Returns(TestData.BasePath);
            codeGenerationProvider.GetGeneratorType().Returns(GetType());
            codeGenerationSettings.DryRun.Returns(false);
            codeGenerationSettings.BasePath.Returns(TestData.BasePath);
            codeGenerationSettings.DefaultFilename.Returns("Filename.txt");
            templateEngine.Render(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            var sut = CreateSut();

            // Act
            await sut.Generate(codeGenerationProvider, generationEnvironment, codeGenerationSettings);

            // Assert
            await generationEnvironment.Received().SaveContents(codeGenerationProvider, TestData.BasePath, "Filename.txt", CancellationToken.None);
        }

        [Fact]
        public async Task Does_Not_Save_Generated_Content_When_BasePath_Is_Filled_But_DryRun_Is_True()
        {
            // Arrange
            var codeGenerationProvider = Fixture.Freeze<ICodeGenerationProvider>();
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var templateEngine = Fixture.Freeze<ITemplateEngine>();
            codeGenerationProvider.Encoding.Returns(Encoding.Latin1);
            codeGenerationProvider.Path.Returns(TestData.BasePath);
            codeGenerationProvider.GetGeneratorType().Returns(GetType());
            codeGenerationSettings.DryRun.Returns(true);
            codeGenerationSettings.DefaultFilename.Returns("Filename.txt");
            templateEngine.Render(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            var sut = CreateSut();

            // Act
            await sut.Generate(codeGenerationProvider, generationEnvironment, codeGenerationSettings);

            // Assert
            await generationEnvironment.DidNotReceive().SaveContents(codeGenerationProvider, TestData.BasePath, "Filename.txt", CancellationToken.None);
        }

        [Fact]
        public async Task Initializes_TemplateProviderPlugin_When_Possible()
        {
            // Arrange
            var counter = 0;
            var provider = new MyPluginCodeGenerationProvider(_ => counter++);
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var templateEngine = Fixture.Freeze<ITemplateEngine>();
            codeGenerationSettings.DryRun.Returns(false);
            codeGenerationSettings.BasePath.Returns(TestData.BasePath);
            codeGenerationSettings.DefaultFilename.Returns("Filename.txt");
            templateEngine.Render(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            var sut = CreateSut();

            // Act
            await sut.Generate(provider, generationEnvironment, codeGenerationSettings);

            // Assert
            counter.Should().Be(1);
        }

        [Fact]
        public async Task Starts_New_Session_On_TemplateProvider()
        {
            // Arrange
            var codeGenerationProvider = Fixture.Freeze<ICodeGenerationProvider>();
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var templateProvider = Fixture.Freeze<ITemplateProvider>();
            var templateEngine = Fixture.Freeze<ITemplateEngine>();
            codeGenerationProvider.Encoding.Returns(Encoding.Latin1);
            codeGenerationProvider.Path.Returns(TestData.BasePath);
            codeGenerationProvider.GetGeneratorType().Returns(GetType());
            codeGenerationSettings.DryRun.Returns(true);
            codeGenerationSettings.DefaultFilename.Returns("Filename.txt");
            var sut = CreateSut();
            templateEngine.Render(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>()).Returns(Result.Success());

            // Act
            await sut.Generate(codeGenerationProvider, generationEnvironment, codeGenerationSettings);

            // Assert
            await templateProvider.Received().StartSession(CancellationToken.None);
        }

        private sealed class MyPluginCodeGenerationProvider : ICodeGenerationProvider, ITemplateComponentRegistryPlugin
        {
            public string Path => string.Empty;
            public bool RecurseOnDeleteGeneratedFiles => false;
            public string LastGeneratedFilesFilename => string.Empty;
            public Encoding Encoding => Encoding.UTF8;

            public Task<object?> CreateAdditionalParameters() => Task.FromResult(default(object?));
            public Type GetGeneratorType() => typeof(object);
            public Task<object?> CreateModel() => Task.FromResult(default(object?));

            private readonly Action<ITemplateComponentRegistry> _action;

            public MyPluginCodeGenerationProvider(Action<ITemplateComponentRegistry> action) => _action = action;

            public Task Initialize(ITemplateComponentRegistry registry, CancellationToken cancellationToken) { _action(registry); return Task.CompletedTask; }
        }
    }
}
