namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests;

public class TemplateFrameworkContextPlaceholderProcessorTests : TestBase<TemplateFrameworkContextPlaceholderProcessor>
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(ComponentRegistrationContext).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }

    public class Evaluate : TemplateFrameworkContextPlaceholderProcessorTests
    {
        private ComponentRegistrationContext ComponentRegistrationContext { get; } = new([]);
        public IFormattableStringParser FormattableStringParser { get; }

        public Evaluate()
        {
            FormattableStringParser = Fixture.Freeze<IFormattableStringParser>();
        }

        [Fact]
        public void Returns_Continue_When_Context_Is_Not_TemplateFrameworkFormattableStringContext()
        {
            // Arrange
            var context = "some context that's not of type TemplateFrameworkFormattableStringContext";
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate("some template", new PlaceholderSettingsBuilder(), context, FormattableStringParser);

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Success_With_Parameter_Value_When_Context_Is_TemplateFrameworkFormattableStringContext_And_Parameter_Is_Known_And_Not_Null()
        {
            // Arrange
            var parametersDictionary = new Dictionary<string, object?>
            {
                { "Name", "Value" }
            };
            var context = new TemplateFrameworkStringContext(parametersDictionary, ComponentRegistrationContext, false);
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate("Name", new PlaceholderSettingsBuilder(), context, FormattableStringParser);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value!.ToString(CultureInfo.InvariantCulture).Should().Be("Value");
        }

        [Fact]
        public void Returns_Success_With_StringEmpty_When_Context_Is_TemplateFrameworkFormattableStringContext_And_Parameter_Is_Known_But_Null()
        {
            // Arrange
            var parametersDictionary = new Dictionary<string, object?>
            {
                { "Name", null }
            };
            var context = new TemplateFrameworkStringContext(parametersDictionary, ComponentRegistrationContext, false);
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate("Name", new PlaceholderSettingsBuilder(), context, FormattableStringParser);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value!.ToString(CultureInfo.InvariantCulture).Should().BeEmpty();
        }

        [Fact]
        public void Returns_Continue_When_Context_Is_TemplateFrameworkFormattableStringContext_And_Parameter_Is_Unknown()
        {
            // Arrange
            var parametersDictionary = new Dictionary<string, object?>();
            var context = new TemplateFrameworkStringContext(parametersDictionary, ComponentRegistrationContext, false);
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate("Name", new PlaceholderSettingsBuilder(), context, FormattableStringParser);

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }

        [Fact]
        public void Adds_Parameter_Name_To_List()
        {
            // Arrange
            var parametersDictionary = new Dictionary<string, object?>();
            var context = new TemplateFrameworkStringContext(parametersDictionary, ComponentRegistrationContext, false);
            var sut = CreateSut();

            // Act
            _ = sut.Evaluate("Name", new PlaceholderSettingsBuilder(), context, FormattableStringParser);

            // Assert
            context.ParameterNamesList.Should().BeEquivalentTo("Name");
        }

        [Fact]
        public void Does_Not_Add_Parameter_Name_To_List_When_Name_Starts_With_Double_Underscore()
        {
            // Arrange
            var parametersDictionary = new Dictionary<string, object?>();
            var context = new TemplateFrameworkStringContext(parametersDictionary, ComponentRegistrationContext, false);
            var sut = CreateSut();

            // Act
            _ = sut.Evaluate("__Name", new PlaceholderSettingsBuilder(), context, FormattableStringParser);

            // Assert
            context.ParameterNamesList.Should().BeEmpty();
        }
    }
}
