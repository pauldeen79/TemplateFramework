namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests;

public class ExpressionStringTemplateTests
{
    protected const string Template = "Hello {Name}!";
    protected IExpressionEvaluator ExpressionEvaluatorMock { get; } = Substitute.For<IExpressionEvaluator>();
    protected ExpressionStringTemplateIdentifier Identifier { get; } = new ExpressionStringTemplateIdentifier(Template, CultureInfo.CurrentCulture);
    protected ComponentRegistrationContext ComponentRegistrationContext { get; } = new([new ComponentRegistrationContextFunction(Substitute.For<IMemberDescriptorMapper>())]);

    protected ExpressionStringTemplate CreateSut() => new(Identifier, ExpressionEvaluatorMock, ComponentRegistrationContext);

    public class Constructor : ExpressionStringTemplateTests
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(ExpressionStringTemplate).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(
                p => !new[] { "model", "iterationNumber", "iterationCount" }.Contains(p.Name));
        }
    }

    public class Render : ExpressionStringTemplateTests
    {
        [Fact]
        public async Task Throws_On_Null_Builder()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Task t =  sut.Render(builder: null!, CancellationToken.None);
            (await t.ShouldThrowAsync<ArgumentNullException>()).ParamName.ShouldBe("builder");
        }

        [Fact]
        public async Task Return_Result_On_NonSuccesful_Result_From_FormattableStringParser()
        {
            // Arrange
            ExpressionEvaluatorMock
                .EvaluateTypedAsync<GenericFormattableString>(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
                .Returns(Result.Error<GenericFormattableString>("Kaboom!"));
            var sut = CreateSut();
            var builder = new StringBuilder();

            // Act
            var result = await sut.Render(builder, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom!");
        }

        [Fact]
        public async Task Appends_Result_From_ExpressionStringParser_To_Builder_On_Succesful_Result()
        {
            // Arrange
            ExpressionEvaluatorMock
                .EvaluateTypedAsync<GenericFormattableString>(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
                .Returns(Result.Success<GenericFormattableString>("Parse result"));
            var sut = CreateSut();
            var builder = new StringBuilder();

            // Act
            await sut.Render(builder, CancellationToken.None);

            // Assert
            builder.ToString().ShouldBe("Parse result");
        }
    }
}
