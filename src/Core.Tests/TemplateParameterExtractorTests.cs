namespace TemplateFramework.Core.Tests;

public class TemplateParameterExtractorTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(TemplateParameterExtractor).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }
    public class Extract
    {
        [Theory, AutoMockData]
        public void Throws_On_Null_TemplateInstance(TemplateParameterExtractor sut)
        {
            // Act & Assert
            sut.Invoking(x => x.Extract(templateInstance: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("templateInstance");
        }

        [Theory, AutoMockData]
        public void Returns_Empty_Result_On_Unsupported_Type(
            [Frozen] ITemplateParameterExtractorComponent templateParameterExtractorComponent, 
            TemplateParameterExtractor sut)
        {
            // Arrange
            templateParameterExtractorComponent.Supports(Arg.Any<object>()).Returns(false);

            // Act
            var result = sut.Extract(templateInstance: new object());

            // Assert
            result.Should().BeEmpty();
        }

        [Theory, AutoMockData]
        public void Returns_Result_From_Component_On_Supported_Type(
            [Frozen] ITemplateParameterExtractorComponent templateParameterExtractorComponent,
            TemplateParameterExtractor sut)
        {
            // Arrange
            var template = new object();
            var parameters = new[] { new TemplateParameter("name", typeof(string)) };
            templateParameterExtractorComponent.Supports(template).Returns(true);
            templateParameterExtractorComponent.Extract(template).Returns(parameters);

            // Act
            var result = sut.Extract(template);

            // Assert
            result.Should().BeEquivalentTo(parameters);
        }
    }
}
