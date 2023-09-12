namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests;

public class TemplateFrameworkContextPlaceholderProcessorTests
{
    public class Process
    {
        private ComponentRegistrationContext ComponentRegistrationContext { get; } = new();

        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(ComponentRegistrationContext).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }

        [Theory, AutoMockData]
        public void Returns_Continue_When_Context_Is_Not_TemplateFrameworkFormattableStringContext(
            [Frozen] IFormattableStringParser formattableStringParser,
            TemplateFrameworkContextPlaceholderProcessor sut)
        {
            // Arrange
            var context = "some context that's not of type TemplateFrameworkFormattableStringContext";

            // Act
            var result = sut.Process("some template", CultureInfo.CurrentCulture, context, formattableStringParser);

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }

        [Theory, AutoMockData]
        public void Returns_Success_With_Parameter_Value_When_Context_Is_TemplateFrameworkFormattableStringContext_And_Parameter_Is_Known_And_Not_Null(
            [Frozen] IFormattableStringParser formattableStringParser,
            TemplateFrameworkContextPlaceholderProcessor sut)
        {
            // Arrange
            var parametersDictionary = new Dictionary<string, object?>
            {
                { "Name", "Value" }
            };
            var context = new TemplateFrameworkStringContext(parametersDictionary, ComponentRegistrationContext, false);

            // Act
            var result = sut.Process("Name", CultureInfo.CurrentCulture, context, formattableStringParser);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be("Value");
        }

        [Theory, AutoMockData]
        public void Returns_Success_With_StringEmpty_When_Context_Is_TemplateFrameworkFormattableStringContext_And_Parameter_Is_Known_But_Null(
            [Frozen] IFormattableStringParser formattableStringParser,
            TemplateFrameworkContextPlaceholderProcessor sut)
        {
            // Arrange
            var parametersDictionary = new Dictionary<string, object?>
            {
                { "Name", null }
            };
            var context = new TemplateFrameworkStringContext(parametersDictionary, ComponentRegistrationContext, false);

            // Act
            var result = sut.Process("Name", CultureInfo.CurrentCulture, context, formattableStringParser);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEmpty();
        }

        [Theory, AutoMockData]
        public void Returns_Continue_When_Context_Is_TemplateFrameworkFormattableStringContext_And_Parameter_Is_Unknown(
            [Frozen] IFormattableStringParser formattableStringParser,
            TemplateFrameworkContextPlaceholderProcessor sut)
        {
            // Arrange
            var parametersDictionary = new Dictionary<string, object?>();
            var context = new TemplateFrameworkStringContext(parametersDictionary, ComponentRegistrationContext, false);

            // Act
            var result = sut.Process("Name", CultureInfo.CurrentCulture, context, formattableStringParser);

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }

        [Theory, AutoMockData]
        public void Adds_Parameter_Name_To_List(
            [Frozen] IFormattableStringParser formattableStringParser,
            TemplateFrameworkContextPlaceholderProcessor sut)
        {
            // Arrange
            var parametersDictionary = new Dictionary<string, object?>();
            var context = new TemplateFrameworkStringContext(parametersDictionary, ComponentRegistrationContext, false);

            // Act
            _ = sut.Process("Name", CultureInfo.CurrentCulture, context, formattableStringParser);

            // Assert
            context.ParameterNamesList.Should().BeEquivalentTo("Name");
        }

        [Theory, AutoMockData]
        public void Does_Not_Add_Parameter_Name_To_List_When_Name_Starts_With_Double_Underscore(
            [Frozen] IFormattableStringParser formattableStringParser,
            TemplateFrameworkContextPlaceholderProcessor sut)
        {
            // Arrange
            var parametersDictionary = new Dictionary<string, object?>();
            var context = new TemplateFrameworkStringContext(parametersDictionary, ComponentRegistrationContext, false);

            // Act
            _ = sut.Process("__Name", CultureInfo.CurrentCulture, context, formattableStringParser);

            // Assert
            context.ParameterNamesList.Should().BeEmpty();
        }
    }
}
