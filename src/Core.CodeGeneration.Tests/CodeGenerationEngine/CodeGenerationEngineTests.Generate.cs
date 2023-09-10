namespace TemplateFramework.Core.CodeGeneration.Tests;

public partial class CodeGenerationEngineTests
{
    public class Generate : CodeGenerationEngineTests
    {
        [Fact]
        public void Throws_On_Null_CodeGenerationProvider()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Invoking(x => x.Generate(codeGenerationProvider: null!, GenerationEnvironmentMock, CodeGenerationSettingsMock))
               .Should().Throw<ArgumentNullException>().WithParameterName("codeGenerationProvider");
        }

        [Fact]
        public void Throws_On_Null_GenerationEnvironment()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Invoking(x => x.Generate(CodeGenerationProviderMock, generationEnvironment: null!, CodeGenerationSettingsMock))
               .Should().Throw<ArgumentNullException>().WithParameterName("generationEnvironment");
        }

        [Fact]
        public void Throws_On_Null_CodeGenerationSettings()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Invoking(x => x.Generate(CodeGenerationProviderMock, GenerationEnvironmentMock, settings: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("settings");
        }

        [Fact]
        public void Saves_Generated_Content_When_BasePath_Is_Filled_And_DryRun_Is_False()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.Encoding.Returns(Encoding.Latin1);
            CodeGenerationProviderMock.Path.Returns(TestData.BasePath);
            CodeGenerationProviderMock.GetGeneratorType().Returns(GetType());
            CodeGenerationSettingsMock.DryRun.Returns(false);
            CodeGenerationSettingsMock.BasePath.Returns(TestData.BasePath);
            CodeGenerationSettingsMock.DefaultFilename.Returns("Filename.txt");

            // Act
            sut.Generate(CodeGenerationProviderMock, GenerationEnvironmentMock, CodeGenerationSettingsMock);

            // Assert
            GenerationEnvironmentMock.Received().SaveContents(CodeGenerationProviderMock, TestData.BasePath, "Filename.txt");
        }

        [Fact]
        public void Does_Not_Save_Generatd_Content_When_BasePath_Is_Filled_But_DryRun_Is_True()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.Encoding.Returns(Encoding.Latin1);
            CodeGenerationProviderMock.Path.Returns(TestData.BasePath);
            CodeGenerationProviderMock.GetGeneratorType().Returns(GetType());
            CodeGenerationSettingsMock.DryRun.Returns(true);
            CodeGenerationSettingsMock.DefaultFilename.Returns("Filename.txt");

            // Act
            sut.Generate(CodeGenerationProviderMock, GenerationEnvironmentMock, CodeGenerationSettingsMock);

            // Assert
            GenerationEnvironmentMock.DidNotReceive().SaveContents(CodeGenerationProviderMock, TestData.BasePath, "Filename.txt");
        }

        [Fact]
        public void Initializes_TemplateProviderPlugin_When_Possible()
        {
            // Arrange
            var sut = CreateSut();
            var counter = 0;
            var provider = new MyPluginCodeGenerationProvider(_ => counter++);
            CodeGenerationSettingsMock.DryRun.Returns(false);
            CodeGenerationSettingsMock.BasePath.Returns(TestData.BasePath);
            CodeGenerationSettingsMock.DefaultFilename.Returns("Filename.txt");

            // Act
            sut.Generate(provider, GenerationEnvironmentMock, CodeGenerationSettingsMock);

            // Assert
            counter.Should().Be(1);
        }

        [Fact]
        public void Starts_New_Session_On_TemplateProvider()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.Encoding.Returns(Encoding.Latin1);
            CodeGenerationProviderMock.Path.Returns(TestData.BasePath);
            CodeGenerationProviderMock.GetGeneratorType().Returns(GetType());
            CodeGenerationSettingsMock.DryRun.Returns(true);
            CodeGenerationSettingsMock.DefaultFilename.Returns("Filename.txt");

            // Act
            sut.Generate(CodeGenerationProviderMock, GenerationEnvironmentMock, CodeGenerationSettingsMock);

            // Assert
            TemplateProviderMock.Received().StartSession();
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
