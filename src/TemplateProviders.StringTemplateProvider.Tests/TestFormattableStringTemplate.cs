namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests;

public class TestFormattableStringTemplate : IParameterizedTemplate, IBuilderTemplate<StringBuilder>
{
    private readonly Dictionary<string, object?> _parameterValues = new();
    private readonly IFormattableStringParser _formattableStringParser;
    private readonly ComponentRegistrationContext _componentRegistrationContext;

    public TestFormattableStringTemplate(IFormattableStringParser formattableStringParser, ComponentRegistrationContext componentRegistrationContext)
    {
        Guard.IsNotNull(formattableStringParser);
        Guard.IsNotNull(componentRegistrationContext);

        _formattableStringParser = formattableStringParser;
        _componentRegistrationContext = componentRegistrationContext;
    }

    const string Template = @"        [Fact]
        public void {UnittestName}()
        {{
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.{MethodName}();

            // Assert
            //TODO
        }}";

    public ITemplateParameter[] GetParameters()
        => new FormattableStringTemplate(new FormattableStringTemplateIdentifier(Template, CultureInfo.CurrentCulture), _formattableStringParser, _componentRegistrationContext).GetParameters();

    public Task<Result> Render(StringBuilder builder, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(builder);

        var context = new TemplateFrameworkStringContext(_parameterValues, _componentRegistrationContext, false);

        var result = _formattableStringParser.Parse(Template, CultureInfo.CurrentCulture, context);

        if (result.IsSuccessful() && result.Value is not null)
        {
            builder.Append(result.Value);
        }
        
        return Task.FromResult((Result)result);
    }

    public void SetParameter(string name, object? value)
    {
        switch (name)
        {
            default:
                _parameterValues.Add(name, value);
                break;
        }
    }
}
