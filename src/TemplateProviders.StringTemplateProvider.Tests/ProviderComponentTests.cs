namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests;

public class ProviderComponentTests
{
    protected IExpressionStringParser ExpressionStringParserMock { get; } = Substitute.For<IExpressionStringParser>();
    protected IFormattableStringParser FormattableStringParserMock { get; } = Substitute.For<IFormattableStringParser>();
    protected ComponentRegistrationContext ComponentRegistrationContext { get; } = new();

    protected ProviderComponent CreateSut() => new(ExpressionStringParserMock, FormattableStringParserMock, ComponentRegistrationContext);

    public class Constructor : ProviderComponentTests
    {
        [Fact]
        public void Throws_On_Null_ExpressionStringParser()
        {
            // Act & Assert
            this.Invoking(_ => new ProviderComponent(expressionStringParser: null!, FormattableStringParserMock, ComponentRegistrationContext))
                .Should().Throw<ArgumentNullException>().WithParameterName("expressionStringParser");
        }

        [Fact]
        public void Throws_On_Null_FormattableStringParser()
        {
            // Act & Assert
            this.Invoking(_ => new ProviderComponent(ExpressionStringParserMock, formattableStringParser: null!, ComponentRegistrationContext))
                .Should().Throw<ArgumentNullException>().WithParameterName("formattableStringParser");
        }

        [Fact]
        public void Throws_On_Null_ComponentRegistrationContext()
        {
            // Act & Assert
            this.Invoking(_ => new ProviderComponent(ExpressionStringParserMock, FormattableStringParserMock, componentRegistrationContext: null!))
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
            var result = sut.Supports(Substitute.For<ITemplateIdentifier>());

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
            sut.Invoking(x => x.Create(identifier: Substitute.For<ITemplateIdentifier>()))
               .Should().Throw<NotSupportedException>();
        }
        
        [Fact]
        public void Returns_ExpressionStringTemplate_On_Identifier_Of_Correct_Type()
        {
            // Arrange
            var sut = CreateSut();
            var identifier = new ExpressionStringTemplateIdentifier("template", CultureInfo.CurrentCulture);

            // Act
            var result = sut.Create(identifier);

            // Assert
            result.Should().BeOfType<ExpressionStringTemplate>();
        }

        [Fact]
        public void Returns_FormattableStringTemplate_On_Identifier_Of_Correct_Type()
        {
            // Arrange
            var sut = CreateSut();
            var identifier = new FormattableStringTemplateIdentifier("template", CultureInfo.CurrentCulture);

            // Act
            var result = sut.Create(identifier);

            // Assert
            result.Should().BeOfType<FormattableStringTemplate>();
        }
    }

    public class StartSession : ProviderComponentTests
    {
        [Fact]
        public void Clears_PlaceholderProcessors()
        {
            // Arrange
            ComponentRegistrationContext.PlaceholderProcessors.Add(Substitute.For<IPlaceholderProcessor>());
            var sut = CreateSut();

            // Act
            sut.StartSession();

            // Assert
            ComponentRegistrationContext.PlaceholderProcessors.Should().BeEmpty();
        }

        [Fact]
        public void Clears_FunctionResultParsers()
        {
            // Arrange
            ComponentRegistrationContext.FunctionResultParsers.Add(Substitute.For<IFunctionResultParser>());
            var sut = CreateSut();

            // Act
            sut.StartSession();

            // Assert
            ComponentRegistrationContext.FunctionResultParsers.Should().BeEmpty();
        }
    }
}
