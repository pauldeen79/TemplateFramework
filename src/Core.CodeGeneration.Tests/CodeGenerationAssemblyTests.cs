namespace TemplateFramework.Core.CodeGeneration.Tests;

public class CodeGenerationAssemblyTests : TestBase<CodeGenerationAssembly>
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(CodeGenerationAssembly).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }

    public class Generate : CodeGenerationAssemblyTests
    {
        [Fact]
        public void Throws_On_Null_Settings() 
        {
            // Arrange
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Generate(settings: null!, generationEnvironment))
               .Should().ThrowAsync<ArgumentNullException>().WithParameterName("settings");
        }

        [Fact]
        public void Throws_On_Null_GenerationEnvironment()
        {
            // Arrange
            var settings = new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), currentDirectory: TestData.BasePath);
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Generate(settings, generationEnvironment: null!))
               .Should().ThrowAsync<ArgumentNullException>().WithParameterName("generationEnvironment");
        }

        [Fact]
        public async Task Runs_All_CodeGenerators_In_Specified_Assembly()
        {
            // Arrange
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationEngine = Fixture.Freeze<ICodeGenerationEngine>();
            var assemblyService = Fixture.Freeze<IAssemblyService>();
            var codeGenerationProviderCreator = Fixture.Freeze<ICodeGenerationProviderCreator>();
            SetupAssemblyService(assemblyService);
            SetupCodeGenerationProviderCreator(codeGenerationProviderCreator);
            var sut = CreateSut();

            // Act
            await sut.Generate(new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), currentDirectory: TestData.BasePath), generationEnvironment);

            // Assert
            await codeGenerationEngine.Received().Generate(Arg.Any<ICodeGenerationProvider>(), Arg.Any<IGenerationEnvironment>(), Arg.Any<ICodeGenerationSettings>());
        }

        [Fact]
        public async Task Runs_Filtered_CodeGenerators_In_Specified_Assembly()
        {
            // Arrange
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationEngine = Fixture.Freeze<ICodeGenerationEngine>();
            var assemblyService = Fixture.Freeze<IAssemblyService>();
            var codeGenerationProviderCreator = Fixture.Freeze<ICodeGenerationProviderCreator>();
            SetupAssemblyService(assemblyService);
            SetupCodeGenerationProviderCreator(codeGenerationProviderCreator);
            var sut = CreateSut();

            // Act
            await sut.Generate(new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), currentDirectory: TestData.BasePath, classNameFilter: new[] { typeof(MyGeneratorProvider).FullName! }), generationEnvironment);

            // Assert
            await codeGenerationEngine.Received().Generate(Arg.Any<ICodeGenerationProvider>(), Arg.Any<IGenerationEnvironment>(), Arg.Any<ICodeGenerationSettings>());
        }

        [Fact]
        public async Task Runs_Filtered_CodeGenerators_In_Specified_Assembly_No_Matches()
        {
            // Arrange
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationEngine = Fixture.Freeze<ICodeGenerationEngine>();
            var assemblyService = Fixture.Freeze<IAssemblyService>();
            var codeGenerationProviderCreator = Fixture.Freeze<ICodeGenerationProviderCreator>();
            SetupAssemblyService(assemblyService);
            SetupCodeGenerationProviderCreator(codeGenerationProviderCreator);
            var sut = CreateSut();

            // Act
            await sut.Generate(new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), currentDirectory: TestData.BasePath, classNameFilter: new[] { "WrongName" }), generationEnvironment);

            // Assert
            await codeGenerationEngine.DidNotReceive().Generate(Arg.Any<ICodeGenerationProvider>(), Arg.Any<IGenerationEnvironment>(), Arg.Any<ICodeGenerationSettings>());
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
