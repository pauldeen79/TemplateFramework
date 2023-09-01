namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider.Tests;

public class TestFormattableStringTemplate : IParameterizedTemplate, IStringBuilderTemplate
{
    private readonly Dictionary<string, object?> _parameterValues = new();
    private readonly IFormattableStringParser _formattableStringParser;

    public TestFormattableStringTemplate(IFormattableStringParser formattableStringParser)
    {
        Guard.IsNotNull(formattableStringParser);

        _formattableStringParser = formattableStringParser;
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
        => new FormattableStringTemplate(new FormattableStringTemplateIdentifier(Template, CultureInfo.CurrentCulture), _formattableStringParser).GetParameters();

    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);

        var context = new TemplateFrameworkFormattableStringContext(_parameterValues);

        builder.Append(_formattableStringParser.Parse(Template, CultureInfo.CurrentCulture, context).GetValueOrThrow());
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
