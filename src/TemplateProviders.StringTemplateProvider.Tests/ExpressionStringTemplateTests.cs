namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests;

public class ExpressionStringTemplateTests
{
    protected const string Template = "$\"Hello {Name}!\"";
    protected IExpressionEvaluator ExpressionEvaluatorMock { get; } = Substitute.For<IExpressionEvaluator>();
    protected ITemplateEngine EngineMock { get; } = Substitute.For<ITemplateEngine>();
    protected ITemplateComponentRegistry ComponentRegistryMock { get; } = Substitute.For<ITemplateComponentRegistry>();
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

    public class RenderAsync : ExpressionStringTemplateTests
    {
        [Fact]
        public async Task Throws_On_Null_Builder()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Task t =  sut.RenderAsync(builder: null!, CancellationToken.None);
            (await t.ShouldThrowAsync<ArgumentNullException>()).ParamName.ShouldBe("builder");
        }

        [Fact]
        public async Task Return_Result_On_NonSuccesful_Result_From_FormattableStringParser()
        {
            // Arrange
            ExpressionEvaluatorMock
                .EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
                .Returns(Result.Error<object?>("Kaboom!"));
            var sut = CreateSut();
            var builder = new StringBuilder();
            sut.Context = new TemplateEngineContext(new RenderTemplateRequest(new TemplateInstanceIdentifier(sut), builder), EngineMock, ComponentRegistryMock, sut);

            // Act
            var result = await sut.RenderAsync(builder, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom!");
        }

        [Fact]
        public async Task Appends_Result_From_ExpressionStringParser_To_Builder_On_Succesful_Result()
        {
            // Arrange
            ExpressionEvaluatorMock
                .EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
                .Returns(Result.Success<object?>("Parse result"));
            var sut = CreateSut();
            var builder = new StringBuilder();
            sut.Context = new TemplateEngineContext(new RenderTemplateRequest(new TemplateInstanceIdentifier(sut), builder), EngineMock, ComponentRegistryMock, sut);

            // Act
            await sut.RenderAsync(builder, CancellationToken.None);

            // Assert
            builder.ToString().ShouldBe("Parse result");
        }
    }
}
