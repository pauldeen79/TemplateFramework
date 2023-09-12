namespace TemplateFramework.Core.Tests;

public class TemplateParameterExtractorTests
{
    protected TemplateParameterExtractor CreateSut() => new(new[] { TemplateParameterExtractorComponentMock });

    protected ITemplateParameterExtractorComponent TemplateParameterExtractorComponentMock { get; } = Substitute.For<ITemplateParameterExtractorComponent>();

    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(TemplateParameterExtractor).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
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
        public void Returns_Empty_Result_On_Unsupported_Type()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Extract(templateInstance: new object());

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Returns_Result_From_Component_On_Supported_Type()
        {
            // Arrange
            var sut = CreateSut();
            var template = new object();
            var parameters = new[] { new TemplateParameter("name", typeof(string)) };
            TemplateParameterExtractorComponentMock.Supports(template).Returns(true);
            TemplateParameterExtractorComponentMock.Extract(template).Returns(parameters);

            // Act
            var result = sut.Extract(template);

            // Assert
            result.Should().BeEquivalentTo(parameters);
        }
    }
}
