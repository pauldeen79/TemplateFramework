namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests;

public class ExpressionStringTemplateTests
{
    protected const string Template = "Hello {Name}!";
    protected Mock<IExpressionStringParser> ExpressionStringParserMock { get; } = new();
    protected Mock<IFormattableStringParser> FormattableStringParserMock { get; } = new();
    protected ExpressionStringTemplateIdentifier Identifier { get; } = new ExpressionStringTemplateIdentifier(Template, CultureInfo.CurrentCulture);
    protected ComponentRegistrationContext ComponentRegistrationContext { get; } = new();

    protected ExpressionStringTemplate CreateSut() => new(Identifier, ExpressionStringParserMock.Object, FormattableStringParserMock.Object, ComponentRegistrationContext);

    public class Constructor : ExpressionStringTemplateTests
    {
        [Fact]
        public void Throws_On_Null_ExpressionStringTemplateIdentifier()
        {
            // Act & Assert
            this.Invoking(_ => new ExpressionStringTemplate(expressionStringTemplateIdentifier: null!, ExpressionStringParserMock.Object, FormattableStringParserMock.Object, ComponentRegistrationContext))
                .Should().Throw<ArgumentNullException>().WithParameterName("expressionStringTemplateIdentifier");
        }

        [Fact]
        public void Throws_On_Null_ExpressionStringParser()
        {
            // Act & Assert
            this.Invoking(_ => new ExpressionStringTemplate(Identifier, expressionStringParser: null!, FormattableStringParserMock.Object, ComponentRegistrationContext))
                .Should().Throw<ArgumentNullException>().WithParameterName("expressionStringParser");
        }

        [Fact]
        public void Throws_On_Null_FormattableStringParser()
        {
            // Act & Assert
            this.Invoking(_ => new ExpressionStringTemplate(Identifier, ExpressionStringParserMock.Object, formattableStringParser: null!, ComponentRegistrationContext))
                .Should().Throw<ArgumentNullException>().WithParameterName("formattableStringParser");
        }

        [Fact]
        public void Throws_On_Null_ComponentRegistrationContext()
        {
            // Act & Assert
            this.Invoking(_ => new ExpressionStringTemplate(Identifier, ExpressionStringParserMock.Object, FormattableStringParserMock.Object, componentRegistrationContext: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("componentRegistrationContext");
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
            ExpressionStringParserMock
                .Setup(x => x.Parse(It.IsAny<string>(), It.IsAny<IFormatProvider>(), It.IsAny<TemplateFrameworkStringContext>(), It.IsAny<IFormattableStringParser>()))
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
            ExpressionStringParserMock
                .Setup(x => x.Parse(It.IsAny<string>(), It.IsAny<IFormatProvider>(), It.IsAny<TemplateFrameworkStringContext>(), It.IsAny<IFormattableStringParser>()))
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
