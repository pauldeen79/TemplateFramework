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
            sut.Invoking(x => x.Generate(provider: null!, GenerationEnvironmentMock.Object, CodeGenerationSettingsMock.Object))
               .Should().Throw<ArgumentNullException>().WithParameterName("provider");
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
        public void Processes_Result_When_BasePath_Is_Filled_And_DryRun_Is_False()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns(string.Empty);
            CodeGenerationProviderMock.SetupGet(x => x.Encoding).Returns(Encoding.Latin1);
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns("MyFile.txt");
            CodeGenerationProviderMock.Setup(x => x.CreateGenerator()).Returns(this);
            CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(false);
            CodeGenerationSettingsMock.SetupGet(x => x.BasePath).Returns(TestData.BasePath);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, GenerationEnvironmentMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            GenerationEnvironmentMock.Verify(x => x.Process(CodeGenerationProviderMock.Object, TestData.BasePath), Times.Once);
        }

        [Fact]
        public void Does_Not_Process_Result_When_BasePath_Is_Filled_But_DryRun_Is_True()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns("MyFile.txt");
            CodeGenerationProviderMock.SetupGet(x => x.Encoding).Returns(Encoding.Latin1);
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.Setup(x => x.CreateGenerator()).Returns(this);
            CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(true);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, GenerationEnvironmentMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            GenerationEnvironmentMock.Verify(x => x.Process(CodeGenerationProviderMock.Object, TestData.BasePath), Times.Never);
        }

        [Fact]
        public void Generates_Multiple_Files_When_Provider_Wants_This()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns(string.Empty);
            CodeGenerationProviderMock.Setup(x => x.CreateGenerator()).Returns(this);
            GenerationEnvironmentMock.SetupGet(x => x.Type).Returns(GenerationEnvironmentType.MultipleContentBuilder);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, GenerationEnvironmentMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest<object?>>(x => x.GenerationEnvironment == GenerationEnvironmentMock.Object)), Times.Once);
        }

        [Fact]
        public void Generates_Single_File_When_Provider_Wants_This()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns("MyFile.txt");
            CodeGenerationProviderMock.Setup(x => x.CreateGenerator()).Returns(this);
            GenerationEnvironmentMock.SetupGet(x => x.Type).Returns(GenerationEnvironmentType.StringBuilder);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, GenerationEnvironmentMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest<object?>>(x => x.GenerationEnvironment == GenerationEnvironmentMock.Object)), Times.Once);
        }
    }
}
