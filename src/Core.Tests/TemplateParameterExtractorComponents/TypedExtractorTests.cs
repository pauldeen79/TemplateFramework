namespace TemplateFramework.Core.Tests.TemplateParameterExtractorComponents;

public class TypedExtractorTests
{
    public class Extract
    {
        [Theory, AutoMockData]
        public async Task Throws_On_Null_TemplateInstance(TypedExtractor sut)
        {
            // Act & Assert
            Task t = sut.ExtractAsync(templateInstance: null!, CancellationToken.None);
            (await t.ShouldThrowAsync<ArgumentNullException>()).ParamName.ShouldBe("templateInstance");
        }

        [Theory, AutoMockData]
        public async Task Throws_On_TemplateInstance_Of_Wrong_Type(TypedExtractor sut)
        {
            // Act & Assert
            Task t = sut.ExtractAsync(templateInstance: new object(), CancellationToken.None);
            (await t.ShouldThrowAsync<ArgumentException>()).ParamName.ShouldBe("templateInstance");
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
    }

    public class Supports
    {
        [Theory, AutoMockData]
        public void Returns_True_When_Template_Implements_IParameterizedTemplate(
            [Frozen] IParameterizedTemplate parameterizedTemplate,
            TypedExtractor sut)
        {
            // Act
            var result = sut.Supports(parameterizedTemplate);

            // Assert
            result.ShouldBeTrue();
        }

        [Theory, AutoMockData]
        public void Returns_False_When_Template_Is_Null(TypedExtractor sut)
        {
            // Act
            var result = sut.Supports(null!);

            // Assert
            result.ShouldBeFalse();
        }

        [Theory, AutoMockData]
        public void Returns_False_When_Template_Is_Not_Null_But_Does_Not_Implement_IParameterizedTemplate(TypedExtractor sut)
        {
            // Act
            var result = sut.Supports(new object());

            // Assert
            result.ShouldBeFalse();
        }
    }
}
