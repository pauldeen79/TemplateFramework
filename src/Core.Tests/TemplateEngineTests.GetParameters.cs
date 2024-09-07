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
               .Should().ThrowAsync<ArgumentNullException>().WithParameterName("templateInstance");
        }

        [Fact]
        public async Task Returns_Correct_TemplateParameters_From_TemplateInstance_When_Not_Null()
        {
            // Arrange
            var sut = CreateSut();
            var template = new object();
            var parameters = new[] { new TemplateParameter("name", typeof(string)) };
            TemplateParameterExtractorMock.Extract(template).Returns(parameters);

            // Act
            var result = await sut.GetParameters(template);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(parameters);
        }
    }
}
