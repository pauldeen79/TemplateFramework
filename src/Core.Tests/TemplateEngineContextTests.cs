namespace TemplateFramework.Core.Tests;

public class TemplateEngineContextTests
{
    protected Mock<IRenderTemplateRequest> RequestMock { get; } = new();
    protected Mock<ITemplateEngine> EngineMock { get; } = new();
    protected Mock<ITemplateComponentRegistry> ComponentRegistryMock { get; } = new();
    protected object Template { get; } = new();

    public class Constructor : TemplateEngineContextTests
    {
        [Fact]
        public void Should_Throw_On_Null_Request()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateEngineContext(request: null!, EngineMock.Object, ComponentRegistryMock.Object, Template))
                .Should().Throw<ArgumentNullException>().WithParameterName("request");
        }

        [Fact]
        public void Should_Throw_On_Null_Engine()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateEngineContext(RequestMock.Object, engine: null!, ComponentRegistryMock.Object, Template))
                .Should().Throw<ArgumentNullException>().WithParameterName("engine");
        }

        [Fact]
        public void Should_Throw_On_Null_ComponentRegistry()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateEngineContext(RequestMock.Object, EngineMock.Object, componentRegistry: null!, Template))
                .Should().Throw<ArgumentNullException>().WithParameterName("componentRegistry");
        }

        [Fact]
        public void Should_Throw_On_Null_Template()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateEngineContext(RequestMock.Object, EngineMock.Object, ComponentRegistryMock.Object, template: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("template");
        }

        [Fact]
        public void Sets_Properties_Correctly()
        {
            // Arrange
            var additionalParameters = new object();
            var templateContextMock = new Mock<ITemplateContext>();
            var generationEnvironmentMock = new Mock<IGenerationEnvironment>();
            var identifierMock = new Mock<ITemplateIdentifier>();
            var model = new object();
            RequestMock.SetupGet(x => x.AdditionalParameters).Returns(additionalParameters);
            RequestMock.SetupGet(x => x.Context).Returns(templateContextMock.Object);
            RequestMock.SetupGet(x => x.DefaultFilename).Returns("Filename.txt");
            RequestMock.SetupGet(x => x.GenerationEnvironment).Returns(generationEnvironmentMock.Object);
            RequestMock.SetupGet(x => x.Identifier).Returns(identifierMock.Object);
            RequestMock.SetupGet(x => x.Model).Returns(model);

            // Act
            var instance = new TemplateEngineContext(RequestMock.Object, EngineMock.Object, ComponentRegistryMock.Object, Template);

            // Assert
            instance.AdditionalParameters.Should().BeEquivalentTo(RequestMock.Object.AdditionalParameters);
            instance.ComponentRegistry.Should().BeSameAs(ComponentRegistryMock.Object);
            instance.Context.Should().BeSameAs(RequestMock.Object.Context);
            instance.DefaultFilename.Should().BeSameAs(RequestMock.Object.DefaultFilename);
            instance.Engine.Should().BeSameAs(EngineMock.Object);
            instance.GenerationEnvironment.Should().BeSameAs(RequestMock.Object.GenerationEnvironment);
            instance.Identifier.Should().BeSameAs(RequestMock.Object.Identifier);
            instance.Model.Should().BeSameAs(RequestMock.Object.Model);
            instance.Template.Should().BeSameAs(Template);
        }
    }
}
