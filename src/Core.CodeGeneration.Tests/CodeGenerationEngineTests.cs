﻿namespace TemplateFramework.Core.CodeGeneration.Tests;

public class CodeGenerationEngineTests : TestBase<CodeGenerationEngine>
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(CodeGenerationEngine).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }

    public class GenerateAsync : CodeGenerationEngineTests
    {
        [Fact]
        public async Task Throws_On_Null_CodeGenerationProvider()
        {
            // Arrange
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var sut = CreateSut();

            // Act
            Task t = sut.GenerateAsync(codeGenerationProvider: null!, generationEnvironment, codeGenerationSettings);
            (await t.ShouldThrowAsync<ArgumentNullException>()).ParamName.ShouldBe("codeGenerationProvider");
        }

        [Fact]
        public async Task Throws_On_Null_GenerationEnvironment()
        {
            // Arrange
            var codeGenerationProvider = Fixture.Freeze<ICodeGenerationProvider>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var sut = CreateSut();

            // Act
            Task t = sut.GenerateAsync(codeGenerationProvider, generationEnvironment: null!, codeGenerationSettings);
            (await t.ShouldThrowAsync<ArgumentNullException>()).ParamName.ShouldBe("generationEnvironment");
        }

        [Fact]
        public async Task Throws_On_Null_CodeGenerationSettings()
        {
            // Arrange
            var codeGenerationProvider = Fixture.Freeze<ICodeGenerationProvider>();
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var sut = CreateSut();

            // Act
            Task t = sut.GenerateAsync(codeGenerationProvider, generationEnvironment, settings: null!);
            (await t.ShouldThrowAsync<ArgumentNullException>()).ParamName.ShouldBe("settings");
        }

        [Fact]
        public async Task Saves_Generated_Content_When_BasePath_Is_Filled_And_DryRun_Is_False()
        {
            // Arrange
            var codeGenerationProvider = Fixture.Freeze<ICodeGenerationProvider>();
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var templateEngine = Fixture.Freeze<ITemplateEngine>();
            var templateProvider = Fixture.Freeze<ITemplateProvider>();
            codeGenerationProvider.Encoding.Returns(Encoding.Latin1);
            codeGenerationProvider.Path.Returns(TestData.BasePath);
            codeGenerationProvider.GetGeneratorType().Returns(GetType());
            codeGenerationProvider.CreateModelAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(Result.Continue<object?>()));
            codeGenerationProvider.CreateAdditionalParametersAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(Result.Continue<object?>()));
            codeGenerationSettings.DryRun.Returns(false);
            codeGenerationSettings.BasePath.Returns(TestData.BasePath);
            codeGenerationSettings.DefaultFilename.Returns("Filename.txt");
            templateEngine.RenderAsync(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            templateProvider.StartSessionAsync(Arg.Any<CancellationToken>()).Returns(Result.Continue());
            var sut = CreateSut();

            // Act
            await sut.GenerateAsync(codeGenerationProvider, generationEnvironment, codeGenerationSettings);

            // Assert
            await generationEnvironment.Received().SaveContentsAsync(codeGenerationProvider, TestData.BasePath, "Filename.txt", CancellationToken.None);
        }

        [Fact]
        public async Task Does_Not_Save_Generated_Content_When_BasePath_Is_Filled_But_DryRun_Is_True()
        {
            // Arrange
            var codeGenerationProvider = Fixture.Freeze<ICodeGenerationProvider>();
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var templateEngine = Fixture.Freeze<ITemplateEngine>();
            var templateProvider = Fixture.Freeze<ITemplateProvider>();
            codeGenerationProvider.Encoding.Returns(Encoding.Latin1);
            codeGenerationProvider.Path.Returns(TestData.BasePath);
            codeGenerationProvider.GetGeneratorType().Returns(GetType());
            codeGenerationProvider.CreateModelAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(Result.Continue<object?>()));
            codeGenerationProvider.CreateAdditionalParametersAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(Result.Continue<object?>()));
            codeGenerationSettings.DryRun.Returns(true);
            codeGenerationSettings.DefaultFilename.Returns("Filename.txt");
            templateEngine.RenderAsync(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            templateProvider.StartSessionAsync(Arg.Any<CancellationToken>()).Returns(Result.Continue());
            var sut = CreateSut();

            // Act
            await sut.GenerateAsync(codeGenerationProvider, generationEnvironment, codeGenerationSettings);

            // Assert
            await generationEnvironment.DidNotReceive().SaveContentsAsync(codeGenerationProvider, TestData.BasePath, "Filename.txt", CancellationToken.None);
        }

        [Fact]
        public async Task Initializes_TemplateProviderPlugin_When_Possible()
        {
            // Arrange
            var counter = 0;
            var provider = new MyPluginCodeGenerationProvider(_ => Task.Run(() => counter++));
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var templateEngine = Fixture.Freeze<ITemplateEngine>();
            var templateProvider = Fixture.Freeze<ITemplateProvider>();
            codeGenerationSettings.DryRun.Returns(false);
            codeGenerationSettings.BasePath.Returns(TestData.BasePath);
            codeGenerationSettings.DefaultFilename.Returns("Filename.txt");
            templateEngine.RenderAsync(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            generationEnvironment.SaveContentsAsync(Arg.Any<ICodeGenerationProvider>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            templateProvider.StartSessionAsync(Arg.Any<CancellationToken>()).Returns(Result.Continue());
            var sut = CreateSut();

            // Act
            var result = await sut.GenerateAsync(provider, generationEnvironment, codeGenerationSettings);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            counter.ShouldBe(1);
        }

        [Fact]
        public async Task Returns_Non_Successful_Result_From_StartSession_When_Applicable()
        {
            // Arrange
            var counter = 0;
            var provider = new MyPluginCodeGenerationProvider(_ => Task.Run(() => counter++));
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var templateEngine = Fixture.Freeze<ITemplateEngine>();
            var templateProvider = Fixture.Freeze<ITemplateProvider>();
            codeGenerationSettings.DryRun.Returns(true);
            templateEngine.RenderAsync(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            templateProvider.StartSessionAsync(Arg.Any<CancellationToken>()).Returns(Result.Error());
            var sut = CreateSut();

            // Act
            var result = await sut.GenerateAsync(provider, generationEnvironment, codeGenerationSettings);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            counter.ShouldBe(0); // start session fails, so the callback of the mock is not reached
        }

        [Fact]
        public async Task Returns_Non_Successful_Result_From_Initialization_When_Applicable()
        {
            // Arrange
            var counter = 0;
            var provider = new MyPluginCodeGenerationProvider(_ => Task.Run(() => counter++), ResultStatus.Error);
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var templateEngine = Fixture.Freeze<ITemplateEngine>();
            var templateProvider = Fixture.Freeze<ITemplateProvider>();
            codeGenerationSettings.DryRun.Returns(false);
            codeGenerationSettings.BasePath.Returns(TestData.BasePath);
            codeGenerationSettings.DefaultFilename.Returns("Filename.txt");
            templateEngine.RenderAsync(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            templateProvider.StartSessionAsync(Arg.Any<CancellationToken>()).Returns(Result.Continue());
            var sut = CreateSut();

            // Act
            var result = await sut.GenerateAsync(provider, generationEnvironment, codeGenerationSettings);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            counter.ShouldBe(1);
        }

        [Fact]
        public async Task Returns_Non_Successful_Result_From_Model_Creation()
        {
            // Arrange
            var codeGenerationProvider = Fixture.Freeze<ICodeGenerationProvider>();
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var templateProvider = Fixture.Freeze<ITemplateProvider>();
            codeGenerationProvider.Encoding.Returns(Encoding.Latin1);
            codeGenerationProvider.Path.Returns(TestData.BasePath);
            codeGenerationProvider.GetGeneratorType().Returns(GetType());
            codeGenerationProvider.CreateModelAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(Result.Error<object?>("Kaboom")));
            codeGenerationProvider.CreateAdditionalParametersAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(Result.Continue<object?>()));
            templateProvider.StartSessionAsync(Arg.Any<CancellationToken>()).Returns(Result.Continue());
            var sut = CreateSut();

            // Act
            var result = await sut.GenerateAsync(codeGenerationProvider, generationEnvironment, codeGenerationSettings);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_Non_Successful_Result_From_Additional_Parameter_Creation()
        {
            // Arrange
            var codeGenerationProvider = Fixture.Freeze<ICodeGenerationProvider>();
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var templateProvider = Fixture.Freeze<ITemplateProvider>();
            codeGenerationProvider.Encoding.Returns(Encoding.Latin1);
            codeGenerationProvider.Path.Returns(TestData.BasePath);
            codeGenerationProvider.GetGeneratorType().Returns(GetType());
            codeGenerationProvider.CreateModelAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(Result.Continue<object?>()));
            codeGenerationProvider.CreateAdditionalParametersAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(Result.Error<object?>("Kaboom")));
            templateProvider.StartSessionAsync(Arg.Any<CancellationToken>()).Returns(Result.Continue());
            var sut = CreateSut();

            // Act
            var result = await sut.GenerateAsync(codeGenerationProvider, generationEnvironment, codeGenerationSettings);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Starts_New_Session_On_TemplateProvider()
        {
            // Arrange
            var codeGenerationProvider = Fixture.Freeze<ICodeGenerationProvider>();
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var templateProvider = Fixture.Freeze<ITemplateProvider>();
            var templateEngine = Fixture.Freeze<ITemplateEngine>();
            codeGenerationProvider.Encoding.Returns(Encoding.Latin1);
            codeGenerationProvider.Path.Returns(TestData.BasePath);
            codeGenerationProvider.GetGeneratorType().Returns(GetType());
            codeGenerationProvider.CreateModelAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(Result.Continue<object?>()));
            codeGenerationProvider.CreateAdditionalParametersAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(Result.Continue<object?>()));
            codeGenerationSettings.DryRun.Returns(true);
            codeGenerationSettings.DefaultFilename.Returns("Filename.txt");
            templateEngine.RenderAsync(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            templateProvider.StartSessionAsync(Arg.Any<CancellationToken>()).Returns(Result.Continue());
            var sut = CreateSut();

            // Act
            await sut.GenerateAsync(codeGenerationProvider, generationEnvironment, codeGenerationSettings);

            // Assert
            await templateProvider.Received().StartSessionAsync(CancellationToken.None);
        }

        [Fact]
        public async Task Returns_Success_When_Rendering_Is_Successful()
        {
            // Arrange
            var counter = 0;
            var provider = new MyPluginCodeGenerationProvider(_ => Task.Run(() => counter++));
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var templateEngine = Fixture.Freeze<ITemplateEngine>();
            var templateProvider = Fixture.Freeze<ITemplateProvider>();
            codeGenerationSettings.DryRun.Returns(true);
            templateEngine.RenderAsync(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            templateProvider.StartSessionAsync(Arg.Any<CancellationToken>()).Returns(Result.Continue());
            var sut = CreateSut();

            // Act
            var result = await sut.GenerateAsync(provider, generationEnvironment, codeGenerationSettings);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public async Task Returns_Rendering_Result_When_Rendering_Is_Not_Successful()
        {
            // Arrange
            var counter = 0;
            var provider = new MyPluginCodeGenerationProvider(_ => Task.Run(() => counter++));
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var templateEngine = Fixture.Freeze<ITemplateEngine>();
            var templateProvider = Fixture.Freeze<ITemplateProvider>();
            codeGenerationSettings.DryRun.Returns(true);
            templateEngine.RenderAsync(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>()).Returns(Result.Error());
            templateProvider.StartSessionAsync(Arg.Any<CancellationToken>()).Returns(Result.Continue());
            var sut = CreateSut();

            // Act
            var result = await sut.GenerateAsync(provider, generationEnvironment, codeGenerationSettings);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
        }

        private sealed class MyPluginCodeGenerationProvider : ICodeGenerationProvider, ITemplateComponentRegistryPlugin
        {
            public string Path => string.Empty;
            public bool RecurseOnDeleteGeneratedFiles => false;
            public string LastGeneratedFilesFilename => string.Empty;
            public Encoding Encoding => Encoding.UTF8;

            public Task<Result<object?>> CreateAdditionalParametersAsync(CancellationToken cancellationToken) => Task.FromResult(Result.Success<object?>(default));
            public Type GetGeneratorType() => typeof(object);
            public Task<Result<object?>> CreateModelAsync(CancellationToken cancellationToken) => Task.FromResult(Result.Success<object?>(default));

            private readonly Func<ITemplateComponentRegistry, Task> _action;
            private readonly ResultStatus _status;

            public MyPluginCodeGenerationProvider(Func<ITemplateComponentRegistry, Task> action, ResultStatus status = ResultStatus.Ok)
            {
                _action = action;
                _status = status;
            }

            public async Task<Result> InitializeAsync(ITemplateComponentRegistry registry, CancellationToken cancellationToken)
            {
                await _action(registry).ConfigureAwait(false);

                return _status == ResultStatus.Ok
                    ? Result.Success()
                    : Result.Error();
            }
        }
    }
}
