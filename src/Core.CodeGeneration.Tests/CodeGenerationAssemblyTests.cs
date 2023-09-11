namespace TemplateFramework.Core.CodeGeneration.Tests;

public class CodeGenerationAssemblyTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            typeof(CodeGenerationAssembly).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }

    public class Generate
    {
        [Theory, AutoMockData]
        public void Throws_On_Null_Settings(
            [Frozen] IGenerationEnvironment generationEnvironment,
            CodeGenerationAssembly sut) 
        {
            // Act & Assert
            sut.Invoking(x => x.Generate(settings: null!, generationEnvironment))
               .Should().Throw<ArgumentNullException>().WithParameterName("settings");
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_GenerationEnvironment(CodeGenerationAssembly sut)
        {
            // Arrange
            var settings = new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), currentDirectory: TestData.BasePath);

            // Act & Assert
            sut.Invoking(x => x.Generate(settings, generationEnvironment: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("generationEnvironment");
        }

        [Theory, AutoMockData]
        public void Runs_All_CodeGenerators_In_Specified_Assembly(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ICodeGenerationEngine codeGenerationEngine,
            [Frozen] IAssemblyService assemblyService,
            [Frozen] ICodeGenerationProviderCreator codeGenerationProviderCreator,
            CodeGenerationAssembly sut)
        {
            // Arrange
            SetupAssemblyService(assemblyService);
            SetupCodeGenerationProviderCreator(codeGenerationProviderCreator);

            // Act
            sut.Generate(new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), currentDirectory: TestData.BasePath), generationEnvironment);

            // Assert
            codeGenerationEngine.Received().Generate(Arg.Any<ICodeGenerationProvider>(), Arg.Any<IGenerationEnvironment>(), Arg.Any<ICodeGenerationSettings>());
        }

        [Theory, AutoMockData]
        public void Runs_Filtered_CodeGenerators_In_Specified_Assembly(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ICodeGenerationEngine codeGenerationEngine,
            [Frozen] IAssemblyService assemblyService,
            [Frozen] ICodeGenerationProviderCreator codeGenerationProviderCreator,
            CodeGenerationAssembly sut)
        {
            // Arrange
            SetupAssemblyService(assemblyService);
            SetupCodeGenerationProviderCreator(codeGenerationProviderCreator);

            // Act
            sut.Generate(new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), currentDirectory: TestData.BasePath, classNameFilter: new[] { typeof(MyGeneratorProvider).FullName! }), generationEnvironment);

            // Assert
            codeGenerationEngine.Received().Generate(Arg.Any<ICodeGenerationProvider>(), Arg.Any<IGenerationEnvironment>(), Arg.Any<ICodeGenerationSettings>());
        }

        [Theory, AutoMockData]
        public void Runs_Filtered_CodeGenerators_In_Specified_Assembly_No_Matches(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ICodeGenerationEngine codeGenerationEngine,
            [Frozen] IAssemblyService assemblyService,
            [Frozen] ICodeGenerationProviderCreator codeGenerationProviderCreator,
            CodeGenerationAssembly sut)
        {
            // Arrange
            SetupAssemblyService(assemblyService);
            SetupCodeGenerationProviderCreator(codeGenerationProviderCreator);

            // Act
            sut.Generate(new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), currentDirectory: TestData.BasePath, classNameFilter: new[] { "WrongName" }), generationEnvironment);

            // Assert
            codeGenerationEngine.DidNotReceive().Generate(Arg.Any<ICodeGenerationProvider>(), Arg.Any<IGenerationEnvironment>(), Arg.Any<ICodeGenerationSettings>());
        }

        private void SetupCodeGenerationProviderCreator(ICodeGenerationProviderCreator codeGenerationProviderCreator)
        {
            codeGenerationProviderCreator
                .TryCreateInstance(Arg.Any<Type>())
                .Returns(x => x.ArgAt<Type>(0).FullName == typeof(MyGeneratorProvider).FullName
                    ? Activator.CreateInstance(x.ArgAt<Type>(0)) as ICodeGenerationProvider
                    : null);
        }

        private void SetupAssemblyService(IAssemblyService assemblyService)
        {
            assemblyService
                .GetAssembly(Arg.Any<string>(), Arg.Any<string>())
                .Returns(GetType().Assembly);
        }
    }
}
