namespace TemplateFramework.Console.Tests;

internal static class TestData
{
#if Windows
    internal const string BasePath = @"C:\Somewhere";
#elif Linux
    internal const string BasePath = @"/usr/bin/python3";
#elif OSX
    internal const string BasePath = @"/Users/moi/Downloads";
#else
    internal const string BasePath = "Unknown basepath, only Windows, Linux and OSX are supported";
#endif
    internal sealed class PlainTemplateWithModelAndAdditionalParameters<T> : IModelContainer<T>, IParameterizedTemplate
    {
        public T? Model { get; set; } = default!;

        public string AdditionalParameter { get; set; } = "";

        public Result SetParameter(string name, object? value)
        {
            if (name == nameof(AdditionalParameter))
            {
                AdditionalParameter = value?.ToString() ?? string.Empty;
                return Result.Success();
            }

            return Result.Continue();
        }

        public Result<ITemplateParameter[]> GetParameters() => Result.Success<ITemplateParameter[]>([new TemplateParameter(nameof(AdditionalParameter), typeof(T?))]);

        public override string ToString() => AdditionalParameter;
    }
}
