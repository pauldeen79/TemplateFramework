namespace TemplateFramework.Core.CodeGeneration;

internal class NamedResultSetBuilder
{
    private readonly List<NamedResult<Task<Result<object?>>>> _resultset = new();

    public void Add(string name, Task<Result<object?>> value) => _resultset.Add(new(name, value));
    public void Add(string name, Task<Result> value) => _resultset.Add(new(name, value.ContinueWith(x => Result.FromExistingResult<object?>(x.Result), TaskScheduler.Current)));

    public async Task<NamedResult<Result<object?>>[]> Build()
    {
        var results = await _resultset
            .SelectAsync(async x => new NamedResult<Result<object?>>(x.Name, await x.Result.ConfigureAwait(false)))
            .ConfigureAwait(false);

        return results
            .TakeWhileWithFirstNonMatching(x => x.Result.IsSuccessful())
            .ToArray();
    }
}
