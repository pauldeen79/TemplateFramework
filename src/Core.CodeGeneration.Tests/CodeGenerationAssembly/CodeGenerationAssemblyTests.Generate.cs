namespace TemplateFramework.Core.CodeGeneration.Tests;

public partial class CodeGenerationAssemblyTests
{
    public class Generate : CodeGenerationAssemblyTests
    {
        public Generate()
        {
            var templateFileManagerMock = new Mock<ITemplateFileManager>();
            TemplateFileManagerFactoryMock.Setup(x => x.Create()).Returns(templateFileManagerMock.Object);
            var multipleConentBuilderMock = new Mock<IMultipleContentBuilder>();
            templateFileManagerMock.SetupGet(x => x.MultipleContentBuilder).Returns(multipleConentBuilderMock.Object);
            multipleConentBuilderMock.Setup(x => x.ToString()).Returns("Output");
        }

        [Fact]
        public void Runs_All_CodeGenerators_In_Specified_Assembly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            _ = sut.Generate(new CodeGenerationAssemblySettings(TestData.BasePath, TestData.GetAssemblyName(), currentDirectory: TestData.BasePath));

            // Assert
            CodeGenerationEngineMock.Verify(x => x.Generate(It.IsAny<ICodeGenerationProvider>(), It.IsAny<ITemplateFileManager>(), It.Is<ICodeGenerationSettings>(x => x.BasePath == TestData.BasePath)), Times.Once);
        }

        [Fact]
        public void Runs_Filtered_CodeGenerators_In_Specified_Assembly()
        {
            // Arrange
            var sut = new CodeGenerationAssembly(CodeGenerationEngineMock.Object, TemplateFileManagerFactoryMock.Object);

            // Act
            _ = sut.Generate(new CodeGenerationAssemblySettings(TestData.BasePath, TestData.GetAssemblyName(), currentDirectory: TestData.BasePath, classNameFilter: new[] { typeof(MyGeneratorProvider).FullName! }));

            // Assert
            CodeGenerationEngineMock.Verify(x => x.Generate(It.IsAny<ICodeGenerationProvider>(), It.IsAny<ITemplateFileManager>(), It.Is<ICodeGenerationSettings>(x => x.BasePath == TestData.BasePath)), Times.Once);
        }

        [Fact]
        public void Runs_Filtered_CodeGenerators_In_Specified_Assembly_No_Matches()
        {
            // Arrange
            var sut = new CodeGenerationAssembly(CodeGenerationEngineMock.Object, TemplateFileManagerFactoryMock.Object);

            // Act
            _ = sut.Generate(new CodeGenerationAssemblySettings(TestData.BasePath, TestData.GetAssemblyName(), currentDirectory: TestData.BasePath, classNameFilter: new[] { "WrongName" }));

            // Assert
            CodeGenerationEngineMock.Verify(x => x.Generate(It.IsAny<ICodeGenerationProvider>(), It.IsAny<ITemplateFileManager>(), It.Is<ICodeGenerationSettings>(x => x.BasePath == TestData.BasePath)), Times.Never);
        }

        [Fact]
        public void Returns_Output()
        {
            // Arrange
            var sut = new CodeGenerationAssembly(CodeGenerationEngineMock.Object, TemplateFileManagerFactoryMock.Object);

            // Act
            var result = sut.Generate(new CodeGenerationAssemblySettings(TestData.BasePath, TestData.GetAssemblyName(), currentDirectory: TestData.BasePath));

            // Assert
            result.Should().Be(@"Output");
        }
    }
}
