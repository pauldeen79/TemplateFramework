namespace TemplateFramework.Core;

public class ViewModelTemplateParameterConverter : ITemplateParameterConverter
{
    private readonly Func<IEnumerable<IViewModel>> _factory;

    public ViewModelTemplateParameterConverter(Func<IEnumerable<IViewModel>> factory)
    {
        Guard.IsNotNull(factory);

        _factory = factory;
    }

    public Result<object?> Convert(object? value, Type type, ITemplateEngineContext context)
    {
        if (value is null)
        {
            return Result.Continue<object?>();
        }

        var viewModelItem = _factory.Invoke()
            .Select(viewModel => new
            {
                ViewModel = viewModel,
                ModelProperty = viewModel.GetType().GetProperty(nameof(IModelContainer<object>.Model)),
            })
            .FirstOrDefault(x => x.ModelProperty is not null && x.ModelProperty.PropertyType.IsInstanceOfType(value));

        if (viewModelItem is null)
        {
            return Result.Continue<object?>();
        }

        // Copy Model to ViewModel
        if (viewModelItem.ModelProperty!.GetValue(viewModelItem.ViewModel) is null)
        {
            viewModelItem.ModelProperty.SetValue(viewModelItem.ViewModel, value);
        }

        return Result.Success(viewModelItem.ViewModel);
    }
}
