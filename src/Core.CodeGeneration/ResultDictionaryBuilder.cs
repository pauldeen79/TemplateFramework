namespace TemplateFramework.Core.CodeGeneration;

internal class ResultDictionaryBuilder
{
    private readonly Dictionary<string, Func<Task<Result>>> _resultset = new();

    public void Add(string name, Func<Task<Result<object?>>> value) => _resultset.Add(name, () => value().ContinueWith(x => (Result)x.Result, TaskScheduler.Current));
    public void Add(string name, Func<Task<Result>> value) => _resultset.Add(name, value);

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
