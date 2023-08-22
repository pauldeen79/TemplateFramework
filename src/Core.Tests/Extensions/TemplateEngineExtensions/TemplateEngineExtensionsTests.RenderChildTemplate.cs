namespace TemplateFramework.Core.Tests.Extensions;

public partial class TemplateEngineExtensionsTests
{
    public class RenderChildTemplate : TemplateEngineExtensionsTests
    {
        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_1()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, Template, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_1()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, Template, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Template == Template)), Times.Once());
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_2()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, () => Template, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_TemplateFactory_2()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, templateFactory: null!, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_2()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, _ => Template, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Template == Template)), Times.Once());
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Once());
        }
        
        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_3()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, Template, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_3()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(GenerationEnvironmentMock.Object, Template, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == null
                && request.Template == Template)), Times.Once());
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_4()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, () => Template, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_TemplateFactory_4()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, templateFactory: null!, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_4()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(GenerationEnvironmentMock.Object, () => Template, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == null
                && request.Template == Template)), Times.Once());
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_5()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, Template);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Template == Template)), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_TemplateFactory_6()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, templateFactory: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_6()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, _ => Template);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Template == Template)), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_7()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplate(GenerationEnvironmentMock.Object, Template);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == null
                && request.Template == Template)), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_TemplateFactory_8()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, templateFactory: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_8()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplate(GenerationEnvironmentMock.Object, () => Template);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == null
                && request.Template == Template)), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_9()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, context: null!, CreateTemplateRequestMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_CreateTemplateRequest_9()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, ContextMock.Object, createTemplateRequest: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("createTemplateRequest");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_9()
        {
            // Arrange
            ContextMock.SetupGet(x => x.DefaultFilename).Returns(DefaultFilename);
            ContextMock.SetupGet(x => x.Provider).Returns(TemplateProviderMock.Object);
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);
            TemplateProviderMock.Setup(x => x.Create(It.IsAny<ICreateTemplateRequest>())).Returns(Template);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, ContextMock.Object, CreateTemplateRequestMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == ContextMock.Object.DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Template == Template)), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_10()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, context: null!, CreateTemplateRequestMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_CreateTemplateRequest_10()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, ContextMock.Object, createTemplateRequest: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("createTemplateRequest");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_10()
        {
            // Arrange
            ContextMock.SetupGet(x => x.DefaultFilename).Returns(DefaultFilename);
            ContextMock.SetupGet(x => x.Provider).Returns(TemplateProviderMock.Object);
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);
            TemplateProviderMock.Setup(x => x.Create(It.IsAny<ICreateTemplateRequest>())).Returns(Template);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(GenerationEnvironmentMock.Object, ContextMock.Object, CreateTemplateRequestMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == ContextMock.Object.DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == null
                && request.Template == Template)), Times.Once());
        }
    }
}
