namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests;

public class TemplateFrameworkContextPlaceholderProcessorTests : TestBase<TemplateFrameworkContextExpressionComponent>
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(ComponentRegistrationContext).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }

    public class Evaluate : TemplateFrameworkContextPlaceholderProcessorTests
    {
        private ComponentRegistrationContext ComponentRegistrationContext { get; } = new([]);
        public IExpressionEvaluator ExpressionEvaluator { get; }

        public Evaluate()
        {
            ExpressionEvaluator = Fixture.Freeze<IExpressionEvaluator>();
        }

        [Fact]
        public async Task Returns_Continue_When_Context_Is_Not_TemplateFrameworkFormattableStringContext()
        {
            // Arrange
            var context = new Dictionary<string, Func<Task<Result<object?>>>> { { "context", () => Task.FromResult(Result.Success<object?>("some context that's not of type TemplateFrameworkFormattableStringContext")) } };
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(new ExpressionEvaluatorContext("some template", new ExpressionEvaluatorSettingsBuilder(), ExpressionEvaluator, context), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public async Task Returns_Success_With_Parameter_Value_When_Context_Is_TemplateFrameworkFormattableStringContext_And_Parameter_Is_Known_And_Not_Null()
        {
            // Arrange
            var parametersDictionary = new Dictionary<string, object?>
            {
                { "Name", "Value" }
            };
            var context = new Dictionary<string, Func<Task<Result<object?>>>> { { "context", () => Task.FromResult(Result.Success<object?>(new TemplateFrameworkStringContext(parametersDictionary, ComponentRegistrationContext, false))) } };
            var sut = CreateSut();

            // Act
            var result = Result.FromExistingResult<GenericFormattableString>(await sut.EvaluateAsync(new ExpressionEvaluatorContext("Name", new ExpressionEvaluatorSettingsBuilder(), ExpressionEvaluator, context), CancellationToken.None));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value!.ToString(CultureInfo.InvariantCulture).ShouldBe("Value");
        }

        [Fact]
        public async Task Returns_Success_With_StringEmpty_When_Context_Is_TemplateFrameworkFormattableStringContext_And_Parameter_Is_Known_But_Null()
        {
            // Arrange
            var parametersDictionary = new Dictionary<string, object?>
            {
                { "Name", null }
            };
            var context = new Dictionary<string, Func<Task<Result<object?>>>> { { "context", () => Task.FromResult(Result.Success<object?>(new TemplateFrameworkStringContext(parametersDictionary, ComponentRegistrationContext, false))) } };
            var sut = CreateSut();

            // Act
            var result = Result.FromExistingResult<GenericFormattableString>(await sut.EvaluateAsync(new ExpressionEvaluatorContext("Name", new ExpressionEvaluatorSettingsBuilder(), ExpressionEvaluator, context), CancellationToken.None));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value!.ToString(CultureInfo.InvariantCulture).ShouldBeEmpty();
        }

        [Fact]
        public async Task Returns_Continue_When_Context_Is_TemplateFrameworkFormattableStringContext_And_Parameter_Is_Unknown()
        {
            // Arrange
            var parametersDictionary = new Dictionary<string, object?>();
            var context = new Dictionary<string, Func<Task<Result<object?>>>> { { "context", () => Task.FromResult(Result.Success<object?>(new TemplateFrameworkStringContext(parametersDictionary, ComponentRegistrationContext, false))) } };
            var sut = CreateSut();

            // Act
            var result = Result.FromExistingResult<GenericFormattableString>(await sut.EvaluateAsync(new ExpressionEvaluatorContext("Name", new ExpressionEvaluatorSettingsBuilder(), ExpressionEvaluator, context), CancellationToken.None));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public async Task Adds_Parameter_Name_To_List()
        {
            // Arrange
            var parametersDictionary = new Dictionary<string, object?>();
            var ctx = new TemplateFrameworkStringContext(parametersDictionary, ComponentRegistrationContext, false);
            var context = new Dictionary<string, Func<Task<Result<object?>>>> { { "context", () => Task.FromResult(Result.Success<object?>(ctx)) } };
            var sut = CreateSut();

            // Act
            _ = Result.FromExistingResult<GenericFormattableString>(await sut.EvaluateAsync(new ExpressionEvaluatorContext("Name", new ExpressionEvaluatorSettingsBuilder(), ExpressionEvaluator, context), CancellationToken.None));

            // Assert
            ctx.ParameterNamesList.ToArray().ShouldBeEquivalentTo(new[] { "Name" });
        }

        [Fact]
        public async Task Does_Not_Add_Parameter_Name_To_List_When_Name_Starts_With_Double_Underscore()
        {
            // Arrange
            var parametersDictionary = new Dictionary<string, object?>();
            var ctx = new TemplateFrameworkStringContext(parametersDictionary, ComponentRegistrationContext, false);
            var context = new Dictionary<string, Func<Task<Result<object?>>>> { { "context", () => Task.FromResult(Result.Success<object?>(ctx)) } };
            var sut = CreateSut();

            // Act
            _ = Result.FromExistingResult<GenericFormattableString>(await sut.EvaluateAsync(new ExpressionEvaluatorContext("__Name", new ExpressionEvaluatorSettingsBuilder(), ExpressionEvaluator, context), CancellationToken.None));

            // Assert
            ctx.ParameterNamesList.ShouldBeEmpty();
        }
    }
}
