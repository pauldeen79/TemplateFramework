namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class ProviderPluginInitializerComponentTests
{
    protected ProviderPluginInitializerComponent CreateSut() => new(TemplateProviderPluginFactoryMock);

    protected ITemplateEngineContext TemplateEngineContextMock { get; } = Substitute.For<ITemplateEngineContext>();
    protected ITemplateComponentRegistryPlugin TemplateComponentRegistryPluginMock { get; } = Substitute.For<ITemplateComponentRegistryPlugin>();
    protected ITemplateContext TemplateContextMock { get; } = Substitute.For<ITemplateContext>();
    protected ITemplateComponentRegistryPluginFactory TemplateProviderPluginFactoryMock { get; } = Substitute.For<ITemplateComponentRegistryPluginFactory>();

    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(ProviderPluginInitializerComponent).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
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
            TemplateEngineContextMock.Template.Returns(TemplateComponentRegistryPluginMock);

            // Act & Assert
            sut.Invoking(x => x.Initialize(TemplateEngineContextMock))
               .Should().NotThrow();
        }

        [Fact]
        public void Does_Not_Initialize_Plugin_On_Template_When_Context_Template_Is_Not_TemplateProviderPlugin()
        {
            // Arrange
            var sut = CreateSut();
            TemplateEngineContextMock.Context.Returns(TemplateContextMock);
            TemplateEngineContextMock.Template.Returns(new object());

            // Act & Assert
            sut.Invoking(x => x.Initialize(TemplateEngineContextMock))
               .Should().NotThrow();
        }

        [Fact]
        public void Initializes_Plugin_On_Template_When_Context_Context_Is_Not_Null_And_Context_Template_Is_TemplateProviderPlugin()
        {
            // Arrange
            var sut = CreateSut();
            TemplateEngineContextMock.Context.Returns(TemplateContextMock);
            TemplateEngineContextMock.Template.Returns(TemplateComponentRegistryPluginMock);

            // Act
            sut.Initialize(TemplateEngineContextMock);

            // Assert
            TemplateComponentRegistryPluginMock.Received().Initialize(Arg.Any<ITemplateComponentRegistry>());
        }

        [Fact]
        public void Initializes_Plugin_On_Identifier_When_Identifier_Is_TemplateProviderPluginIdentifier()
        {
            // Arrange
            var identifier = new IdentifierWithTemplateProviderPluginIdentifier(GetType().Assembly.FullName, GetType().FullName!, Directory.GetCurrentDirectory());
            var sut = CreateSut();
            TemplateEngineContextMock.Context.Returns(TemplateContextMock);
            TemplateEngineContextMock.Template.Returns(new object());
            TemplateEngineContextMock.Identifier.Returns(identifier);
            TemplateProviderPluginFactoryMock.Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(TemplateComponentRegistryPluginMock);

            // Act
            sut.Initialize(TemplateEngineContextMock);

            // Assert
            TemplateComponentRegistryPluginMock.Received().Initialize(Arg.Any<ITemplateComponentRegistry>());
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
