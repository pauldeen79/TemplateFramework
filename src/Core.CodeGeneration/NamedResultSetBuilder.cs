namespace TemplateFramework.Core.CodeGeneration;

internal class NamedResultSetBuilder
{
    private readonly List<NamedResult<Func<Task<Result>>>> _resultset = new();

    public void Add(string name, Func<Task<Result<object?>>> value) => _resultset.Add(new(name, () => value().ContinueWith(x => (Result)x.Result, TaskScheduler.Current)));
    public void Add(string name, Func<Task<Result>> value) => _resultset.Add(new(name, value));

    public async Task<NamedResult<Result>[]> Build()
    {
        var results = new List<NamedResult<Result>>();

        foreach (var item in _resultset)
        {
            var result = await item.Result().ConfigureAwait(false);
            results.Add(new NamedResult<Result>(item.Name, result));
            if (!result.IsSuccessful())
            {
                break;
            }
        }

        return results.ToArray();
    }
}
