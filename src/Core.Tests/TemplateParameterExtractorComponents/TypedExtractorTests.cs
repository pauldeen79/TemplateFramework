using TemplateFramework.Abstractions.Templates;
using Shouldly;

namespace TemplateFramework.Core.Tests.TemplateParameterExtractorComponents;

public class TypedExtractorTests
{
    public class Extract
    {
        [Theory, AutoMockData]
        public void Throws_On_Null_TemplateInstance(TypedExtractor sut)
        {
            // Act & Assert
            Action a = () => sut.Extract(templateInstance: null!))
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("templateInstance");
        }

        [Theory, AutoMockData]
        public void Throws_On_TemplateInstance_Of_Wrong_Type(TypedExtractor sut)
        {
            // Act & Assert
            Action a = () => sut.Extract(templateInstance: new object()))
            a.ShouldThrow<ArgumentException>().ParamName.ShouldBe("templateInstance");
        }

        [Fact]
        public void Returns_Result_From_TemplateInstance_When_It_Implements_IParameterizedTemplate()
        {
            // Arrange
            var parameterizedTemplate = Substitute.For<IParameterizedTemplate>();
            var parameters = Result.Success<ITemplateParameter[]>([new TemplateParameter("SomeName", typeof(string))]);
            parameterizedTemplate.GetParameters().Returns(parameters);
            var sut = new TypedExtractor();

            // Act
            var result = sut.Extract(parameterizedTemplate);

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
