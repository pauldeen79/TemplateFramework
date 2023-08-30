namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider.Tests;

public class TestFormattableStringTemplate : IParameterizedTemplate, IStringBuilderTemplate
{
    private readonly Dictionary<string, object?> _parameterValues = new();

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
    {
        var services = new ServiceCollection()
            .AddParsers()
            .AddTemplateFrameworkFormattableStringTemplateProvider();

        using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
        var formattableStringParser = provider.GetRequiredService<IFormattableStringParser>();

        return new FormattableStringTemplate(new FormattableStringTemplateIdentifier(Template, CultureInfo.CurrentCulture), formattableStringParser).GetParameters();
    }

    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);

        var services = new ServiceCollection()
            .AddParsers()
            .AddTemplateFrameworkFormattableStringTemplateProvider();

        using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

        var formattableStringParser = provider.GetRequiredService<IFormattableStringParser>();
        var context = new TemplateFrameworkFormattableStringContext(_parameterValues);

        builder.Append(formattableStringParser.Parse(Template, CultureInfo.CurrentCulture, context).GetValueOrThrow());
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
