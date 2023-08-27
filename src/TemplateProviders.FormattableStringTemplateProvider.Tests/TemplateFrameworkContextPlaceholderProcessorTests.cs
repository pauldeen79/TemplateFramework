namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider.Tests;

public class TemplateFrameworkContextPlaceholderProcessorTests
{
    public class Process
    {
        [Fact]
        public void Returns_Continue_When_Context_Is_Not_TemplateFrameworkFormattableStringContext()
        {
            // Arrange
            var sut = new TemplateFrameworkContextPlaceholderProcessor();
            var context = "some context that's not of type TemplateFrameworkFormattableStringContext";

            // Act
            var result = sut.Process("some template", CultureInfo.CurrentCulture, context);

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Success_With_Parameter_Value_When_Context_Is_TemplateFrameworkFormattableStringContext_And_Parameter_Is_Known_And_Not_Null()
        {
            // Arrange
            var sut = new TemplateFrameworkContextPlaceholderProcessor();
            var parametersDictionary = new Dictionary<string, object?>
            {
                { "Name", "Value" }
            };
            var context = new TemplateFrameworkFormattableStringContext(parametersDictionary);

            // Act
            var result = sut.Process("Name", CultureInfo.CurrentCulture, context);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be("Value");
        }

        [Fact]
        public void Returns_Success_With_StringEmpty_When_Context_Is_TemplateFrameworkFormattableStringContext_And_Parameter_Is_Known_But_Null()
        {
            // Arrange
            var sut = new TemplateFrameworkContextPlaceholderProcessor();
            var parametersDictionary = new Dictionary<string, object?>
            {
                { "Name", null }
            };
            var context = new TemplateFrameworkFormattableStringContext(parametersDictionary);

            // Act
            var result = sut.Process("Name", CultureInfo.CurrentCulture, context);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEmpty();
        }

        [Fact]
        public void Returns_Success_With_Parameter_Value_When_Context_Is_TemplateFrameworkFormattableStringContext_And_Parameter_Is_Unknown()
        {
            // Arrange
            var sut = new TemplateFrameworkContextPlaceholderProcessor();
            var parametersDictionary = new Dictionary<string, object?>();
            var context = new TemplateFrameworkFormattableStringContext(parametersDictionary);

            // Act
            var result = sut.Process("Name", CultureInfo.CurrentCulture, context);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEmpty();
        }

        [Fact]
        public void Adds_Parameter_Name_To_List()
        {
            // Arrange
            var sut = new TemplateFrameworkContextPlaceholderProcessor();
            var parametersDictionary = new Dictionary<string, object?>();
            var context = new TemplateFrameworkFormattableStringContext(parametersDictionary);

            // Act
            _ = sut.Process("Name", CultureInfo.CurrentCulture, context);

            // Assert
            context.ParameterNamesList.Should().BeEquivalentTo("Name");
        }
    }
}
