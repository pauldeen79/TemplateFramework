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
            Action a = () => sut.Extract(templateInstance: null!))
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("templateInstance");
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
            result.Status.ShouldBe(ResultStatus.Continue);
            result.Value.ShouldBeEmpty();
        }

        [Fact]
        public void Returns_Result_From_Component_On_Supported_Type()
        {
            // Arrange
            var template = new object();
            var templateParameterExtractorComponent = Substitute.For<ITemplateParameterExtractorComponent>();
            var parametersResult = Result.Success<ITemplateParameter[]>([new TemplateParameter("name", typeof(string))]);
            templateParameterExtractorComponent.Supports(template).Returns(true);
            templateParameterExtractorComponent.Extract(template).Returns(parametersResult);
            var sut = new TemplateParameterExtractor([templateParameterExtractorComponent]);

            // Act
            var result = sut.Extract(template);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(parametersResult.Value);
        }
    }
}
