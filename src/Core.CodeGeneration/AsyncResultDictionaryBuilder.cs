namespace TemplateFramework.Core.CodeGeneration;

internal class AsyncResultDictionaryBuilder
{
    private readonly Dictionary<string, Func<Task<Result>>> _resultset = new();

    public AsyncResultDictionaryBuilder Add<T>(string name, Func<Task<Result<T>>> value)
    {
        _resultset.Add(name, () => value().ContinueWith(x => (Result)x.Result, TaskScheduler.Current));
        return this;
    }

    public AsyncResultDictionaryBuilder Add(string name, Func<Task<Result>> value)
    {
        _resultset.Add(name, value);
        return this;
    }

    public async Task<Dictionary<string, Result>> Build()
    {
        var results = new Dictionary<string, Result>();

        foreach (var item in _resultset)
        {
            var result = await item.Value().ConfigureAwait(false);
            results.Add(item.Key, result);
            if (!result.IsSuccessful())
            {
                break;
            }
        }

        return results;
    }
}
