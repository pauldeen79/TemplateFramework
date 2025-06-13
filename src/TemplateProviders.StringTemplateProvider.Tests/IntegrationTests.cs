namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests;

public class IntegrationTests : TestBase
{
    [Fact]
    public async Task Can_Process_Template_With_FormattableString_Placeholders()
    {
        // Arrange
        var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        var services = new ServiceCollection()
            .AddExpressionEvaluator()
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
        var result = await templateEngine.RenderAsync(request, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        builder.ToString().ShouldBe("Hello world!");
    }

    [Fact]
    public async Task Can_Get_TemplateParameters_From_FormattableString()
    {
        // Arrange
        var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        var componentRegistry = Fixture.Freeze<ITemplateComponentRegistry>();
        var services = new ServiceCollection()
            .AddExpressionEvaluator()
            .AddTemplateFramework()
            .AddTemplateFrameworkStringTemplateProvider()
            .AddSingleton(templateComponentRegistryPluginFactory);

        using var provider = services.BuildServiceProvider(true);
        using var scope = provider.CreateScope();
        var expressionEvaluator = scope.ServiceProvider.GetRequiredService<IExpressionEvaluator>();
        var componentRegistrationContext = scope.ServiceProvider.GetRequiredService<ComponentRegistrationContext>();
        var engine = scope.ServiceProvider.GetRequiredService<ITemplateEngine>();
        var template = new FormattableStringTemplate(new FormattableStringTemplateIdentifier("Hello {Prefix} {Name}!", CultureInfo.CurrentCulture), expressionEvaluator, componentRegistrationContext);
        template.Context = new TemplateEngineContext(new RenderTemplateRequest(new TemplateInstanceIdentifier(template), new StringBuilder()), engine, componentRegistry, template);

        // Act 
        var result = await template.GetParametersAsync(CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.GetValueOrThrow().Select(x => x.Name).ToArray().ShouldBeEquivalentTo(new[] { "Name", "Prefix" });
    }

    [Fact]
    public async Task Can_Use_Custom_Registered_PlaceholderProcessor_In_FormattableStringTemplate()
    {
        // Arrange
        var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        var services = new ServiceCollection()
            .AddExpressionEvaluator()
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
        var result = await templateEngine.RenderAsync(request, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        builder.ToString().ShouldBe("aaa Hello world! zzz");
    }

    [Fact]
    public async Task Can_Use_Expression_In_Placeholder_In_FormattableStringTemplate()
    {
        // Arrange
        var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        var services = new ServiceCollection()
            .AddExpressionEvaluator()
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
        await templateEngine.RenderAsync(request, CancellationToken.None);

        // Assert
        builder.ToString().ShouldBe("aaa 2 zzz");
    }

    [Fact]
    public async Task Can_Use_Custom_Registered_Function_In_FormattableStringTemplate()
    {
        // Arrange
        var templateComponentRegistryPluginFactory = Fixture.Freeze<ITemplateComponentRegistryPluginFactory>();
        var services = new ServiceCollection()
            .AddTemplateFrameworkStringTemplateProvider() // important to register this first, to get the functions registered correctly
            .AddExpressionEvaluator()
            .AddTemplateFramework()
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
        var result = await templateEngine.RenderAsync(request, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        builder.ToString().ShouldBe("aaa Hello world! zzz");
    }
}
