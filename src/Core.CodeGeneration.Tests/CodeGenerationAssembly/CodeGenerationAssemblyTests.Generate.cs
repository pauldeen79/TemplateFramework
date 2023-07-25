namespace TemplateFramework.Core.CodeGeneration.Tests;

public partial class CodeGenerationAssemblyTests
{
    public class Generate : CodeGenerationAssemblyTests
    {
        public Generate()
        {
            MultipleContentBuilderMock.Setup(x => x.ToString()).Returns("Output");
        }

        [Fact]
        public void Throws_On_Null_Settings()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Generate(settings: null!, MultipleContentBuilderMock.Object))
               .Should().Throw<ArgumentNullException>().WithParameterName("settings");
        }

        [Fact]
        public void Throws_On_Null_GenerationEnvironment()
        {
            // Arrange
            var sut = CreateSut();
            var settings = new CodeGenerationAssemblySettings(TestData.BasePath, TestData.GetAssemblyName(), currentDirectory: TestData.BasePath);

            // Act & Assert
            sut.Invoking(x => x.Generate(settings, generationEnvironment: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("generationEnvironment");
        }

        [Fact]
        public void Runs_All_CodeGenerators_In_Specified_Assembly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Generate(new CodeGenerationAssemblySettings(TestData.BasePath, TestData.GetAssemblyName(), currentDirectory: TestData.BasePath), MultipleContentBuilderMock.Object);

            // Assert
            CodeGenerationEngineMock.Verify(x => x.Generate(It.IsAny<ICodeGenerationProvider>(), It.IsAny<IMultipleContentBuilder>(), It.IsAny<ICodeGenerationSettings>()), Times.Once);
        }

        [Fact]
        public void Runs_Filtered_CodeGenerators_In_Specified_Assembly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Generate(new CodeGenerationAssemblySettings(TestData.BasePath, TestData.GetAssemblyName(), currentDirectory: TestData.BasePath, classNameFilter: new[] { typeof(MyGeneratorProvider).FullName! }), MultipleContentBuilderMock.Object);

            // Assert
            CodeGenerationEngineMock.Verify(x => x.Generate(It.IsAny<ICodeGenerationProvider>(), It.IsAny<IMultipleContentBuilder>(), It.IsAny<ICodeGenerationSettings>()), Times.Once);
        }

        [Fact]
        public void Runs_Filtered_CodeGenerators_In_Specified_Assembly_No_Matches()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Generate(new CodeGenerationAssemblySettings(TestData.BasePath, TestData.GetAssemblyName(), currentDirectory: TestData.BasePath, classNameFilter: new[] { "WrongName" }), MultipleContentBuilderMock.Object);

            // Assert
            CodeGenerationEngineMock.Verify(x => x.Generate(It.IsAny<ICodeGenerationProvider>(), It.IsAny<IMultipleContentBuilder>(), It.IsAny<ICodeGenerationSettings>()), Times.Never);
        }
    }
}
