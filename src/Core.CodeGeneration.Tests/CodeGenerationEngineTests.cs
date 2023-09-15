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
            sut.Invoking(x => x.Generate(codeGenerationProvider: null!, generationEnvironment, codeGenerationSettings))
               .Should().Throw<ArgumentNullException>().WithParameterName("codeGenerationProvider");
        }

        [Fact]
        public void Throws_On_Null_GenerationEnvironment()
        {
            // Arrange
            var codeGenerationProvider = Fixture.Freeze<ICodeGenerationProvider>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var sut = CreateSut();

            // Act
            sut.Invoking(x => x.Generate(codeGenerationProvider, generationEnvironment: null!, codeGenerationSettings))
               .Should().Throw<ArgumentNullException>().WithParameterName("generationEnvironment");
        }

        [Fact]
        public void Throws_On_Null_CodeGenerationSettings()
        {
            // Arrange
            var codeGenerationProvider = Fixture.Freeze<ICodeGenerationProvider>();
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var sut = CreateSut();

            // Act
            sut.Invoking(x => x.Generate(codeGenerationProvider, generationEnvironment, settings: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("settings");
        }

        [Fact]
        public void Saves_Generated_Content_When_BasePath_Is_Filled_And_DryRun_Is_False()
        {
            // Arrange
            var codeGenerationProvider = Fixture.Freeze<ICodeGenerationProvider>();
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            codeGenerationProvider.Encoding.Returns(Encoding.Latin1);
            codeGenerationProvider.Path.Returns(TestData.BasePath);
            codeGenerationProvider.GetGeneratorType().Returns(GetType());
            codeGenerationSettings.DryRun.Returns(false);
            codeGenerationSettings.BasePath.Returns(TestData.BasePath);
            codeGenerationSettings.DefaultFilename.Returns("Filename.txt");
            var sut = CreateSut();

            // Act
            sut.Generate(codeGenerationProvider, generationEnvironment, codeGenerationSettings);

            // Assert
            generationEnvironment.Received().SaveContents(codeGenerationProvider, TestData.BasePath, "Filename.txt");
        }

        [Fact]
        public void Does_Not_Save_Generatd_Content_When_BasePath_Is_Filled_But_DryRun_Is_True()
        {
            // Arrange
            var codeGenerationProvider = Fixture.Freeze<ICodeGenerationProvider>();
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            codeGenerationProvider.Encoding.Returns(Encoding.Latin1);
            codeGenerationProvider.Path.Returns(TestData.BasePath);
            codeGenerationProvider.GetGeneratorType().Returns(GetType());
            codeGenerationSettings.DryRun.Returns(true);
            codeGenerationSettings.DefaultFilename.Returns("Filename.txt");
            var sut = CreateSut();

            // Act
            sut.Generate(codeGenerationProvider, generationEnvironment, codeGenerationSettings);

            // Assert
            generationEnvironment.DidNotReceive().SaveContents(codeGenerationProvider, TestData.BasePath, "Filename.txt");
        }

        [Fact]
        public void Initializes_TemplateProviderPlugin_When_Possible()
        {
            // Arrange
            var counter = 0;
            var provider = new MyPluginCodeGenerationProvider(_ => counter++);
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            codeGenerationSettings.DryRun.Returns(false);
            codeGenerationSettings.BasePath.Returns(TestData.BasePath);
            codeGenerationSettings.DefaultFilename.Returns("Filename.txt");
            var sut = CreateSut();

            // Act
            sut.Generate(provider, generationEnvironment, codeGenerationSettings);

            // Assert
            counter.Should().Be(1);
        }

        [Fact]
        public void Starts_New_Session_On_TemplateProvider()
        {
            // Arrange
            var codeGenerationProvider = Fixture.Freeze<ICodeGenerationProvider>();
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var templateProvider = Fixture.Freeze<ITemplateProvider>();
            codeGenerationProvider.Encoding.Returns(Encoding.Latin1);
            codeGenerationProvider.Path.Returns(TestData.BasePath);
            codeGenerationProvider.GetGeneratorType().Returns(GetType());
            codeGenerationSettings.DryRun.Returns(true);
            codeGenerationSettings.DefaultFilename.Returns("Filename.txt");
            var sut = CreateSut();

            // Act
            sut.Generate(codeGenerationProvider, generationEnvironment, codeGenerationSettings);

            // Assert
            templateProvider.Received().StartSession();
        }

        private sealed class MyPluginCodeGenerationProvider : ICodeGenerationProvider, ITemplateComponentRegistryPlugin
        {
            public string Path => string.Empty;
            public bool RecurseOnDeleteGeneratedFiles => false;
            public string LastGeneratedFilesFilename => string.Empty;
            public Encoding Encoding => Encoding.UTF8;

            public object? CreateAdditionalParameters() => null;
            public Type GetGeneratorType() => typeof(object);
            public object? CreateModel() => null;

            private readonly Action<ITemplateComponentRegistry> _action;

            public MyPluginCodeGenerationProvider(Action<ITemplateComponentRegistry> action) => _action = action;

            public void Initialize(ITemplateComponentRegistry registry) => _action(registry);
        }
    }
}
