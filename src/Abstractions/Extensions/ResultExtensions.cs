namespace TemplateFramework.Abstractions.Extensions;

public static class ResultExtensions
{
    public static void Either(this Result instance, Action<Result> errorDelegate, Action successDelegate)
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

        if (!instance.IsSuccessful())
        {
            errorDelegate(instance);
            return;
        }

        successDelegate();
    }

    public static void Either<T>(this Result<T>instance, Action<Result<T>> errorDelegate, Action<Result<T>> successDelegate)
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

        if (!instance.IsSuccessful())
        {
            errorDelegate(instance);
            return;
        }

        successDelegate(instance);
    }

    public static Result Either<T>(this Result<T> instance, Func<Result<T>, Result> errorDelegate, Func<Result<T>, Result> successDelegate)
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

        if (!instance.IsSuccessful())
        {
            return errorDelegate(instance);
        }

        return successDelegate(instance);
    }

    public static Result<TResult> Either<TSource, TResult>(this Result<TSource> instance, Func<Result<TSource>, Result<TResult>> errorDelegate, Func<Result<TSource>, Result<TResult>> successDelegate)
    {
        errorDelegate = errorDelegate.IsNotNull(nameof(errorDelegate));
        successDelegate = successDelegate.IsNotNull(nameof(successDelegate));

        if (!instance.IsSuccessful())
        {
            return errorDelegate(instance);
        }

        return successDelegate(instance);
    }

    public static Result PerformUntilFailure<T>(this IEnumerable<T> instance, Func<T, Result> actionDelegate)
    {
        actionDelegate = actionDelegate.IsNotNull(nameof(actionDelegate));

        return instance
            .Select(x => actionDelegate(x))
            .TakeWhileWithFirstNonMatching(x => x.IsSuccessful())
            .LastOrDefault() ?? Result.Success();
    }

    public static async Task<Result> PerformUntilFailure<T>(this IEnumerable<T> instance, Func<T, Task<Result>> actionDelegate)
    {
        actionDelegate = actionDelegate.IsNotNull(nameof(actionDelegate));

        foreach (var item in instance)
        {
            var result = await actionDelegate(item).ConfigureAwait(false);
            if (!result.IsSuccessful())
            {
                return result;
            }
        }

        return Result.Success();
    }

    public static async Task<Result> PerformUntilFailure(this IEnumerable instance, Func<object, Task<Result>> actionDelegate)
    {
        actionDelegate = actionDelegate.IsNotNull(nameof(actionDelegate));

        foreach (var item in instance)
        {
            var result = await actionDelegate(item).ConfigureAwait(false);
            if (!result.IsSuccessful())
            {
                return result;
            }
        }

        return Result.Success();
    }
}
