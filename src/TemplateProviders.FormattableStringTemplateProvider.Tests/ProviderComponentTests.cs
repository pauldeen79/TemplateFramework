namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider.Tests;

public class ProviderComponentTests
{
    protected Mock<IFormattableStringParser> FormattableStringParserMock { get; } = new();

    protected ProviderComponent CreateSut() => new(FormattableStringParserMock.Object);

    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_FormattableStringParser()
        {
            // Act & Assert
            this.Invoking(_ => new ProviderComponent(formattableStringParser: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("formattableStringParser");
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
        public void Returns_False_When_Request_Is_Not_CreateFormattableStringTemplateRequest()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(new Mock<ITemplateIdentifier>().Object);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_True_When_Request_Is_CreateFormattableStringTemplateRequest()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(new CreateFormattableStringTemplateRequest("template", CultureInfo.CurrentCulture));

            // Assert
            result.Should().BeTrue();
        }
    }

    public class Create : ProviderComponentTests
    {
        [Fact]
        public void Throws_On_Null_Request()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(request: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("request");
        }

        [Fact]
        public void Throws_On_Request_Of_Wrong_Type()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(request: new Mock<ITemplateIdentifier>().Object))
               .Should().Throw<ArgumentException>().WithParameterName("request");
        }
        
        [Fact]
        public void Returns_FormattableStringTemplate_On_Request_Of_Correct_Type()
        {
            // Arrange
            var sut = CreateSut();
            var request = new CreateFormattableStringTemplateRequest("template", CultureInfo.CurrentCulture);

            // Act
            var result = sut.Create(request);

            // Assert
            result.Should().BeOfType<FormattableStringTemplate>();
        }
    }
}
