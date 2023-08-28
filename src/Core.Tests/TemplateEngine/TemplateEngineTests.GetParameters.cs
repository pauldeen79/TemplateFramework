namespace TemplateFramework.Core.Tests;

public partial class TemplateEngineTests
{
    public class GetParameters : TemplateEngineTests
    {
        [Fact]
        public void Throws_On_Null_TemplateInstance()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.GetParameters(templateInstance: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("templateInstance");
        }

        [Fact]
        public void Returns_Correct_TemplateParameters_From_TemplateInstance_When_Not_Null()
        {
            // Arrange
            var sut = CreateSut();
            var template = new object();
            var parameters = new[] { new TemplateParameter("name", typeof(string)) };
            TemplateParameterExtractorMock.Setup(x => x.Extract(template)).Returns(parameters);

            // Act
            var result = sut.GetParameters(template);

            // Assert
            result.Should().BeEquivalentTo(parameters);
        }
    }
}
