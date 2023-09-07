namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class ProviderPluginInitializerComponentTests
{
    protected ProviderPluginInitializerComponent CreateSut() => new(TemplateProviderPluginFactoryMock.Object);

    protected Mock<ITemplateEngineContext> TemplateEngineContextMock { get; } = new();
    protected Mock<ITemplateComponentRegistryPlugin> TemplateComponentRegistryPluginMock { get; } = new();
    protected Mock<ITemplateContext> TemplateContextMock { get; } = new();
    protected Mock<ITemplateComponentRegistryPluginFactory> TemplateProviderPluginFactoryMock { get; } = new();

    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Factory()
        {
            // Act & Assert
            this.Invoking(_ => new ProviderPluginInitializerComponent(factory: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("factory");
        }
    }

    public class Initialize : ProviderPluginInitializerComponentTests
    {
        [Fact]
        public void Throws_On_Null_Context()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Initialize(context: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Does_Not_Initialize_Plugin_On_Template_When_Context_Context_Is_Null()
        {
            // Arrange
            var sut = CreateSut();
            TemplateEngineContextMock.SetupGet(x => x.Template).Returns(TemplateComponentRegistryPluginMock.Object);

            // Act & Assert
            sut.Invoking(x => x.Initialize(TemplateEngineContextMock.Object))
               .Should().NotThrow();
        }

        [Fact]
        public void Does_Not_Initialize_Plugin_On_Template_When_Context_Template_Is_Not_TemplateProviderPlugin()
        {
            // Arrange
            var sut = CreateSut();
            TemplateEngineContextMock.SetupGet(x => x.Context).Returns(TemplateContextMock.Object);
            TemplateEngineContextMock.SetupGet(x => x.Template).Returns(new object());

            // Act & Assert
            sut.Invoking(x => x.Initialize(TemplateEngineContextMock.Object))
               .Should().NotThrow();
        }

        [Fact]
        public void Initializes_Plugin_On_Template_When_Context_Context_Is_Not_Null_And_Context_Template_Is_TemplateProviderPlugin()
        {
            // Arrange
            var sut = CreateSut();
            TemplateEngineContextMock.SetupGet(x => x.Context).Returns(TemplateContextMock.Object);
            TemplateEngineContextMock.SetupGet(x => x.Template).Returns(TemplateComponentRegistryPluginMock.Object);

            // Act
            sut.Initialize(TemplateEngineContextMock.Object);

            // Assert
            TemplateComponentRegistryPluginMock.Verify(x => x.Initialize(It.IsAny<ITemplateProvider>()), Times.Once);
        }

        [Fact]
        public void Initializes_Plugin_On_Identifier_When_Identifier_Is_TemplateProviderPluginIdentifier()
        {
            // Arrange
            var identifier = new IdentifierWithTemplateProviderPluginIdentifier(GetType().Assembly.FullName, GetType().FullName!, Directory.GetCurrentDirectory());
            var sut = CreateSut();
            TemplateEngineContextMock.SetupGet(x => x.Context).Returns(TemplateContextMock.Object);
            TemplateEngineContextMock.SetupGet(x => x.Template).Returns(new object());
            TemplateEngineContextMock.SetupGet(x => x.Identifier).Returns(identifier);
            TemplateProviderPluginFactoryMock.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(TemplateComponentRegistryPluginMock.Object);

            // Act
            sut.Initialize(TemplateEngineContextMock.Object);

            // Assert
            TemplateComponentRegistryPluginMock.Verify(x => x.Initialize(It.IsAny<ITemplateProvider>()), Times.Once);
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
