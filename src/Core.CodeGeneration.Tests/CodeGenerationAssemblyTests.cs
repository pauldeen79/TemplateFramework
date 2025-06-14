﻿namespace TemplateFramework.Core.CodeGeneration.Tests;

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

    public class GenerateAsync : CodeGenerationAssemblyTests
    {
        [Fact]
        public async Task Throws_On_Null_Settings()
        {
            // Arrange
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var sut = CreateSut();

            // Act & Assert
            Task t = sut.GenerateAsync(settings: null!, generationEnvironment, CancellationToken.None);
            (await t.ShouldThrowAsync<ArgumentNullException>()).ParamName.ShouldBe("settings");
        }

        [Fact]
        public async Task Throws_On_Null_GenerationEnvironment()
        {
            // Arrange
            var settings = new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), currentDirectory: TestData.BasePath);
            var sut = CreateSut();

            // Act & Assert
            Task t = sut.GenerateAsync(settings, generationEnvironment: null!, CancellationToken.None);
            (await t.ShouldThrowAsync<ArgumentNullException>()).ParamName.ShouldBe("generationEnvironment");
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
            codeGenerationEngine.GenerateAsync(Arg.Any<ICodeGenerationProvider>(), Arg.Any<IGenerationEnvironment>(), Arg.Any<ICodeGenerationSettings>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            var sut = CreateSut();

            // Act
            await sut.GenerateAsync(new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), currentDirectory: TestData.BasePath), generationEnvironment, CancellationToken.None);

            // Assert
            await codeGenerationEngine.Received().GenerateAsync(Arg.Any<ICodeGenerationProvider>(), Arg.Any<IGenerationEnvironment>(), Arg.Any<ICodeGenerationSettings>(), Arg.Any<CancellationToken>());
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
            codeGenerationEngine.GenerateAsync(Arg.Any<ICodeGenerationProvider>(), Arg.Any<IGenerationEnvironment>(), Arg.Any<ICodeGenerationSettings>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            var sut = CreateSut();

            // Act
            await sut.GenerateAsync(new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), currentDirectory: TestData.BasePath, classNameFilter: [typeof(MyGeneratorProvider).FullName!]), generationEnvironment, CancellationToken.None);

            // Assert
            await codeGenerationEngine.Received().GenerateAsync(Arg.Any<ICodeGenerationProvider>(), Arg.Any<IGenerationEnvironment>(), Arg.Any<ICodeGenerationSettings>(), Arg.Any<CancellationToken>());
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
            await sut.GenerateAsync(new CodeGenerationAssemblySettings(TestData.BasePath, "DefaultFilename.txt", TestData.GetAssemblyName(), currentDirectory: TestData.BasePath, classNameFilter: ["WrongName"]), generationEnvironment, CancellationToken.None);

            // Assert
            await codeGenerationEngine.DidNotReceive().GenerateAsync(Arg.Any<ICodeGenerationProvider>(), Arg.Any<IGenerationEnvironment>(), Arg.Any<ICodeGenerationSettings>(), Arg.Any<CancellationToken>());
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
