namespace TemplateFramework.Core.CodeGeneration.Tests;

public class CodeGenerationEngineTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(CodeGenerationEngine).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }

    public class Generate
    {
        [Theory, AutoMockData]
        public void Throws_On_Null_CodeGenerationProvider(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ICodeGenerationSettings codeGenerationSettings,
            CodeGenerationEngine sut)
        {
            // Act
            sut.Invoking(x => x.Generate(codeGenerationProvider: null!, generationEnvironment, codeGenerationSettings))
               .Should().Throw<ArgumentNullException>().WithParameterName("codeGenerationProvider");
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_GenerationEnvironment(
            [Frozen] ICodeGenerationProvider codeGenerationProvider,
            [Frozen] ICodeGenerationSettings codeGenerationSettings,
            CodeGenerationEngine sut)
        {
            // Act
            sut.Invoking(x => x.Generate(codeGenerationProvider, generationEnvironment: null!, codeGenerationSettings))
               .Should().Throw<ArgumentNullException>().WithParameterName("generationEnvironment");
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_CodeGenerationSettings(
            [Frozen] ICodeGenerationProvider codeGenerationProvider,
            [Frozen] IGenerationEnvironment generationEnvironment,
            CodeGenerationEngine sut)
        {
            // Act
            sut.Invoking(x => x.Generate(codeGenerationProvider, generationEnvironment, settings: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("settings");
        }

        [Theory, AutoMockData]
        public void Saves_Generated_Content_When_BasePath_Is_Filled_And_DryRun_Is_False(
            [Frozen] ICodeGenerationProvider codeGenerationProvider,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ICodeGenerationSettings codeGenerationSettings,
            CodeGenerationEngine sut)
        {
            // Arrange
            codeGenerationProvider.Encoding.Returns(Encoding.Latin1);
            codeGenerationProvider.Path.Returns(TestData.BasePath);
            codeGenerationProvider.GetGeneratorType().Returns(GetType());
            codeGenerationSettings.DryRun.Returns(false);
            codeGenerationSettings.BasePath.Returns(TestData.BasePath);
            codeGenerationSettings.DefaultFilename.Returns("Filename.txt");

            // Act
            sut.Generate(codeGenerationProvider, generationEnvironment, codeGenerationSettings);

            // Assert
            generationEnvironment.Received().SaveContents(codeGenerationProvider, TestData.BasePath, "Filename.txt");
        }

        [Theory, AutoMockData]
        public void Does_Not_Save_Generatd_Content_When_BasePath_Is_Filled_But_DryRun_Is_True(
            [Frozen] ICodeGenerationProvider codeGenerationProvider,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ICodeGenerationSettings codeGenerationSettings,
            CodeGenerationEngine sut)
        {
            // Arrange
            codeGenerationProvider.Encoding.Returns(Encoding.Latin1);
            codeGenerationProvider.Path.Returns(TestData.BasePath);
            codeGenerationProvider.GetGeneratorType().Returns(GetType());
            codeGenerationSettings.DryRun.Returns(true);
            codeGenerationSettings.DefaultFilename.Returns("Filename.txt");

            // Act
            sut.Generate(codeGenerationProvider, generationEnvironment, codeGenerationSettings);

            // Assert
            generationEnvironment.DidNotReceive().SaveContents(codeGenerationProvider, TestData.BasePath, "Filename.txt");
        }

        [Theory, AutoMockData]
        public void Initializes_TemplateProviderPlugin_When_Possible(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ICodeGenerationSettings codeGenerationSettings,
            CodeGenerationEngine sut)
        {
            // Arrange
            var counter = 0;
            var provider = new MyPluginCodeGenerationProvider(_ => counter++);
            codeGenerationSettings.DryRun.Returns(false);
            codeGenerationSettings.BasePath.Returns(TestData.BasePath);
            codeGenerationSettings.DefaultFilename.Returns("Filename.txt");

            // Act
            sut.Generate(provider, generationEnvironment, codeGenerationSettings);

            // Assert
            counter.Should().Be(1);
        }

        [Theory, AutoMockData]
        public void Starts_New_Session_On_TemplateProvider(
            [Frozen] ICodeGenerationProvider codeGenerationProvider,
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ICodeGenerationSettings codeGenerationSettings,
            [Frozen] ITemplateProvider templateProvider,
            CodeGenerationEngine sut)
        {
            // Arrange
            codeGenerationProvider.Encoding.Returns(Encoding.Latin1);
            codeGenerationProvider.Path.Returns(TestData.BasePath);
            codeGenerationProvider.GetGeneratorType().Returns(GetType());
            codeGenerationSettings.DryRun.Returns(true);
            codeGenerationSettings.DefaultFilename.Returns("Filename.txt");

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
