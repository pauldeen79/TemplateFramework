﻿namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class ParameterInitializerTests
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
        public void Throws_On_Null_Context(ParameterInitializerComponent sut)
        {
            // Act & Assert
            sut.Invoking(x => x.Initialize(context: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Theory, AutoMockData]
        public void Sets_AdditionalParameters_When_Template_Implements_IParameterizedTemplate(
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
            valueConverter.Convert(Arg.Any<object?>(), Arg.Any<Type>()).Returns(x => x.Args()[0]);

            // Act
            sut.Initialize(engineContext);

            // Assert
            template.AdditionalParameter.Should().Be(additionalParameters.AdditionalParameter);
        }

        [Theory, AutoMockData]
        public void Converts_AdditionalParameter_When_Converter_Is_Available(
            [Frozen] ITemplateEngine templateEngine,
            [Frozen] ITemplateProvider templateProvider,
            [Frozen] IValueConverter valueConverter,
            ParameterInitializerComponent sut)
        {
            // Arrange
            var additionalParameters = new { AdditionalParameter = "?" };
            var template = new PlainTemplateWithAdditionalParameters();
            object? convertedValue = "Hello world!";
            valueConverter.Convert(Arg.Any<object?>(), Arg.Any<Type>())
                              .Returns(convertedValue);
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder(), DefaultFilename, additionalParameters);
            var engineContext = new TemplateEngineContext(request, templateEngine, templateProvider, template);

            // Act
            sut.Initialize(engineContext);

            // Assert
            template.AdditionalParameter.Should().BeEquivalentTo(convertedValue.ToString());
        }

        [Theory, AutoMockData]
        public void Does_Not_Throw_On_Non_Existing_AdditionalParameters(
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
            sut.Invoking(x => x.Initialize(engineContext))
               .Should().NotThrow();
        }

        [Theory, AutoMockData]
        public void Skips_Model_AdditionalParameter(
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
            valueConverter.Convert(Arg.Any<object?>(), Arg.Any<Type>()).Returns(x => x.Args()[0]);

            // Act
            sut.Initialize(engineContext);

            // Assert
            template.Model.Should().BeNull();
            template.AdditionalParameter.Should().Be(additionalParameters.AdditionalParameter);
        }

        [Theory, AutoMockData]
        public void Can_Inject_ViewModel_On_Template_Using_AdditionalParameters(
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
            valueConverter.Convert(Arg.Any<object?>(), Arg.Any<Type>()).Returns(x => x.Args()[0]);

            // Act
            sut.Initialize(engineContext);

            // Assert
            template.ViewModel.Should().BeSameAs(viewModel);
        }

        [Theory, AutoMockData]
        public void Sets_AdditionalParameters_When_Template_Has_Public_Readable_And_Writable_Properties(
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
            valueConverter.Convert(Arg.Any<object?>(), Arg.Any<Type>()).Returns(x => x.Args()[0]);
            templateEngine.GetParameters(Arg.Any<object>()).Returns(new[] { new TemplateParameter(nameof(TestData.PocoParameterizedTemplate.Parameter), typeof(string)) });

            // Act
            sut.Initialize(engineContext);

            // Assert
            template.Parameter.Should().Be(additionalParameters.Parameter);
        }

        [Theory, AutoMockData]
        public void Skips_AdditionalParameters_When_Template_Does_Not_Implement_IParameterizedTemplate_And_Property_Is_Missing(
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
            valueConverter.Convert(Arg.Any<object?>(), Arg.Any<Type>()).Returns(x => x.Args()[0]);
            templateEngine.GetParameters(Arg.Any<object>()).Returns(new[] { new TemplateParameter(nameof(TestData.PocoParameterizedTemplate.Parameter), typeof(string)) });

            // Act & Assert
            sut.Invoking(x => x.Initialize(engineContext)).Should().NotThrow();
        }
    }
}
