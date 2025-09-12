using CrossCutting.Utilities.ExpressionEvaluator.Builders.Extensions;

namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests;

public class FormattableStringTemplateTests
{
    protected const string Template = "Hello {Name}!";
    protected IExpressionEvaluator ExpressionEvaluatorMock { get; } = Substitute.For<IExpressionEvaluator>();
    protected ITemplateEngine EngineMock { get; } = Substitute.For<ITemplateEngine>();
    protected ITemplateComponentRegistry ComponentRegistryMock { get; } = Substitute.For<ITemplateComponentRegistry>();
    protected FormattableStringTemplateIdentifier Identifier { get; } = new FormattableStringTemplateIdentifier(Template, CultureInfo.CurrentCulture);
    protected ComponentRegistrationContext ComponentRegistrationContext { get; } = new([]);

    protected FormattableStringTemplate CreateSut() => new(Identifier, ExpressionEvaluatorMock, ComponentRegistrationContext);

    public class Constructor : FormattableStringTemplateTests
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(FormattableStringTemplate).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(
                p => !new[] { "model", "iterationNumber", "iterationCount" }.Contains(p.Name));
        }
    }

    public class GetParametersAsync : FormattableStringTemplateTests
    {
        [Fact]
        public async Task Returns_Parameters_From_Template()
        {
            // Arrange
            var sut = CreateSut();
            ExpressionEvaluatorMock.ParseAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
                .Returns(async x =>
                {
                    // Note that in this unit test, we have to mock the behavior of ExpressionEvaluator :)
                    // There is also an Integration test to prove it works in real life ;-)
                    var ctx = Result.FromExistingResult<TemplateFrameworkStringContext>(await x.ArgAt<ExpressionEvaluatorContext>(0).State["context"].ConfigureAwait(false));
                    ctx.GetValueOrThrow().ParameterNamesList.Add("Name");
                    return new ExpressionParseResultBuilder()
                        .WithSourceExpression("Dummy")
                        .WithStatus(ResultStatus.Ok)
                        .Build();
                });
            sut.Context = new TemplateEngineContext(new RenderTemplateRequest(new TemplateInstanceIdentifier(sut), new StringBuilder()), EngineMock, ComponentRegistryMock, sut);

            // Act
            var result = await sut.GetParametersAsync(CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.GetValueOrThrow().Select(x => x.Name).ToArray().ShouldBeEquivalentTo(new[] { "Name" });
        }
    }

    public class RenderAsync : FormattableStringTemplateTests
    {
        [Fact]
        public async Task Throws_On_Null_Builder()
        {
            // Arrange
            var sut = CreateSut();
            sut.Context = Substitute.For<ITemplateEngineContext>();

            // Act & Assert
            Task t = sut.RenderAsync(builder: null!, CancellationToken.None);
            (await t.ShouldThrowAsync<ArgumentNullException>()).ParamName.ShouldBe("builder");
        }

        [Fact]
        public async Task Returns_Reuslt_On_NonSuccesful_Result_From_FormattableStringParser()
        {
            // Arrange
            ExpressionEvaluatorMock
                .EvaluateTypedAsync<GenericFormattableString>(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
                .Returns(Result.Error<GenericFormattableString>("Kaboom!"));
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
        public async Task Appends_Result_From_FormattableStringParser_To_Builder_On_Succesful_Result()
        {
            // Arrange
            ExpressionEvaluatorMock
                .EvaluateTypedAsync<GenericFormattableString>(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
                .Returns(Result.Success<GenericFormattableString>("Parse result"));
            var sut = CreateSut();
            var builder = new StringBuilder();
            sut.Context = new TemplateEngineContext(new RenderTemplateRequest(new TemplateInstanceIdentifier(sut), builder), EngineMock, ComponentRegistryMock, sut);

            // Act
            await sut.RenderAsync(builder, CancellationToken.None);

            // Assert
            builder.ToString().ShouldBe("Parse result");
        }
    }

    public class SetParameterAsync : FormattableStringTemplateTests
    {
        [Fact]
        public async Task Adds_Parameter_To_Context()
        {
            // Arrange
            var sut = CreateSut();
            IDictionary<string, object?>? dictionary = null;
            ExpressionEvaluatorMock
                .EvaluateTypedAsync<GenericFormattableString>(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
                .Returns(async x =>
                {
                    var ctx = Result.FromExistingResult<TemplateFrameworkStringContext>(await x.ArgAt<ExpressionEvaluatorContext>(0).State["context"].ConfigureAwait(false));
                    dictionary = ctx.GetValueOrThrow().ParametersDictionary;

                    return Result.Success<GenericFormattableString>(string.Empty);
                });
            sut.Context = new TemplateEngineContext(new RenderTemplateRequest(new TemplateInstanceIdentifier(sut), new StringBuilder()), EngineMock, ComponentRegistryMock, sut);

            // Act
            var result = await sut.SetParameterAsync("Name", "Value", CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            (await sut.RenderAsync(new StringBuilder(), CancellationToken.None)).ThrowIfNotSuccessful();
            dictionary.ShouldNotBeNull();
            dictionary!.First().Key.ShouldBe("Name");
            dictionary!.First().Value.ShouldBe("Value");
        }
    }
}
