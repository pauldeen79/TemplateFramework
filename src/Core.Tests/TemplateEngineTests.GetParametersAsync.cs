namespace TemplateFramework.Core.Tests;

public partial class TemplateEngineTests
{
    public class GetParametersAsync : TemplateEngineTests
    {
        [Fact]
        public async Task Throws_On_Null_TemplateInstance()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Task t = sut.GetParametersAsync(templateInstance: null!, CancellationToken.None);
            (await t.ShouldThrowAsync<ArgumentNullException>()).ParamName.ShouldBe("templateInstance");
        }

        [Fact]
        public async Task Returns_Correct_TemplateParameters_From_TemplateInstance_When_Not_Null()
        {
            // Arrange
            var sut = CreateSut();
            var template = new object();
            var parameters = Result.Success<ITemplateParameter[]>([new TemplateParameter("name", typeof(string))]);
            TemplateParameterExtractorMock.ExtractAsync(template, CancellationToken.None).Returns(parameters);

            // Act
            var result = await sut.GetParametersAsync(template, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(parameters.Value);
        }
    }
}
