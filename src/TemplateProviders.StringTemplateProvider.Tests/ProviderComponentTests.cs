﻿namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests;

public class ProviderComponentTests
{
    protected IExpressionStringEvaluator ExpressionStringEvaluatorMock { get; } = Substitute.For<IExpressionStringEvaluator>();
    protected IFormattableStringParser FormattableStringParserMock { get; } = Substitute.For<IFormattableStringParser>();
    protected ComponentRegistrationContext ComponentRegistrationContext { get; } = new([new ComponentRegistrationContextFunction(Substitute.For<IFunctionDescriptorMapper>())]);

    protected ProviderComponent CreateSut() => new(ExpressionStringEvaluatorMock, FormattableStringParserMock, ComponentRegistrationContext);

    public class Constructor : ProviderComponentTests
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(ProviderComponent).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(
                p => !new[] { "model", "iterationNumber", "iterationCount" }.Contains(p.Name));
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
            result.ShouldBeFalse();
        }

        [Fact]
        public void Returns_False_When_Request_Is_Not_ExpressionStringTemplateIdentifier_Or_FormattableStringTemplateIdentifier()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(Substitute.For<ITemplateIdentifier>());

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void Returns_True_When_Request_Is_ExpressionStringTemplateIdentifier()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(new ExpressionStringTemplateIdentifier("template", CultureInfo.CurrentCulture));

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void Returns_True_When_Request_Is_FormattableStringTemplateIdentifier()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(new FormattableStringTemplateIdentifier("template", CultureInfo.CurrentCulture));

            // Assert
            result.ShouldBeTrue();
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
            Action a = () => sut.Create(identifier: null!);
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("identifier");
        }

        [Fact]
        public void Throws_On_Identifier_Of_Wrong_Type()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Action a = () => sut.Create(identifier: Substitute.For<ITemplateIdentifier>());
            a.ShouldThrow<NotSupportedException>();
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
            result.ShouldBeOfType<ExpressionStringTemplate>();
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
            result.ShouldBeOfType<FormattableStringTemplate>();
        }
    }

    public class StartSession : ProviderComponentTests
    {
        [Fact]
        public async Task Clears_PlaceholderProcessors()
        {
            // Arrange
            ComponentRegistrationContext.Placeholders.Add(Substitute.For<IPlaceholder>());
            var sut = CreateSut();

            // Act
            await sut.StartSession(CancellationToken.None);

            // Assert
            ComponentRegistrationContext.Placeholders.ShouldBeEmpty();
        }

        [Fact]
        public async Task Clears_FunctionResultParsers()
        {
            // Arrange
            ComponentRegistrationContext.Functions.Add(Substitute.For<IFunction>());
            var sut = CreateSut();

            // Act
            await sut.StartSession(CancellationToken.None);

            // Assert
            ComponentRegistrationContext.Functions.ShouldBeEmpty();
        }
    }
}
