namespace TemplateFramework.Core.Tests;

public class TemplateParameterExtractorTests
{
    protected TemplateParameterExtractor CreateSut() => new(new[] { TemplateParameterExtractorComponentMock.Object });

    protected Mock<ITemplateParameterExtractorComponent> TemplateParameterExtractorComponentMock { get; } = new();

    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Components()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateParameterExtractor(components: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("components");
        }
    }
    public class Extract : TemplateParameterExtractorTests
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
        public void Throws_On_Unsupported_Type()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Extract(templateInstance: new object()))
               .Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void Returns_Result_From_Component_On_Supported_Type()
        {
            // Arrange
            var sut = CreateSut();
            var template = new object();
            var parameters = new[] { new TemplateParameter("name", typeof(string)) };
            TemplateParameterExtractorComponentMock.Setup(x => x.Supports(template)).Returns(true);
            TemplateParameterExtractorComponentMock.Setup(x => x.Extract(template)).Returns(parameters);

            // Act
            var result = sut.Extract(template);

            // Assert
            result.Should().BeEquivalentTo(parameters);
        }
    }
}
