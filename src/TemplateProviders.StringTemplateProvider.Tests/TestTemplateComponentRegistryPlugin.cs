using CrossCutting.Utilities.ExpressionEvaluator.Builders.Extensions;

namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests;

public sealed class TestTemplateComponentRegistryPlugin : ITemplateComponentRegistryPlugin
{
    public ComponentRegistrationContext ComponentRegistrationContext { get; }

    public TestTemplateComponentRegistryPlugin(ComponentRegistrationContext componentRegistrationContext)
    {
        Guard.IsNotNull(componentRegistrationContext);

        ComponentRegistrationContext = componentRegistrationContext;
    }

    public Task<Result> Initialize(ITemplateComponentRegistry registry, CancellationToken cancellationToken)
    {
        var processorProcessorMock = Substitute.For<IExpressionComponent>();
        processorProcessorMock
            .EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
            .Returns(args => args.ArgAt<ExpressionEvaluatorContext>(0).Expression == "__test"
                ? Result.Success<GenericFormattableString>("Hello world!")
                : Result.Continue<GenericFormattableString>());

        processorProcessorMock
            .ParseAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
            .Returns(args => args.ArgAt<ExpressionEvaluatorContext>(0).Expression == "__test"
                ? new ExpressionParseResultBuilder().WithSourceExpression(args.ArgAt<ExpressionEvaluatorContext>(0).Expression).WithExpressionComponentType(GetType()).WithResultType(typeof(GenericFormattableString)).WithStatus(ResultStatus.Ok).Build()
                : new ExpressionParseResultBuilder().WithSourceExpression(args.ArgAt<ExpressionEvaluatorContext>(0).Expression).WithExpressionComponentType(GetType()).WithStatus(ResultStatus.Continue).Build());

        var functionMock = new FunctionMock();

        ComponentRegistrationContext.Expressions.Add(processorProcessorMock);
        ComponentRegistrationContext.AddFunction(functionMock);

        return Task.FromResult(Result.Success());
    }

    [MemberName("MyFunction")]
    private sealed class FunctionMock : IFunction
    {
        public Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
        {
            return Task.FromResult(Result.Success<object?>("Hello world!"));
        }
    }
}
