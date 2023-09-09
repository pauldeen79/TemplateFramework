﻿namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests;

public class IntegrationTests
{
    [Fact]
    public void Can_Process_Template_With_FormattableString_Placeholders()
    {
        // Arrange
        var services = new ServiceCollection()
            .AddParsers()
            .AddTemplateFramework()
            .AddTemplateFrameworkStringTemplateProvider()
            .AddSingleton(new Mock<ITemplateComponentRegistryPluginFactory>().Object);

        using var provider = services.BuildServiceProvider(true);
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
            .AddTemplateFrameworkStringTemplateProvider();

        using var provider = services.BuildServiceProvider(true);
        using var scope = provider.CreateScope();
        var formattableStringParser = scope.ServiceProvider.GetRequiredService<IFormattableStringParser>();
        var componentRegistrationContext = scope.ServiceProvider.GetRequiredService<ComponentRegistrationContext>();
        var template = new FormattableStringTemplate(new FormattableStringTemplateIdentifier("Hello {Prefix} {Name}!", CultureInfo.CurrentCulture), formattableStringParser, componentRegistrationContext);

        // Act 
        var result = template.GetParameters();

        // Assert
        result.Select(x => x.Name).Should().BeEquivalentTo("Prefix", "Name");
    }

    [Fact]
    public void Can_Use_Custom_Registered_PlaceholderProcessor()
    {
        // Arrange
        var templateComponentRegistryPluginFactoryMock = new Mock<ITemplateComponentRegistryPluginFactory>();

        var services = new ServiceCollection()
            .AddParsers()
            .AddTemplateFramework()
            .AddTemplateFrameworkStringTemplateProvider()
            .AddSingleton(templateComponentRegistryPluginFactoryMock.Object)
            .AddScoped<ITemplateComponentRegistryPlugin, TestTemplateComponentRegistryPlugin>();

        using var provider = services.BuildServiceProvider(true);
        using var scope = provider.CreateScope();

        templateComponentRegistryPluginFactoryMock
            .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns<string, string, string>((_, className, _)
                => scope.ServiceProvider.GetServices<ITemplateComponentRegistryPlugin>()
                    .OfType<TestTemplateComponentRegistryPlugin>()
                    .FirstOrDefault(x => x.GetType().FullName == className)
                        ?? throw new NotSupportedException($"Unsupported template component registry plug-in type: {className}"));

        var builder = new StringBuilder();
        var templateEngine = scope.ServiceProvider.GetRequiredService<ITemplateEngine>();
        var identifier = new FormattableStringTemplateIdentifier
        (
            "aaa {__test} zzz",
            CultureInfo.CurrentCulture,
            typeof(TestTemplateComponentRegistryPlugin).Assembly.FullName,
            typeof(TestTemplateComponentRegistryPlugin).FullName,
            Directory.GetCurrentDirectory()
        );
        var template = scope.ServiceProvider.GetRequiredService<ITemplateProvider>().Create(identifier);
        var context = new TemplateContext(templateEngine, scope.ServiceProvider.GetRequiredService<ITemplateProvider>(), "myfile.txt", identifier, template);
        var request = new RenderTemplateRequest(identifier, builder, context);

        // Act
        templateEngine.Render(request);

        // Assert
        builder.ToString().Should().Be("aaa Hello world! zzz");
    }

    [Fact]
    public void Can_Use_Expression_In_Placeholder()
    {
        // Arrange
        var templateComponentRegistryPluginFactoryMock = new Mock<ITemplateComponentRegistryPluginFactory>();

        var services = new ServiceCollection()
            .AddParsers()
            .AddTemplateFramework()
            .AddTemplateFrameworkStringTemplateProvider()
            .AddSingleton(templateComponentRegistryPluginFactoryMock.Object);

        using var provider = services.BuildServiceProvider(true);
        using var scope = provider.CreateScope();

        var builder = new StringBuilder();
        var templateEngine = scope.ServiceProvider.GetRequiredService<ITemplateEngine>();
        var identifier = new FormattableStringTemplateIdentifier
        (
            "aaa {1 + 1} zzz",
            CultureInfo.CurrentCulture
        );
        var template = scope.ServiceProvider.GetRequiredService<ITemplateProvider>().Create(identifier);
        var context = new TemplateContext(templateEngine, scope.ServiceProvider.GetRequiredService<ITemplateProvider>(), "myfile.txt", identifier, template);
        var request = new RenderTemplateRequest(identifier, builder, context);

        // Act
        templateEngine.Render(request);

        // Assert
        builder.ToString().Should().Be("aaa 2 zzz");
    }

    [Fact]
    public void Can_Use_Custom_Registered_FunctionResultParser()
    {
        // Arrange
        var templateComponentRegistryPluginFactoryMock = new Mock<ITemplateComponentRegistryPluginFactory>();

        var services = new ServiceCollection()
            .AddParsers()
            .AddTemplateFramework()
            .AddTemplateFrameworkStringTemplateProvider()
            .AddSingleton(templateComponentRegistryPluginFactoryMock.Object)
            .AddScoped<ITemplateComponentRegistryPlugin, TestTemplateComponentRegistryPlugin>();

        using var provider = services.BuildServiceProvider(true);
        using var scope = provider.CreateScope();

        templateComponentRegistryPluginFactoryMock
            .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns<string, string, string>((_, className, _)
                => scope.ServiceProvider.GetServices<ITemplateComponentRegistryPlugin>()
                    .OfType<TestTemplateComponentRegistryPlugin>()
                    .FirstOrDefault(x => x.GetType().FullName == className)
                        ?? throw new NotSupportedException($"Unsupported template component registry plug-in type: {className}"));

        var builder = new StringBuilder();
        var templateEngine = scope.ServiceProvider.GetRequiredService<ITemplateEngine>();
        var identifier = new ExpressionStringTemplateIdentifier
        (
            "=\"aaa \" & MyFunction() & \" zzz\"",
            CultureInfo.CurrentCulture,
            typeof(TestTemplateComponentRegistryPlugin).Assembly.FullName,
            typeof(TestTemplateComponentRegistryPlugin).FullName,
            Directory.GetCurrentDirectory()
        );
        var template = scope.ServiceProvider.GetRequiredService<ITemplateProvider>().Create(identifier);
        var context = new TemplateContext(templateEngine, scope.ServiceProvider.GetRequiredService<ITemplateProvider>(), "myfile.txt", identifier, template);
        var request = new RenderTemplateRequest(identifier, builder, context);

        // Act
        templateEngine.Render(request);

        // Assert
        builder.ToString().Should().Be("aaa Hello world! zzz");
    }
}
