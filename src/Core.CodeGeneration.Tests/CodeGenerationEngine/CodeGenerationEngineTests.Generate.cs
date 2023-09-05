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
            sut.Invoking(x => x.Generate(codeGenerationProvider: null!, GenerationEnvironmentMock.Object, CodeGenerationSettingsMock.Object))
               .Should().Throw<ArgumentNullException>().WithParameterName("codeGenerationProvider");
        }

        [Fact]
        public void Throws_On_Null_GenerationEnvironment()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Invoking(x => x.Generate(CodeGenerationProviderMock.Object, generationEnvironment: null!, CodeGenerationSettingsMock.Object))
               .Should().Throw<ArgumentNullException>().WithParameterName("generationEnvironment");
        }

        [Fact]
        public void Throws_On_Null_CodeGenerationSettings()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Invoking(x => x.Generate(CodeGenerationProviderMock.Object, GenerationEnvironmentMock.Object, settings: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("settings");
        }

        [Fact]
        public void Saves_Generated_Content_When_BasePath_Is_Filled_And_DryRun_Is_False()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.Encoding).Returns(Encoding.Latin1);
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.Setup(x => x.GetGeneratorType()).Returns(GetType());
            CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(false);
            CodeGenerationSettingsMock.SetupGet(x => x.BasePath).Returns(TestData.BasePath);
            CodeGenerationSettingsMock.SetupGet(x => x.DefaultFilename).Returns("Filename.txt");

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, GenerationEnvironmentMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            GenerationEnvironmentMock.Verify(x => x.SaveContents(CodeGenerationProviderMock.Object, TestData.BasePath, "Filename.txt"), Times.Once);
        }

        [Fact]
        public void Does_Not_Save_Generatd_Content_When_BasePath_Is_Filled_But_DryRun_Is_True()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.Encoding).Returns(Encoding.Latin1);
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.Setup(x => x.GetGeneratorType()).Returns(GetType());
            CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(true);
            CodeGenerationSettingsMock.SetupGet(x => x.DefaultFilename).Returns("Filename.txt");

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, GenerationEnvironmentMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            GenerationEnvironmentMock.Verify(x => x.SaveContents(CodeGenerationProviderMock.Object, TestData.BasePath, "Filename.txt"), Times.Never);
        }

        [Fact]
        public void Initializes_TemplateProviderPlugin_When_Possible()
        {
            // Arrange
            var sut = CreateSut();
            var counter = 0;
            var provider = new MyPluginCodeGenerationProvider(_ => counter++);
            CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(false);
            CodeGenerationSettingsMock.SetupGet(x => x.BasePath).Returns(TestData.BasePath);
            CodeGenerationSettingsMock.SetupGet(x => x.DefaultFilename).Returns("Filename.txt");

            // Act
            sut.Generate(provider, GenerationEnvironmentMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            counter.Should().Be(1);
        }

        [Fact]
        public void Starts_New_Session_On_TemplateProvider()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.Encoding).Returns(Encoding.Latin1);
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.Setup(x => x.GetGeneratorType()).Returns(GetType());
            CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(true);
            CodeGenerationSettingsMock.SetupGet(x => x.DefaultFilename).Returns("Filename.txt");

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, GenerationEnvironmentMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            TemplateProviderMock.Verify(x => x.StartSession(), Times.Once);
        }

        private sealed class MyPluginCodeGenerationProvider : ICodeGenerationProvider, ITemplateProviderPlugin
        {
            public string Path => string.Empty;
            public bool RecurseOnDeleteGeneratedFiles => false;
            public string LastGeneratedFilesFilename => string.Empty;
            public Encoding Encoding => Encoding.UTF8;

            public object? CreateAdditionalParameters() => null;
            public Type GetGeneratorType() => typeof(object);
            public object? CreateModel() => null;

            private readonly Action<ITemplateProvider> _action;

            public MyPluginCodeGenerationProvider(Action<ITemplateProvider> action) => _action = action;

            public void Initialize(ITemplateProvider provider) => _action(provider);
        }
    }
}
