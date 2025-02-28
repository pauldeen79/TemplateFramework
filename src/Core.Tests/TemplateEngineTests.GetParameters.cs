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
            Action a = () => sut.GetParameters(templateInstance: null!);
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("templateInstance");
        }

        [Fact]
        public async Task Returns_Correct_TemplateParameters_From_TemplateInstance_When_Not_Null()
        {
            // Arrange
            var sut = CreateSut();
            var template = new object();
            var parameters = Result.Success<ITemplateParameter[]>([new TemplateParameter("name", typeof(string))]);
            TemplateParameterExtractorMock.Extract(template).Returns(parameters);

            // Act
            var result = await sut.GetParameters(template);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(parameters.Value);
        }
    }
}
