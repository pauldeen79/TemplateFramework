namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider.Tests;

public class IntegrationTests
{
    [Fact]
    public void Can_Process_Template_With_FormattableString_Placeholders()
    {
        // Arrange
        using var provider = new ServiceCollection()
            .AddParsers()
            .AddTemplateFramework()
            .AddTemplateFrameworkFormattableStringTemplateProvider()
            .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
        var templateEngine = provider.GetRequiredService<ITemplateEngine>();
        var formattableStringParser = provider.GetRequiredService<IFormattableStringParser>();
        var builder = new StringBuilder();
        var request = new RenderTemplateRequest(new FormattableStringTemplate(new CreateFormattableStringTemplateRequest("Hello {Name}!", CultureInfo.CurrentCulture), formattableStringParser), builder, new { Name = "world" });

        // Act
        templateEngine.Render(request);

        // Assert
        builder.ToString().Should().Be("Hello world!");
    }

    [Fact]
    public void Can_Get_TemplateParameters_From_FormattableString()
    {
        // Arrange
        using var provider = new ServiceCollection()
            .AddParsers()
            .AddTemplateFramework()
            .AddTemplateFrameworkFormattableStringTemplateProvider()
            .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
        var formattableStringParser = provider.GetRequiredService<IFormattableStringParser>();
        var builder = new StringBuilder();
        var template = new FormattableStringTemplate(new CreateFormattableStringTemplateRequest("Hello {Prefix} {Name}!", CultureInfo.CurrentCulture), formattableStringParser);

        // Act 
        var result = template.GetParameters();

        // Assert
        result.Select(x => x.Name).Should().BeEquivalentTo("Prefix", "Name");
    }
}
