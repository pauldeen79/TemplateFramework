namespace TemplateFramework.Core.Tests;

public class TemplateEngineContextTests
{
    protected IRenderTemplateRequest RequestMock { get; } = Substitute.For<IRenderTemplateRequest>();
    protected ITemplateEngine EngineMock { get; } = Substitute.For<ITemplateEngine>();
    protected ITemplateComponentRegistry ComponentRegistryMock { get; } = Substitute.For<ITemplateComponentRegistry>();
    protected object Template { get; } = new();

    public class Constructor : TemplateEngineContextTests
    {
        [Fact]
        public void Should_Throw_On_Null_Request()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateEngineContext(request: null!, EngineMock, ComponentRegistryMock, Template))
                .Should().Throw<ArgumentNullException>().WithParameterName("request");
        }

        [Fact]
        public void Should_Throw_On_Null_Engine()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateEngineContext(RequestMock, engine: null!, ComponentRegistryMock, Template))
                .Should().Throw<ArgumentNullException>().WithParameterName("engine");
        }

        [Fact]
        public void Should_Throw_On_Null_ComponentRegistry()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateEngineContext(RequestMock, EngineMock, componentRegistry: null!, Template))
                .Should().Throw<ArgumentNullException>().WithParameterName("componentRegistry");
        }

        [Fact]
        public void Should_Throw_On_Null_Template()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateEngineContext(RequestMock, EngineMock, ComponentRegistryMock, template: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("template");
        }

        [Fact]
        public void Sets_Properties_Correctly()
        {
            // Arrange
            var additionalParameters = new object();
            var templateContextMock = Substitute.For<ITemplateContext>();
            var generationEnvironmentMock = Substitute.For<IGenerationEnvironment>();
            var identifierMock = Substitute.For<ITemplateIdentifier>();
            var model = new object();
            RequestMock.AdditionalParameters.Returns(additionalParameters);
            RequestMock.Context.Returns(templateContextMock);
            RequestMock.DefaultFilename.Returns("Filename.txt");
            RequestMock.GenerationEnvironment.Returns(generationEnvironmentMock);
            RequestMock.Identifier.Returns(identifierMock);
            RequestMock.Model.Returns(model);

            // Act
            var instance = new TemplateEngineContext(RequestMock, EngineMock, ComponentRegistryMock, Template);

            // Assert
            instance.AdditionalParameters.Should().BeEquivalentTo(RequestMock.AdditionalParameters);
            instance.ComponentRegistry.Should().BeSameAs(ComponentRegistryMock);
            instance.Context.Should().BeSameAs(RequestMock.Context);
            instance.DefaultFilename.Should().BeSameAs(RequestMock.DefaultFilename);
            instance.Engine.Should().BeSameAs(EngineMock);
            instance.GenerationEnvironment.Should().BeSameAs(RequestMock.GenerationEnvironment);
            instance.Identifier.Should().BeSameAs(RequestMock.Identifier);
            instance.Model.Should().BeSameAs(RequestMock.Model);
            instance.Template.Should().BeSameAs(Template);
        }
    }
}
