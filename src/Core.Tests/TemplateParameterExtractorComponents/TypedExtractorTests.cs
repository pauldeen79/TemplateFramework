namespace TemplateFramework.Core.Tests.TemplateParameterExtractorComponents;

public class TypedExtractorTests
{
    public class Extract
    {
        [Theory, AutoMockData]
        public void Throws_On_Null_TemplateInstance(TypedExtractor sut)
        {
            // Act & Assert
            sut.Invoking(x => x.Extract(templateInstance: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("templateInstance");
        }

        [Theory, AutoMockData]
        public void Throws_On_TemplateInstance_Of_Wrong_Type(TypedExtractor sut)
        {
            // Act & Assert
            sut.Invoking(x => x.Extract(templateInstance: new object()))
               .Should().Throw<ArgumentException>().WithParameterName("templateInstance");
        }

        [Theory, AutoMockData]
        public void Returns_Result_From_TemplateInstance_When_It_Implements_IParameterizedTemplate(
            [Frozen] IParameterizedTemplate parameterizedTemplate,
            TypedExtractor sut)
        {
            // Arrange
            var parameters = new[] { new TemplateParameter("SomeName", typeof(string)) };
            parameterizedTemplate.GetParameters().Returns(parameters);

            // Act
            var result = sut.Extract(parameterizedTemplate);

            // Assert
            result.Should().BeEquivalentTo(parameters);
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
            result.Should().BeTrue();
        }

        [Theory, AutoMockData]
        public void Returns_False_When_Template_Is_Null(TypedExtractor sut)
        {
            // Act
            var result = sut.Supports(null!);

            // Assert
            result.Should().BeFalse();
        }

        [Theory, AutoMockData]
        public void Returns_False_When_Template_Is_Not_Null_But_Does_Not_Implement_IParameterizedTemplate(TypedExtractor sut)
        {
            // Act
            var result = sut.Supports(new object());

            // Assert
            result.Should().BeFalse();
        }
    }
}
