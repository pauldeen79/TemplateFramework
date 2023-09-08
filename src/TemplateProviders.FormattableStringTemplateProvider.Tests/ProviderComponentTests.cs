namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider.Tests;

public class ProviderComponentTests
{
    protected Mock<IExpressionStringParser> ExpressionStringParserMock { get; } = new();
    protected Mock<IFormattableStringParser> FormattableStringParserMock { get; } = new();
    protected ComponentRegistrationContext ComponentRegistrationContext { get; } = new();

    protected ProviderComponent CreateSut() => new(ExpressionStringParserMock.Object, FormattableStringParserMock.Object, ComponentRegistrationContext);

    public class Constructor : ProviderComponentTests
    {
        [Fact]
        public void Throws_On_Null_ExpressionStringParser()
        {
            // Act & Assert
            this.Invoking(_ => new ProviderComponent(expressionStringParser: null!, FormattableStringParserMock.Object, ComponentRegistrationContext))
                .Should().Throw<ArgumentNullException>().WithParameterName("expressionStringParser");
        }

        [Fact]
        public void Throws_On_Null_FormattableStringParser()
        {
            // Act & Assert
            this.Invoking(_ => new ProviderComponent(ExpressionStringParserMock.Object, formattableStringParser: null!, ComponentRegistrationContext))
                .Should().Throw<ArgumentNullException>().WithParameterName("formattableStringParser");
        }

        [Fact]
        public void Throws_On_Null_ComponentRegistrationContext()
        {
            // Act & Assert
            this.Invoking(_ => new ProviderComponent(ExpressionStringParserMock.Object, FormattableStringParserMock.Object, componentRegistrationContext: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("componentRegistrationContext");
        }
    }

    public class Supports : ProviderComponentTests
    {
        [Fact]
        public void Returns_False_When_Request_Is_Null()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(null!);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_False_When_Request_Is_Not_ExpressionStringTemplateIdentifier_Or_FormattableStringTemplateIdentifier()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(new Mock<ITemplateIdentifier>().Object);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_True_When_Request_Is_ExpressionStringTemplateIdentifier()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(new ExpressionStringTemplateIdentifier("template", CultureInfo.CurrentCulture));

            // Assert
            result.Should().BeTrue();
        }


        [Fact]
        public void Returns_True_When_Request_Is_FormattableStringTemplateIdentifier()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(new FormattableStringTemplateIdentifier("template", CultureInfo.CurrentCulture));

            // Assert
            result.Should().BeTrue();
        }
    }

    public class Create : ProviderComponentTests
    {
        [Fact]
        public void Throws_On_Null_Identifier()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(identifier: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("identifier");
        }

        [Fact]
        public void Throws_On_Identifier_Of_Wrong_Type()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(identifier: new Mock<ITemplateIdentifier>().Object))
               .Should().Throw<NotSupportedException>();
        }
        
        [Fact]
        public void Returns_FormattableStringTemplate_On_Identifier_Of_Correct_Type()
        {
            // Arrange
            var sut = CreateSut();
            var identifier = new ExpressionStringTemplateIdentifier("template", CultureInfo.CurrentCulture);

            // Act
            var result = sut.Create(identifier);

            // Assert
            result.Should().BeOfType<ExpressionStringTemplate>();
        }
    }

    public class StartSession : ProviderComponentTests
    {
        [Fact]
        public void Clears_Processors()
        {
            // Arrange
            ComponentRegistrationContext.Processors.Add(new Mock<IPlaceholderProcessor>().Object);
            var sut = CreateSut();

            // Act
            sut.StartSession();

            // Assert
            ComponentRegistrationContext.Processors.Should().BeEmpty();
        }
    }
}
