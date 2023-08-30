﻿namespace TemplateFramework.TemplateProviders.FormattableStringTemplateProvider.Tests;

public class IntegrationTests
{
    [Fact]
    public void Can_Process_Template_With_FormattableString_Placeholders()
    {
        // Arrange
        var services = new ServiceCollection()
            .AddParsers()
            .AddTemplateFramework()
            .AddTemplateFrameworkFormattableStringTemplateProvider();

        using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
        using var scope = provider.CreateScope();
        var templateProvider = scope.ServiceProvider.GetRequiredService<ITemplateProvider>();
        var template = templateProvider.Create(new FormattableStringTemplateIdentifier("Hello {Name}!", CultureInfo.CurrentCulture));
        var templateEngine = scope.ServiceProvider.GetRequiredService<ITemplateEngine>();
        var builder = new StringBuilder();
        var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), builder, new { Name = "world" });

        // Act
        templateEngine.Render(request);

        // Assert
        builder.ToString().Should().Be("Hello world!");
    }

    [Fact]
    public void Can_Get_TemplateParameters_From_FormattableString()
    {
        // Arrange
        var services = new ServiceCollection()
            .AddParsers()
            .AddTemplateFramework()
            .AddTemplateFrameworkFormattableStringTemplateProvider();

        using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
        using var scope = provider.CreateScope();
        var formattableStringParser = scope.ServiceProvider.GetRequiredService<IFormattableStringParser>();
        var template = new FormattableStringTemplate(new FormattableStringTemplateIdentifier("Hello {Prefix} {Name}!", CultureInfo.CurrentCulture), formattableStringParser);

        // Act 
        var result = template.GetParameters();

        // Assert
        result.Select(x => x.Name).Should().BeEquivalentTo("Prefix", "Name");
    }
}
