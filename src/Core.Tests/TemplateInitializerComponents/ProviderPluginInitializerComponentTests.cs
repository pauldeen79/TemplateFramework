﻿namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class ProviderPluginInitializerComponentTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(ProviderPluginInitializerComponent).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }

    public class Initialize
    {
        [Theory, AutoMockData]
        public async Task Throws_On_Null_Context(ProviderPluginInitializerComponent sut)
        {
            // Act & Assert
            Task t = sut.InitializeAsync(context: null!, CancellationToken.None);
            (await t.ShouldThrowAsync<ArgumentNullException>()).ParamName.ShouldBe("context");
        }

        [Theory, AutoMockData]
        public async Task Does_Not_Initialize_Plugin_On_Template_When_Context_Context_Is_Null(
            [Frozen] ITemplateEngineContext templateEngine,
            [Frozen] ITemplateComponentRegistryPlugin templateComponentRegistryPlugin,
            ProviderPluginInitializerComponent sut)
        {
            // Arrange
            templateEngine.Template.Returns(templateComponentRegistryPlugin);

            // Act & Assert
            Task t = sut.InitializeAsync(templateEngine, CancellationToken.None);
            await t.ShouldNotThrowAsync();
        }

        [Theory, AutoMockData]
        public async Task Does_Not_Initialize_Plugin_On_Template_When_Context_Template_Is_Not_TemplateProviderPlugin(
            [Frozen] ITemplateEngineContext templateEngine,
            [Frozen] ITemplateContext templateContext,
            ProviderPluginInitializerComponent sut)
        {
            // Arrange
            templateEngine.Context.Returns(templateContext);
            templateEngine.Template.Returns(new object());

            // Act & Assert
            Task t = sut.InitializeAsync(templateEngine, CancellationToken.None);
            await t.ShouldNotThrowAsync();
        }

        [Theory, AutoMockData]
        public async Task Initializes_Plugin_On_Template_When_Context_Context_Is_Not_Null_And_Context_Template_Is_TemplateProviderPlugin(
            [Frozen] ITemplateEngineContext templateEngine,
            [Frozen] ITemplateContext templateContext,
            [Frozen] ITemplateComponentRegistryPlugin templateComponentRegistryPlugin,
            ProviderPluginInitializerComponent sut)
        {
            // Arrange
            templateEngine.Context.Returns(templateContext);
            templateEngine.Template.Returns(templateComponentRegistryPlugin);

            // Act
            await sut.InitializeAsync(templateEngine, CancellationToken.None);

            // Assert
            await templateComponentRegistryPlugin.Received().InitializeAsync(Arg.Any<ITemplateComponentRegistry>(), Arg.Any<CancellationToken>());
        }

        [Theory, AutoMockData]
        public async Task Initializes_Plugin_On_Identifier_When_Identifier_Is_TemplateProviderPluginIdentifier(
            [Frozen] ITemplateEngineContext templateEngine,
            [Frozen] ITemplateContext templateContext,
            [Frozen] ITemplateComponentRegistryPluginFactory templateProviderPluginFactory,
            [Frozen] ITemplateComponentRegistryPlugin templateComponentRegistryPlugin,
            ProviderPluginInitializerComponent sut)
        {
            // Arrange
            var identifier = new IdentifierWithTemplateProviderPluginIdentifier(GetType().Assembly.FullName, GetType().FullName!, Directory.GetCurrentDirectory());
            templateEngine.Context.Returns(templateContext);
            templateEngine.Template.Returns(new object());
            templateEngine.Identifier.Returns(identifier);
            templateProviderPluginFactory.Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(templateComponentRegistryPlugin);

            // Act
            await sut.InitializeAsync(templateEngine, CancellationToken.None);

            // Assert
            await templateComponentRegistryPlugin.Received().InitializeAsync(Arg.Any<ITemplateComponentRegistry>(), Arg.Any<CancellationToken>());
        }

        private sealed class IdentifierWithTemplateProviderPluginIdentifier : ITemplateComponentRegistryIdentifier
        {
            public IdentifierWithTemplateProviderPluginIdentifier(string? templateProviderAssemblyName, string? templateProviderClassName, string currentDirectory)
            {
                PluginAssemblyName = templateProviderAssemblyName;
                PluginClassName = templateProviderClassName;
                CurrentDirectory = currentDirectory;
            }

            public string? PluginAssemblyName { get; }
            public string? PluginClassName { get; }
            public string CurrentDirectory { get; }
        }
    }
}
