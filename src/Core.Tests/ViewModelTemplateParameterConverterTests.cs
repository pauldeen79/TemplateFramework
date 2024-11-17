namespace TemplateFramework.Core.Tests;

public partial class ViewModelTemplateParameterConverterTests
{
    protected Func<IEnumerable<IViewModel>> Factory { get; }
    protected Collection<IViewModel> ViewModels { get; }
    protected ITemplateEngineContext Context { get; }
    protected Type Type { get; }

    protected ViewModelTemplateParameterConverter CreateSut()
        => new ViewModelTemplateParameterConverter(Factory);

    public ViewModelTemplateParameterConverterTests()
    {
        ViewModels = [];
        Factory = () => ViewModels;
        Context = Substitute.For<ITemplateEngineContext>();
        Type = typeof(object); // Actually, this parameter is not used, but it's part of the interface so we need to provide it
    }
}
