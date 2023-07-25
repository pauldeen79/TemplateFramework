namespace TemplateFramework.Core.CodeGeneration.Tests;

public partial class CodeGenerationEngineTests
{
    public class Generate : CodeGenerationEngineTests
    {
        public Generate()
        {
            MultipleContentBuilderMock.Setup(x => x.ToString()).Returns("Output");
        }

        [Fact]
        public void Throws_On_Null_CodeGenerationProvider()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Invoking(x => x.Generate(provider: null!, MultipleContentBuilderMock.Object, CodeGenerationSettingsMock.Object))
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
            sut.Invoking(x => x.Generate(CodeGenerationProviderMock.Object, MultipleContentBuilderMock.Object, settings: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("settings");
        }

        [Fact]
        public void Saves_Result_When_BasePath_Is_Filled_And_DryRun_Is_False()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns(string.Empty);
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns("MyFile.txt");
            CodeGenerationProviderMock.Setup(x => x.CreateGenerator()).Returns(this);
            CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(false);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, MultipleContentBuilderMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            MultipleContentBuilderMock.Verify(x => x.SaveAll(), Times.Once);
        }

        [Fact]
        public void Does_Not_Save_Result_When_BasePath_Is_Filled_But_DryRun_Is_True()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns("MyFile.txt");
            CodeGenerationProviderMock.Setup(x => x.CreateGenerator()).Returns(this);
            CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(true);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, MultipleContentBuilderMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            MultipleContentBuilderMock.Verify(x => x.SaveAll(), Times.Never);
        }

        [Fact]
        public void Deletes_Previous_LastGeneratedFiles_File_When_Provider_Has_NonEmpty_LastGeneratedFilesFilename()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns(string.Empty);
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.LastGeneratedFilesFilename).Returns("GeneratedFiles.txt");
            CodeGenerationProviderMock.Setup(x => x.CreateGenerator()).Returns(this);
            CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(false);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, MultipleContentBuilderMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            MultipleContentBuilderMock.Verify(x => x.DeleteLastGeneratedFiles(Path.Combine(TestData.BasePath, "GeneratedFiles.txt"), false), Times.Once);
        }

        [Fact]
        public void Does_Not_Delete_Previous_LastGeneratedFiles_File_When_Provider_Has_Empty_LastGeneratedFilesFilename()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns(string.Empty);
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.LastGeneratedFilesFilename).Returns(string.Empty);
            CodeGenerationProviderMock.Setup(x => x.CreateGenerator()).Returns(this);
            CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(false);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, MultipleContentBuilderMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            MultipleContentBuilderMock.Verify(x => x.DeleteLastGeneratedFiles(Path.Combine(TestData.BasePath, "GeneratedFiles.txt"), false), Times.Never);
        }

        [Fact]
        public void Writes_Next_LastGeneratedFiles_File_When_Provider_Has_NonEmpty_LastGeneratedFilesFilename()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns(string.Empty);
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.LastGeneratedFilesFilename).Returns("GeneratedFiles.txt");
            CodeGenerationProviderMock.Setup(x => x.CreateGenerator()).Returns(this);
            CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(false);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, MultipleContentBuilderMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            MultipleContentBuilderMock.Verify(x => x.SaveLastGeneratedFiles(Path.Combine(TestData.BasePath, "GeneratedFiles.txt")), Times.Once);
        }

        [Fact]
        public void Does_Not_Write_Next_LastGeneratedFiles_File_When_Provider_Has_Empty_LastGeneratedFilesFilename()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns(string.Empty);
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.LastGeneratedFilesFilename).Returns(string.Empty);
            CodeGenerationProviderMock.Setup(x => x.CreateGenerator()).Returns(this);
            CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(false);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, MultipleContentBuilderMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            MultipleContentBuilderMock.Verify(x => x.SaveLastGeneratedFiles(Path.Combine(TestData.BasePath, "GeneratedFiles.txt")), Times.Never);
        }

        [Fact]
        public void Generates_Multiple_Files_When_Provider_Wants_This()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns(string.Empty);
            CodeGenerationProviderMock.Setup(x => x.CreateGenerator()).Returns(this);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, MultipleContentBuilderMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.IsAny<IRenderTemplateRequest<object?>>()), Times.Once);
        }

        [Fact]
        public void Generates_Single_File_When_Provider_Wants_This()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns("MyFile.txt");
            CodeGenerationProviderMock.Setup(x => x.CreateGenerator()).Returns(this);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, MultipleContentBuilderMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.IsAny<IRenderTemplateRequest<object?>>()), Times.Once);
        }
    }
}
