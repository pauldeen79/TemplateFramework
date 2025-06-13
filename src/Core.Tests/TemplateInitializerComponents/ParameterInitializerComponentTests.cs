namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class ParameterInitializerComponentTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(ParameterInitializerComponent).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }

    public class Initialize
    {
        private const string DefaultFilename = "DefaultFilename.txt";

        [Theory, AutoMockData]
        public async Task Throws_On_Null_Context(ParameterInitializerComponent sut)
        {
            // Act & Assert
            Task t = sut.InitializeAsync(context: null!, CancellationToken.None);
            (await t.ShouldThrowAsync<ArgumentNullException>()).ParamName.ShouldBe("context");
        }

        [Theory, AutoMockData]
        public async Task Sets_AdditionalParameters_When_Template_Implements_IParameterizedTemplate(
            [Frozen] ITemplateEngine templateEngine,
            [Frozen] ITemplateProvider templateProvider,
            [Frozen] IValueConverter valueConverter,
            ParameterInitializerComponent sut)
        {
            // Arrange
            var additionalParameters = new { AdditionalParameter = "Hello world!" };
            var template = new PlainTemplateWithAdditionalParameters();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, additionalParameters);
            var engineContext = new TemplateEngineContext(request, templateEngine, templateProvider, template);
            valueConverter.Convert(Arg.Any<object?>(), Arg.Any<Type>(), Arg.Any<ITemplateEngineContext>()).Returns(x => x.Args()[0]);

            // Act
            await sut.InitializeAsync(engineContext, CancellationToken.None);

            // Assert
            template.AdditionalParameter.ShouldBe(additionalParameters.AdditionalParameter);
        }

        [Theory, AutoMockData]
        public async Task Returns_Result_When_Template_Implements_IParameterizedTemplate_And_Result_Is_Not_Successful_On_SetParameter(
            [Frozen] ITemplateEngine templateEngine,
            [Frozen] ITemplateProvider templateProvider,
            [Frozen] IValueConverter valueConverter,
            ParameterInitializerComponent sut)
        {
            // Arrange
            var additionalParameters = new { AdditionalParameter = "Hello world!" };
            var template = new PlainTemplateWithAdditionalParameters
            {
                SetParameterReturnValue = Result.Error("Kaboom!")
            };
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, additionalParameters);
            var engineContext = new TemplateEngineContext(request, templateEngine, templateProvider, template);
            valueConverter.Convert(Arg.Any<object?>(), Arg.Any<Type>(), Arg.Any<ITemplateEngineContext>()).Returns(x => x.Args()[0]);

            // Act
            var result = await sut.InitializeAsync(engineContext, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom!");
        }

        [Theory, AutoMockData]
        public async Task Returns_Result_When_Template_Implements_IParameterizedTemplate_And_Result_Is_Not_Successful_On_GetParameters(
            [Frozen] ITemplateEngine templateEngine,
            [Frozen] ITemplateProvider templateProvider,
            [Frozen] IValueConverter valueConverter,
            ParameterInitializerComponent sut)
        {
            // Arrange
            var additionalParameters = new { AdditionalParameter = "Hello world!" };
            var template = new PlainTemplateWithAdditionalParameters
            {
                GetParametersReturnValue = Result.Error<ITemplateParameter[]>("Kaboom!")
            };
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, additionalParameters);
            var engineContext = new TemplateEngineContext(request, templateEngine, templateProvider, template);
            valueConverter.Convert(Arg.Any<object?>(), Arg.Any<Type>(), Arg.Any<ITemplateEngineContext>()).Returns(x => x.Args()[0]);

            // Act
            var result = await sut.InitializeAsync(engineContext, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom!");
        }

        [Theory, AutoMockData]
        public async Task Converts_AdditionalParameter_When_Converter_Is_Available(
            [Frozen] ITemplateEngine templateEngine,
            [Frozen] ITemplateProvider templateProvider,
            [Frozen] IValueConverter valueConverter,
            ParameterInitializerComponent sut)
        {
            // Arrange
            var additionalParameters = new { AdditionalParameter = "?" };
            var template = new PlainTemplateWithAdditionalParameters();
            object? convertedValue = "Hello world!";
            valueConverter.Convert(Arg.Any<object?>(), Arg.Any<Type>(), Arg.Any<ITemplateEngineContext>())
                          .Returns(convertedValue);
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, additionalParameters);
            var engineContext = new TemplateEngineContext(request, templateEngine, templateProvider, template);

            // Act
            await sut.InitializeAsync(engineContext, CancellationToken.None);

            // Assert
            template.AdditionalParameter.ShouldBeEquivalentTo(convertedValue.ToString());
        }

        [Theory, AutoMockData]
        public async Task Does_Not_Throw_On_Non_Existing_AdditionalParameters(
            [Frozen] ITemplateEngine templateEngine,
            [Frozen] ITemplateProvider templateProvider,
            ParameterInitializerComponent sut)
        {
            // Arrange
            var additionalParameters = new { AdditionalParameter = "Hello world!", NonExistingParameter = "Kaboom" };
            var template = new PlainTemplateWithAdditionalParameters();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, additionalParameters);
            var engineContext = new TemplateEngineContext(request, templateEngine, templateProvider, template);

            // Act & Assert
            Task t = sut.InitializeAsync(engineContext, CancellationToken.None);
            await t.ShouldNotThrowAsync();
        }

        [Theory, AutoMockData]
        public async Task Skips_Model_AdditionalParameter(
            [Frozen] ITemplateEngine templateEngine,
            [Frozen] ITemplateProvider templateProvider,
            [Frozen] IValueConverter valueConverter,
            ParameterInitializerComponent sut)
        {
            // Arrange
            var model = "Hello world!";
            var additionalParameters = new { AdditionalParameter = "Hello world!", Model = "Ignored" };
            var template = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), model, new StringBuilder(), DefaultFilename, additionalParameters);
            var engineContext = new TemplateEngineContext(request, templateEngine, templateProvider, template);
            valueConverter.Convert(Arg.Any<object?>(), Arg.Any<Type>(), Arg.Any<ITemplateEngineContext>()).Returns(x => x.Args()[0]);

            // Act
            await sut.InitializeAsync(engineContext, CancellationToken.None);

            // Assert
            template.Model.ShouldBeNull();
            template.AdditionalParameter.ShouldBe(additionalParameters.AdditionalParameter);
        }

        [Theory, AutoMockData]
        public async Task Can_Inject_ViewModel_On_Template_Using_AdditionalParameters(
            [Frozen] ITemplateEngine templateEngine,
            [Frozen] ITemplateProvider templateProvider,
            [Frozen] IValueConverter valueConverter,
            ParameterInitializerComponent sut)
        {
            // Arrange
            var template = new TestData.TemplateWithViewModel<TestData.NonConstructableViewModel>(_ => { });
            var viewModel = new TestData.NonConstructableViewModel("Some value");
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, additionalParameters: new { ViewModel = viewModel });
            var engineContext = new TemplateEngineContext(request, templateEngine, templateProvider, template);
            valueConverter.Convert(Arg.Any<object?>(), Arg.Any<Type>(), Arg.Any<ITemplateEngineContext>()).Returns(x => x.Args()[0]);

            // Act
            await sut.InitializeAsync(engineContext, CancellationToken.None);

            // Assert
            template.ViewModel.ShouldBeSameAs(viewModel);
        }

        [Theory, AutoMockData]
        public async Task Sets_AdditionalParameters_When_Template_Has_Public_Readable_And_Writable_Properties(
            [Frozen] ITemplateEngine templateEngine,
            [Frozen] ITemplateProvider templateProvider,
            [Frozen] IValueConverter valueConverter,
            ParameterInitializerComponent sut)
        {
            // Arrange
            var additionalParameters = new { Parameter = "Hello world!" };
            var template = new TestData.PocoParameterizedTemplate();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, additionalParameters);
            var engineContext = new TemplateEngineContext(request, templateEngine, templateProvider, template);
            valueConverter.Convert(Arg.Any<object?>(), Arg.Any<Type>(), Arg.Any<ITemplateEngineContext>()).Returns(x => x.Args()[0]);
            templateEngine.GetParametersAsync(Arg.Any<object>(), Arg.Any<CancellationToken>()).Returns(Result.Success<ITemplateParameter[]>([new TemplateParameter(nameof(TestData.PocoParameterizedTemplate.Parameter), typeof(string))]));

            // Act
            await sut.InitializeAsync(engineContext, CancellationToken.None);

            // Assert
            template.Parameter.ShouldBe(additionalParameters.Parameter);
        }

        [Theory, AutoMockData]
        public async Task Returns_Result_When_Template_Has_Public_Readable_And_Writable_Properties_And_GetParameters_Returns_Non_Successful_Result(
            [Frozen] ITemplateEngine templateEngine,
            [Frozen] ITemplateProvider templateProvider,
            [Frozen] IValueConverter valueConverter,
            ParameterInitializerComponent sut)
        {
            // Arrange
            var additionalParameters = new { Parameter = "Hello world!" };
            var template = new TestData.PocoParameterizedTemplate();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, additionalParameters);
            var engineContext = new TemplateEngineContext(request, templateEngine, templateProvider, template);
            valueConverter.Convert(Arg.Any<object?>(), Arg.Any<Type>(), Arg.Any<ITemplateEngineContext>()).Returns(x => x.Args()[0]);
            templateEngine.GetParametersAsync(Arg.Any<object>(), Arg.Any<CancellationToken>()).Returns(Result.Error<ITemplateParameter[]>("Kaboom!"));

            // Act
            var result = await sut.InitializeAsync(engineContext, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom!");
        }

        [Theory, AutoMockData]
        public async Task Skips_AdditionalParameters_When_Template_Does_Not_Implement_IParameterizedTemplate_And_Property_Is_Missing(
            [Frozen] ITemplateEngine templateEngine,
            [Frozen] ITemplateProvider templateProvider,
            [Frozen] IValueConverter valueConverter,
            ParameterInitializerComponent sut)
        {
            // Arrange
            var additionalParameters = new { WrongParameter = "Hello world!" };
            var template = new TestData.PocoParameterizedTemplate();
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, additionalParameters);
            var engineContext = new TemplateEngineContext(request, templateEngine, templateProvider, template);
            valueConverter.Convert(Arg.Any<object?>(), Arg.Any<Type>(), Arg.Any<ITemplateEngineContext>()).Returns(x => x.Args()[0]);
            templateEngine.GetParametersAsync(Arg.Any<object>(), Arg.Any<CancellationToken>()).Returns(Result.Success<ITemplateParameter[]>([new TemplateParameter(nameof(TestData.PocoParameterizedTemplate.Parameter), typeof(string))]));

            // Act & Assert
            Task t = sut.InitializeAsync(engineContext, CancellationToken.None);
            await t.ShouldNotThrowAsync();
        }
    }
}
