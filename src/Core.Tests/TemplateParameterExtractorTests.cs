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
        public async Task Throws_On_Null_TemplateInstance(TemplateParameterExtractor sut)
        {
            // Act & Assert
            Task t = sut.ExtractAsync(templateInstance: null!, CancellationToken.None);
            (await t.ShouldThrowAsync<ArgumentNullException>()).ParamName.ShouldBe("templateInstance");
        }

        [Theory, AutoMockData]
        public async Task Returns_Empty_Result_On_Unsupported_Type(
            [Frozen] ITemplateParameterExtractorComponent templateParameterExtractorComponent,
            TemplateParameterExtractor sut)
        {
            // Arrange
            templateParameterExtractorComponent.Supports(Arg.Any<object>()).Returns(false);

            // Act
            var result = await sut.ExtractAsync(templateInstance: new object(), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
            result.Value.ShouldBeEmpty();
        }

        [Fact]
        public async Task Returns_Result_From_Component_On_Supported_Type()
        {
            // Arrange
            var template = new object();
            var templateParameterExtractorComponent = Substitute.For<ITemplateParameterExtractorComponent>();
            var parametersResult = Result.Success<ITemplateParameter[]>([new TemplateParameter("name", typeof(string))]);
            templateParameterExtractorComponent.Supports(template).Returns(true);
            templateParameterExtractorComponent.ExtractAsync(template, Arg.Any<CancellationToken>()).Returns(parametersResult);
            var sut = new TemplateParameterExtractor([templateParameterExtractorComponent]);

            // Act
            var result = await sut.ExtractAsync(template, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(parametersResult.Value);
        }
    }
}
