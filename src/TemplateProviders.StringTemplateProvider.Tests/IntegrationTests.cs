namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests;

public class IntegrationTests : TestBase
{
    [Fact]
    public async Task Can_Process_Template_With_FormattableString_Placeholders()
    {
        // Arrange
        var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        var services = new ServiceCollection()
            .AddParsers()
            .AddTemplateFramework()
            .AddTemplateFrameworkStringTemplateProvider()
            .AddSingleton(templateComponentRegistryPluginFactory);

        using var provider = services.BuildServiceProvider(true);
        using var scope = provider.CreateScope();
        var templateProvider = scope.ServiceProvider.GetRequiredService<ITemplateProvider>();
        var template = templateProvider.Create(new FormattableStringTemplateIdentifier("Hello {Name}!", CultureInfo.CurrentCulture));
        var templateEngine = scope.ServiceProvider.GetRequiredService<ITemplateEngine>();
        var builder = new StringBuilder();
        var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), builder, new { Name = "world" });

        // Act
        await templateEngine.Render(request, CancellationToken.None);

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
    public async Task Can_Use_Custom_Registered_PlaceholderProcessor_In_FormattableStringTemplate()
    {
        // Arrange
        var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        var services = new ServiceCollection()
            .AddParsers()
            .AddTemplateFramework()
            .AddTemplateFrameworkStringTemplateProvider()
            .AddSingleton(templateComponentRegistryPluginFactory)
            .AddScoped<ITemplateComponentRegistryPlugin, TestTemplateComponentRegistryPlugin>();

        using var provider = services.BuildServiceProvider(true);
        using var scope = provider.CreateScope();

        templateComponentRegistryPluginFactory.Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(args
                => scope.ServiceProvider.GetServices<ITemplateComponentRegistryPlugin>()
                    .OfType<TestTemplateComponentRegistryPlugin>()
                    .FirstOrDefault(x => x.GetType().FullName == args.ArgAt<string>(1))
                        ?? throw new NotSupportedException($"Unsupported template component registry plug-in type: {args.ArgAt<string>(1)}"));

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
        await templateEngine.Render(request, CancellationToken.None);

        // Assert
        builder.ToString().Should().Be("aaa Hello world! zzz");
    }

    [Fact]
    public async Task Can_Use_Expression_In_Placeholder_In_FormattableStringTemplate()
    {
        // Arrange
        var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        var services = new ServiceCollection()
            .AddParsers()
            .AddTemplateFramework()
            .AddTemplateFrameworkStringTemplateProvider()
            .AddSingleton(templateComponentRegistryPluginFactory);

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
        await templateEngine.Render(request, CancellationToken.None);

        // Assert
        builder.ToString().Should().Be("aaa 2 zzz");
    }

    [Fact]
    public async Task Can_Use_Custom_Registered_FunctionResultParser_In_FormattableStringTemplate()
    {
        // Arrange
        var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        var services = new ServiceCollection()
            .AddParsers()
            .AddTemplateFramework()
            .AddTemplateFrameworkStringTemplateProvider()
            .AddSingleton(templateComponentRegistryPluginFactory)
            .AddScoped<ITemplateComponentRegistryPlugin, TestTemplateComponentRegistryPlugin>();

        using var provider = services.BuildServiceProvider(true);
        using var scope = provider.CreateScope();

        templateComponentRegistryPluginFactory.Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(args
                => scope.ServiceProvider.GetServices<ITemplateComponentRegistryPlugin>()
                    .OfType<TestTemplateComponentRegistryPlugin>()
                    .FirstOrDefault(x => x.GetType().FullName == args.ArgAt<string>(1))
                        ?? throw new NotSupportedException($"Unsupported template component registry plug-in type: {args.ArgAt<string>(1)}"));

        var builder = new StringBuilder();
        var templateEngine = scope.ServiceProvider.GetRequiredService<ITemplateEngine>();
        var identifier = new FormattableStringTemplateIdentifier
        (
            "aaa {MyFunction()} zzz",
            CultureInfo.CurrentCulture,
            typeof(TestTemplateComponentRegistryPlugin).Assembly.FullName,
            typeof(TestTemplateComponentRegistryPlugin).FullName,
            Directory.GetCurrentDirectory()
        );
        var template = scope.ServiceProvider.GetRequiredService<ITemplateProvider>().Create(identifier);
        var context = new TemplateContext(templateEngine, scope.ServiceProvider.GetRequiredService<ITemplateProvider>(), "myfile.txt", identifier, template);
        var request = new RenderTemplateRequest(identifier, builder, context);

        // Act
        await templateEngine.Render(request, CancellationToken.None);

        // Assert
        builder.ToString().Should().Be("aaa Hello world! zzz");
    }

    [Fact]
    public async Task Can_Use_Custom_Registered_FunctionResultParser_In_ExpressionStringTemplate()
    {
        // Arrange
        var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        var services = new ServiceCollection()
            .AddParsers()
            .AddTemplateFramework()
            .AddTemplateFrameworkStringTemplateProvider()
            .AddSingleton(templateComponentRegistryPluginFactory)
            .AddScoped<ITemplateComponentRegistryPlugin, TestTemplateComponentRegistryPlugin>();

        using var provider = services.BuildServiceProvider(true);
        using var scope = provider.CreateScope();

        templateComponentRegistryPluginFactory.Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(args
                => scope.ServiceProvider.GetServices<ITemplateComponentRegistryPlugin>()
                    .OfType<TestTemplateComponentRegistryPlugin>()
                    .FirstOrDefault(x => x.GetType().FullName == args.ArgAt<string>(1))
                        ?? throw new NotSupportedException($"Unsupported template component registry plug-in type: {args.ArgAt<string>(1)}"));

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
        await templateEngine.Render(request, CancellationToken.None);

        // Assert
        builder.ToString().Should().Be("aaa Hello world! zzz");
    }

    [Fact]
    public async Task Can_Use_ExpressionFramework_In_FormattableStringTemplate()
    {
        // Arrange
        var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        var services = new ServiceCollection()
            .AddParsers()
            .AddTemplateFramework()
            .AddTemplateFrameworkStringTemplateProvider()
            .AddSingleton(templateComponentRegistryPluginFactory)
            .AddExpressionParser();

        using var provider = services.BuildServiceProvider(true);
        using var scope = provider.CreateScope();

        var builder = new StringBuilder();
        var templateEngine = scope.ServiceProvider.GetRequiredService<ITemplateEngine>();
        var identifier = new FormattableStringTemplateIdentifier
        (
            "{ToUpperCase(\"aaa\")} {Today()} {ToUpperCase(\"zzz\")}",
            CultureInfo.InvariantCulture // important to make this test pass on all cultures!
        );
        var template = scope.ServiceProvider.GetRequiredService<ITemplateProvider>().Create(identifier);
        var context = new TemplateContext(templateEngine, scope.ServiceProvider.GetRequiredService<ITemplateProvider>(), "myfile.txt", identifier, template);
        var request = new RenderTemplateRequest(identifier, builder, context);

        // Act
        await templateEngine.Render(request, CancellationToken.None);

        // Assert
        builder.ToString().Should().Be($"AAA {DateTime.Today.ToString(CultureInfo.InvariantCulture)} ZZZ"); // CultureInfo.InvariantCulture is important to make this test pass on all cultures!
    }

    [Fact]
    public async Task Can_Use_ExpressionFramework_In_ExpressionStringTemplate()
    {
        // Arrange
        var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        var services = new ServiceCollection()
            .AddParsers()
            .AddTemplateFramework()
            .AddTemplateFrameworkStringTemplateProvider()
            .AddSingleton(templateComponentRegistryPluginFactory)
            .AddExpressionParser();

        using var provider = services.BuildServiceProvider(true);
        using var scope = provider.CreateScope();

        var builder = new StringBuilder();
        var templateEngine = scope.ServiceProvider.GetRequiredService<ITemplateEngine>();
        var identifier = new ExpressionStringTemplateIdentifier
        (
            "=ToUpperCase(\"aaa \") & Today() & ToUpperCase(\" zzz\")",
            CultureInfo.InvariantCulture // important to make this test pass on all cultures!
        );
        var template = scope.ServiceProvider.GetRequiredService<ITemplateProvider>().Create(identifier);
        var context = new TemplateContext(templateEngine, scope.ServiceProvider.GetRequiredService<ITemplateProvider>(), "myfile.txt", identifier, template);
        var request = new RenderTemplateRequest(identifier, builder, context);

        // Act
        await templateEngine.Render(request, CancellationToken.None);

        // Assert
        builder.ToString().Should().Be($"AAA {DateTime.Today.ToString(CultureInfo.InvariantCulture)} ZZZ"); // CultureInfo.InvariantCulture is important to make this test pass on all cultures!
    }
}
