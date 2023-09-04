namespace TemplateFramework.Core.CodeGeneration.Tests;

public partial class CodeGenerationAssemblyTests
{
    public class Generate : CodeGenerationAssemblyTests
    {
        public Generate()
        {
            CodeGenerationProviderCreatorMock
                .Setup(x => x.TryCreateInstance(It.IsAny<Type>()))
                .Returns<Type>(t => t.FullName == typeof(MyGeneratorProvider).FullName
                    ? Activator.CreateInstance(t) as ICodeGenerationProvider
                    : null);
            AssemblyServiceMock
                .Setup(x => x.GetAssembly(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(GetType().Assembly);
        }

        [Fact]
        public void Throws_On_Null_Settings()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Generate(settings: null!, GenerationEnvironmentMock.Object))
               .Should().Throw<ArgumentNullException>().WithParameterName("settings");
        }

        [Fact]
        public void Throws_On_Null_GenerationEnvironment()
        {
            // Arrange
            var sut = CreateSut();
            var settings = new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), currentDirectory: TestData.BasePath);

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
            sut.Generate(new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), currentDirectory: TestData.BasePath), GenerationEnvironmentMock.Object);

            // Assert
            CodeGenerationEngineMock.Verify(x => x.Generate(It.IsAny<ICodeGenerationProvider>(), It.IsAny<ITemplateProvider>(), It.IsAny<IGenerationEnvironment>(), It.IsAny<ICodeGenerationSettings>()), Times.Once);
        }

        [Fact]
        public void Runs_Filtered_CodeGenerators_In_Specified_Assembly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Generate(new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), currentDirectory: TestData.BasePath, classNameFilter: new[] { typeof(MyGeneratorProvider).FullName! }), GenerationEnvironmentMock.Object);

            // Assert
            CodeGenerationEngineMock.Verify(x => x.Generate(It.IsAny<ICodeGenerationProvider>(), It.IsAny<ITemplateProvider>(), It.IsAny<IGenerationEnvironment>(), It.IsAny<ICodeGenerationSettings>()), Times.Once);
        }

        [Fact]
        public void Runs_Filtered_CodeGenerators_In_Specified_Assembly_No_Matches()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Generate(new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), currentDirectory: TestData.BasePath, classNameFilter: new[] { "WrongName" }), GenerationEnvironmentMock.Object);

            // Assert
            CodeGenerationEngineMock.Verify(x => x.Generate(It.IsAny<ICodeGenerationProvider>(), It.IsAny<ITemplateProvider>(), It.IsAny<IGenerationEnvironment>(), It.IsAny<ICodeGenerationSettings>()), Times.Never);
        }
    }
}
