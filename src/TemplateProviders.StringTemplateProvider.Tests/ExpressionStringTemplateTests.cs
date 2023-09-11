namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests;

public class ExpressionStringTemplateTests
{
    protected const string Template = "Hello {Name}!";
    protected IExpressionStringParser ExpressionStringParserMock { get; } = Substitute.For<IExpressionStringParser>();
    protected IFormattableStringParser FormattableStringParserMock { get; } = Substitute.For<IFormattableStringParser>();
    protected ExpressionStringTemplateIdentifier Identifier { get; } = new ExpressionStringTemplateIdentifier(Template, CultureInfo.CurrentCulture);
    protected ComponentRegistrationContext ComponentRegistrationContext { get; } = new();

    protected ExpressionStringTemplate CreateSut() => new(Identifier, ExpressionStringParserMock, FormattableStringParserMock, ComponentRegistrationContext);

    public class Constructor : ExpressionStringTemplateTests
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            typeof(ExpressionStringTemplate).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(
                p => !new[] { "model", "iterationNumber", "iterationCount" }.Contains(p.Name));
        }
    }

    public class Render : ExpressionStringTemplateTests
    {
        [Fact]
        public void Throws_On_Null_Builder()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Render(builder: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_NonSuccesful_Result_From_FormattableStringParser()
        {
            // Arrange
            ExpressionStringParserMock.Parse(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<TemplateFrameworkStringContext>(), Arg.Any<IFormattableStringParser>())
                .Returns(Result<object?>.Error("Kaboom!"));
            var sut = CreateSut();
            var builder = new StringBuilder();

            // Act & Assert
            sut.Invoking(x => x.Render(builder))
               .Should().Throw<InvalidOperationException>().WithMessage("Result: Error, ErrorMessage: Kaboom!");
        }

        [Fact]
        public void Appends_Result_From_ExpressionStringParser_To_Builder_On_Succesful_Result()
        {
            // Arrange
            ExpressionStringParserMock.Parse(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<TemplateFrameworkStringContext>(), Arg.Any<IFormattableStringParser>())
                .Returns(Result<object?>.Success("Parse result"));
            var sut = CreateSut();
            var builder = new StringBuilder();

            // Act
            sut.Render(builder);

            // Assert
            builder.ToString().Should().Be("Parse result");
        }
    }
}
