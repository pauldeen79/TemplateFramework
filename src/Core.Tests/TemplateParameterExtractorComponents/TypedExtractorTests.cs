namespace TemplateFramework.Core.Tests.TemplateParameterExtractorComponents;

public class TypedExtractorTests
{
    protected TypedExtractor CreateSut() => new();

    protected IParameterizedTemplate ParameterizedTemplateMock { get; } = Substitute.For<IParameterizedTemplate>();

    public class Extract : TypedExtractorTests
    {
        [Fact]
        public void Throws_On_Null_TemplateInstance()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Extract(templateInstance: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("templateInstance");
        }

        [Fact]
        public void Throws_On_TemplateInstance_Of_Wrong_Type()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Extract(templateInstance: new object()))
               .Should().Throw<ArgumentException>().WithParameterName("templateInstance");
        }

        [Fact]
        public void Returns_Result_From_TemplateInstance_When_It_Implements_IParameterizedTemplate()
        {
            // Arrange
            var sut = CreateSut();
            var parameters = new[] { new TemplateParameter("SomeName", typeof(string)) };
            ParameterizedTemplateMock.GetParameters().Returns(parameters);

            // Act
            var result = sut.Extract(ParameterizedTemplateMock);

            // Assert
            result.Should().BeEquivalentTo(parameters);
        }
    }

    public class Supports : TypedExtractorTests
    {
        [Fact]
        public void Returns_True_When_Template_Implements_IParameterizedTemplate()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(ParameterizedTemplateMock);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_False_When_Template_Is_Null()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(null!);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_False_When_Template_Is_Not_Null_But_Does_Not_Implement_IParameterizedTemplate()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(new object());

            // Assert
            result.Should().BeFalse();
        }
    }
}
