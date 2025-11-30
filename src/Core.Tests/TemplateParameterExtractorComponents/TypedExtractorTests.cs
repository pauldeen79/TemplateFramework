namespace TemplateFramework.Core.Tests.TemplateParameterExtractorComponents;

public class TypedExtractorTests
{
    public class Extract
    {
        [Theory, AutoMockData]
        public async Task Returns_Continue_On_Null_TemplateInstance(TypedExtractor sut)
        {
            // Act
            var result = await sut.ExtractAsync(templateInstance: null!, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Theory, AutoMockData]
        public async Task Returns_Continue_On_TemplateInstance_Of_Wrong_Type(TypedExtractor sut)
        {
            // Act
            var result = await sut.ExtractAsync(templateInstance: new object(), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public async Task Returns_Result_From_TemplateInstance_When_It_Implements_IParameterizedTemplate()
        {
            // Arrange
            var parameterizedTemplate = Substitute.For<IParameterizedTemplate>();
            var parameters = Result.Success<ITemplateParameter[]>([new TemplateParameter("SomeName", typeof(string))]);
            parameterizedTemplate.GetParametersAsync(Arg.Any<CancellationToken>()).Returns(parameters);
            var sut = new TypedExtractor();

            // Act
            var result = await sut.ExtractAsync(parameterizedTemplate, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(parameters.Value);
        }

        [Theory, AutoMockData]
        public async Task Returns_Continue_When_Template_Is_Null(TypedExtractor sut)
        {
            // Act
            var result = await sut.ExtractAsync(null!, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Theory, AutoMockData]
        public async Task Returns_Continue_When_Template_Is_Not_Null_But_Does_Not_Implement_IParameterizedTemplate(TypedExtractor sut)
        {
            // Act
            var result = await sut.ExtractAsync(new object(), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }
    }
}
