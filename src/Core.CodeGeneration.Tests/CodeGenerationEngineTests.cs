namespace TemplateFramework.Core.CodeGeneration.Tests;

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

    public class Generate : CodeGenerationEngineTests
    {
        [Fact]
        public async Task Throws_On_Null_CodeGenerationProvider()
        {
            // Arrange
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var sut = CreateSut();

            // Act
            await sut.Awaiting(x => x.Generate(codeGenerationProvider: null!, generationEnvironment, codeGenerationSettings))
                     .Should().ThrowAsync<ArgumentNullException>().WithParameterName("codeGenerationProvider");
        }

        [Fact]
        public async Task Throws_On_Null_GenerationEnvironment()
        {
            // Arrange
            var codeGenerationProvider = Fixture.Freeze<ICodeGenerationProvider>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var sut = CreateSut();

            // Act
            await sut.Awaiting(x => x.Generate(codeGenerationProvider, generationEnvironment: null!, codeGenerationSettings))
                     .Should().ThrowAsync<ArgumentNullException>().WithParameterName("generationEnvironment");
        }

        [Fact]
        public async Task Throws_On_Null_CodeGenerationSettings()
        {
            // Arrange
            var codeGenerationProvider = Fixture.Freeze<ICodeGenerationProvider>();
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var sut = CreateSut();

            // Act
            await sut.Awaiting(x => x.Generate(codeGenerationProvider, generationEnvironment, settings: null!))
                     .Should().ThrowAsync<ArgumentNullException>().WithParameterName("settings");
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
            codeGenerationProvider.CreateModel().Returns(Task.FromResult(Result.Continue<object?>()));
            codeGenerationProvider.CreateAdditionalParameters().Returns(Task.FromResult(Result.Continue<object?>()));
            codeGenerationSettings.DryRun.Returns(false);
            codeGenerationSettings.BasePath.Returns(TestData.BasePath);
            codeGenerationSettings.DefaultFilename.Returns("Filename.txt");
            templateEngine.Render(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            templateProvider.StartSession(Arg.Any<CancellationToken>()).Returns(Result.Continue());
            var sut = CreateSut();

            // Act
            await sut.Generate(codeGenerationProvider, generationEnvironment, codeGenerationSettings);

            // Assert
            await generationEnvironment.Received().SaveContents(codeGenerationProvider, TestData.BasePath, "Filename.txt", CancellationToken.None);
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
            codeGenerationProvider.CreateModel().Returns(Task.FromResult(Result.Continue<object?>()));
            codeGenerationProvider.CreateAdditionalParameters().Returns(Task.FromResult(Result.Continue<object?>()));
            codeGenerationSettings.DryRun.Returns(true);
            codeGenerationSettings.DefaultFilename.Returns("Filename.txt");
            templateEngine.Render(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            templateProvider.StartSession(Arg.Any<CancellationToken>()).Returns(Result.Continue());
            var sut = CreateSut();

            // Act
            await sut.Generate(codeGenerationProvider, generationEnvironment, codeGenerationSettings);

            // Assert
            await generationEnvironment.DidNotReceive().SaveContents(codeGenerationProvider, TestData.BasePath, "Filename.txt", CancellationToken.None);
        }

        [Fact]
        public async Task Initializes_TemplateProviderPlugin_When_Possible()
        {
            // Arrange
            var counter = 0;
            var provider = new MyPluginCodeGenerationProvider(_ => counter++);
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var templateEngine = Fixture.Freeze<ITemplateEngine>();
            var templateProvider = Fixture.Freeze<ITemplateProvider>();
            codeGenerationSettings.DryRun.Returns(false);
            codeGenerationSettings.BasePath.Returns(TestData.BasePath);
            codeGenerationSettings.DefaultFilename.Returns("Filename.txt");
            templateEngine.Render(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            generationEnvironment.SaveContents(Arg.Any<ICodeGenerationProvider>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            templateProvider.StartSession(Arg.Any<CancellationToken>()).Returns(Result.Continue());
            var sut = CreateSut();

            // Act
            var result = await sut.Generate(provider, generationEnvironment, codeGenerationSettings);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            counter.Should().Be(1);
        }

        [Fact]
        public async Task Returns_Non_Successful_Result_From_StartSession_When_Applicable()
        {
            // Arrange
            var counter = 0;
            var provider = new MyPluginCodeGenerationProvider(_ => counter++);
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var templateEngine = Fixture.Freeze<ITemplateEngine>();
            var templateProvider = Fixture.Freeze<ITemplateProvider>();
            codeGenerationSettings.DryRun.Returns(true);
            templateEngine.Render(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            templateProvider.StartSession(Arg.Any<CancellationToken>()).Returns(Result.Error());
            var sut = CreateSut();

            // Act
            var result = await sut.Generate(provider, generationEnvironment, codeGenerationSettings);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            counter.Should().Be(1); // start session fails, so the callback of the mock is not reached
        }

        [Fact]
        public async Task Returns_Non_Successful_Result_From_Initialization_When_Applicable()
        {
            // Arrange
            var counter = 0;
            var provider = new MyPluginCodeGenerationProvider(_ => counter++, ResultStatus.Error);
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var templateEngine = Fixture.Freeze<ITemplateEngine>();
            var templateProvider = Fixture.Freeze<ITemplateProvider>();
            codeGenerationSettings.DryRun.Returns(false);
            codeGenerationSettings.BasePath.Returns(TestData.BasePath);
            codeGenerationSettings.DefaultFilename.Returns("Filename.txt");
            templateEngine.Render(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            templateProvider.StartSession(Arg.Any<CancellationToken>()).Returns(Result.Continue());
            var sut = CreateSut();

            // Act
            var result = await sut.Generate(provider, generationEnvironment, codeGenerationSettings);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            counter.Should().Be(1);
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
            codeGenerationProvider.CreateModel().Returns(Task.FromResult(Result.Error<object?>("Kaboom")));
            codeGenerationProvider.CreateAdditionalParameters().Returns(Task.FromResult(Result.Continue<object?>()));
            templateProvider.StartSession(Arg.Any<CancellationToken>()).Returns(Result.Continue());
            var sut = CreateSut();

            // Act
            var result = await sut.Generate(codeGenerationProvider, generationEnvironment, codeGenerationSettings);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
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
            codeGenerationProvider.CreateModel().Returns(Task.FromResult(Result.Continue<object?>()));
            codeGenerationProvider.CreateAdditionalParameters().Returns(Task.FromResult(Result.Error<object?>("Kaboom")));
            templateProvider.StartSession(Arg.Any<CancellationToken>()).Returns(Result.Continue());
            var sut = CreateSut();

            // Act
            var result = await sut.Generate(codeGenerationProvider, generationEnvironment, codeGenerationSettings);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
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
            codeGenerationProvider.CreateModel().Returns(Task.FromResult(Result.Continue<object?>()));
            codeGenerationProvider.CreateAdditionalParameters().Returns(Task.FromResult(Result.Continue<object?>()));
            codeGenerationSettings.DryRun.Returns(true);
            codeGenerationSettings.DefaultFilename.Returns("Filename.txt");
            templateEngine.Render(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            templateProvider.StartSession(Arg.Any<CancellationToken>()).Returns(Result.Continue());
            var sut = CreateSut();

            // Act
            await sut.Generate(codeGenerationProvider, generationEnvironment, codeGenerationSettings);

            // Assert
            await templateProvider.Received().StartSession(CancellationToken.None);
        }

        [Fact]
        public async Task Returns_Success_When_Rendering_Is_Successful()
        {
            // Arrange
            var counter = 0;
            var provider = new MyPluginCodeGenerationProvider(_ => counter++);
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var templateEngine = Fixture.Freeze<ITemplateEngine>();
            var templateProvider = Fixture.Freeze<ITemplateProvider>();
            codeGenerationSettings.DryRun.Returns(true);
            templateEngine.Render(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            templateProvider.StartSession(Arg.Any<CancellationToken>()).Returns(Result.Continue());
            var sut = CreateSut();

            // Act
            var result = await sut.Generate(provider, generationEnvironment, codeGenerationSettings);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public async Task Returns_Rendering_Result_When_Rendering_Is_Not_Successful()
        {
            // Arrange
            var counter = 0;
            var provider = new MyPluginCodeGenerationProvider(_ => counter++);
            var generationEnvironment = Fixture.Freeze<IGenerationEnvironment>();
            var codeGenerationSettings = Fixture.Freeze<ICodeGenerationSettings>();
            var templateEngine = Fixture.Freeze<ITemplateEngine>();
            var templateProvider = Fixture.Freeze<ITemplateProvider>();
            codeGenerationSettings.DryRun.Returns(true);
            templateEngine.Render(Arg.Any<IRenderTemplateRequest>(), Arg.Any<CancellationToken>()).Returns(Result.Error());
            templateProvider.StartSession(Arg.Any<CancellationToken>()).Returns(Result.Continue());
            var sut = CreateSut();

            // Act
            var result = await sut.Generate(provider, generationEnvironment, codeGenerationSettings);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
        }

        private sealed class MyPluginCodeGenerationProvider : ICodeGenerationProvider, ITemplateComponentRegistryPlugin
        {
            public string Path => string.Empty;
            public bool RecurseOnDeleteGeneratedFiles => false;
            public string LastGeneratedFilesFilename => string.Empty;
            public Encoding Encoding => Encoding.UTF8;

            public Task<Result<object?>> CreateAdditionalParameters() => Task.FromResult(Result.Success<object?>(default));
            public Type GetGeneratorType() => typeof(object);
            public Task<Result<object?>> CreateModel() => Task.FromResult(Result.Success<object?>(default));

            private readonly Action<ITemplateComponentRegistry> _action;
            private readonly ResultStatus _status;

            public MyPluginCodeGenerationProvider(Action<ITemplateComponentRegistry> action, ResultStatus status = ResultStatus.Ok)
            {
                _action = action;
                _status = status;
            }

            public Task<Result> Initialize(ITemplateComponentRegistry registry, CancellationToken cancellationToken) { _action(registry); return Task.FromResult(_status == ResultStatus.Ok ? Result.Success() : Result.Error()); }
        }
    }
}
