namespace TemplateFramework.Core.CodeGeneration.Tests;

public partial class CodeGenerationAssemblyTests
{
    public class Generate : CodeGenerationAssemblyTests
    {
        public Generate()
        {
            CodeGenerationProviderCreatorMock.TryCreateInstance(Arg.Any<Type>())
                .Returns(x => x.ArgAt<Type>(0).FullName == typeof(MyGeneratorProvider).FullName
                    ? Activator.CreateInstance(x.ArgAt<Type>(0)) as ICodeGenerationProvider
                    : null);
            AssemblyServiceMock.GetAssembly(Arg.Any<string>(), Arg.Any<string>())
                .Returns(GetType().Assembly);
        }

        [Fact]
        public void Throws_On_Null_Settings()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Generate(settings: null!, GenerationEnvironmentMock))
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
            sut.Generate(new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), currentDirectory: TestData.BasePath), GenerationEnvironmentMock);

            // Assert
            CodeGenerationEngineMock.Received().Generate(Arg.Any<ICodeGenerationProvider>(), Arg.Any<IGenerationEnvironment>(), Arg.Any<ICodeGenerationSettings>());
        }

        [Fact]
        public void Runs_Filtered_CodeGenerators_In_Specified_Assembly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Generate(new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), currentDirectory: TestData.BasePath, classNameFilter: new[] { typeof(MyGeneratorProvider).FullName! }), GenerationEnvironmentMock);

            // Assert
            CodeGenerationEngineMock.Received().Generate(Arg.Any<ICodeGenerationProvider>(), Arg.Any<IGenerationEnvironment>(), Arg.Any<ICodeGenerationSettings>());
        }

        [Fact]
        public void Runs_Filtered_CodeGenerators_In_Specified_Assembly_No_Matches()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Generate(new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), currentDirectory: TestData.BasePath, classNameFilter: new[] { "WrongName" }), GenerationEnvironmentMock);

            // Assert
            CodeGenerationEngineMock.DidNotReceive().Generate(Arg.Any<ICodeGenerationProvider>(), Arg.Any<IGenerationEnvironment>(), Arg.Any<ICodeGenerationSettings>());
        }
    }
}
