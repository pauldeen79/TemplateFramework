namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider.Tests;

public class FormattableStringTemplateTests
{
    protected const string Template = "Hello {Name}!";
    protected Mock<IFormattableStringParser> FormattableStringParserMock { get; } = new();
    protected FormattableStringTemplateIdentifier Request { get; } = new FormattableStringTemplateIdentifier(Template, CultureInfo.CurrentCulture);

    protected FormattableStringTemplate CreateSut() => new(Request, FormattableStringParserMock.Object);

    public class Constructor : FormattableStringTemplateTests
    {
        [Fact]
        public void Throws_On_Null_CreateFormattableStringTemplateRequest()
        {
            // Act & Assert
            this.Invoking(_ => new FormattableStringTemplate(createFormattableStringTemplateRequest: null!, FormattableStringParserMock.Object))
                .Should().Throw<ArgumentNullException>().WithParameterName("createFormattableStringTemplateRequest");
        }

        [Fact]
        public void Throws_On_Null_FormattableStringParser()
        {
            // Act & Assert
            this.Invoking(_ => new FormattableStringTemplate(Request, formattableStringParser: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("formattableStringParser");
        }
    }

    public class GetParameters : FormattableStringTemplateTests
    {
        [Fact]
        public void Returns_Parameters_From_Template()
        {
            // Arrange
            var sut = CreateSut();
            FormattableStringParserMock
                .Setup(x => x.Parse(It.IsAny<string>(), It.IsAny<IFormatProvider>(), It.IsAny<TemplateFrameworkFormattableStringContext>()))
                .Returns<string, IFormatProvider, object?>((input, formatProvider, context) =>
                {
                    // Note that in this unit test, we have to mock the behavior of FormattableStringParser :)
                    // There is also an Integration test to prove it works in real life ;-)
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    ((TemplateFrameworkFormattableStringContext)context).ParameterNamesList.Add("Name");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    return Result<string>.Success(string.Empty);
                });

            // Act
            var result = sut.GetParameters();

            // Assert
            result.Select(x => x.Name).Should().BeEquivalentTo("Name");
        }
    }

    public class Render_MultipleContentBuilder : FormattableStringTemplateTests
    {
        [Fact]
        public void Throws_On_Null_Builder()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Render(builder: default(IMultipleContentBuilder)!))
               .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_NonSuccesful_Result_From_FormattableStringParser()
        {
            // Arrange
            FormattableStringParserMock
                .Setup(x => x.Parse(It.IsAny<string>(), It.IsAny<IFormatProvider>(), It.IsAny<TemplateFrameworkFormattableStringContext>()))
                .Returns(Result<string>.Error("Kaboom!"));
            var sut = CreateSut();
            var builder = new StringBuilder();

            // Act & Assert
            sut.Invoking(x => x.Render(builder))
               .Should().Throw<InvalidOperationException>().WithMessage("Result: Error, ErrorMessage: Kaboom!");
        }

        [Fact]
        public void Appends_Result_From_FormattableStringParser_To_Builder_On_Succesful_Result()
        {
            // Arrange
            FormattableStringParserMock
                .Setup(x => x.Parse(It.IsAny<string>(), It.IsAny<IFormatProvider>(), It.IsAny<TemplateFrameworkFormattableStringContext>()))
                .Returns(Result<string>.Success("Parse result"));
            var sut = CreateSut();
            var builder = new StringBuilder();

            // Act
            sut.Render(builder);

            // Assert
            builder.ToString().Should().Be("Parse result");
        }
    }

    public class SetParameter : FormattableStringTemplateTests
    {
        [Fact]
        public void Adds_Parameter_To_Context()
        {
            // Arrange
            var sut = CreateSut();
            IDictionary<string, object?>? dictionary = null;
            FormattableStringParserMock
                .Setup(x => x.Parse(It.IsAny<string>(), It.IsAny<IFormatProvider>(), It.IsAny<TemplateFrameworkFormattableStringContext>()))
                .Returns<string, IFormatProvider, object?>((input, formatProvider, context) =>
                {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    dictionary = ((TemplateFrameworkFormattableStringContext)context).ParametersDictionary;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                    return Result<string>.Success(string.Empty);
                });

            // Act
            sut.SetParameter("Name", "Value");

            // Assert
            sut.Render(new StringBuilder());
            dictionary.Should().NotBeNull();
            dictionary!.First().Key.Should().Be("Name");
            dictionary!.First().Value.Should().Be("Value");
        }
    }
}
