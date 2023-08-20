namespace TemplateFramework.Core.Tests.Extensions;

public partial class TemplateEngineExtensionsTests
{
    public class RenderChildTemplate : TemplateEngineExtensionsTests
    {
        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_1()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, Template, DefaultFilename, AdditionalParameters, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_1()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, Template, DefaultFilename, AdditionalParameters, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == ContextMock.Object
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Template == Template)), Times.Once());
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_2()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, Template, DefaultFilename, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_2()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, Template, DefaultFilename, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Template == Template)), Times.Once());
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_3()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, Template, AdditionalParameters, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_3()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, Template, AdditionalParameters, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == ContextMock.Object
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Template == Template)), Times.Once());
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_4()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, Template, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_4()
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
        public void RenderChildTemplate_Throws_On_Null_Context_5()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, () => Template, DefaultFilename, AdditionalParameters, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_TemplateFactory_5()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, templateFactory: null!, DefaultFilename, AdditionalParameters, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_5()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, _ => Template, DefaultFilename, AdditionalParameters, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == ContextMock.Object
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Template == Template)), Times.Once());
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_6()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, () => Template, DefaultFilename, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_TemplateFactory_6()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, templateFactory: null!, DefaultFilename, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_6()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, _ => Template, DefaultFilename, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Template == Template)), Times.Once());
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_7()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, () => Template, AdditionalParameters, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_TemplateFactory_7()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, templateFactory: null!, AdditionalParameters, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_7()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, _ => Template, AdditionalParameters, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == ContextMock.Object
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Template == Template)), Times.Once());
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_8()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, () => Template, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_TemplateFactory_8()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, templateFactory: null!, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_8()
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
        public void RenderChildTemplate_Throws_On_Null_Context_9()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, Template, DefaultFilename, AdditionalParameters, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_9()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(GenerationEnvironmentMock.Object, Template, DefaultFilename, AdditionalParameters, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == ContextMock.Object
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == null
                && request.Template == Template)), Times.Once());
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_10()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, Template, DefaultFilename, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_10()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(GenerationEnvironmentMock.Object, Template, DefaultFilename, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == null
                && request.Template == Template)), Times.Once());
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_11()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, Template, AdditionalParameters, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_11()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(GenerationEnvironmentMock.Object, Template, AdditionalParameters, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == ContextMock.Object
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == null
                && request.Template == Template)), Times.Once());
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_12()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, Template, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_12()
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
        public void RenderChildTemplate_Throws_On_Null_Context_13()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, () => Template, DefaultFilename, AdditionalParameters, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_TemplateFactory_13()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, templateFactory: null!, DefaultFilename, AdditionalParameters, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_13()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(GenerationEnvironmentMock.Object, () => Template, DefaultFilename, AdditionalParameters, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == ContextMock.Object
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == null
                && request.Template == Template)), Times.Once());
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_14()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, () => Template, DefaultFilename, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_TemplateFactory_14()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, templateFactory: null!, DefaultFilename, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_14()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(GenerationEnvironmentMock.Object, () => Template, DefaultFilename, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == null
                && request.Template == Template)), Times.Once());
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_15()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, () => Template, AdditionalParameters, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_TemplateFactory_15()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, templateFactory: null!, AdditionalParameters, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_15()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(GenerationEnvironmentMock.Object, () => Template, AdditionalParameters, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == ContextMock.Object
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == null
                && request.Template == Template)), Times.Once());
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_16()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, () => Template, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_TemplateFactory_16()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, templateFactory: null!, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_16()
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
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_17()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, Template, DefaultFilename, AdditionalParameters);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == null
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Template == Template)), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_18()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, Template, DefaultFilename);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Template == Template)), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_19()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, Template, AdditionalParameters);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Template == Template)), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_20()
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
        public void RenderChildTemplate_Throws_On_Null_TemplateFactory_21()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, templateFactory: null!, DefaultFilename, AdditionalParameters))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_21()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, _ => Template, DefaultFilename, AdditionalParameters);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == null
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Template == Template)), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_TemplateFactory_22()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, templateFactory: null!, DefaultFilename))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_22()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, _ => Template, DefaultFilename);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Template == Template)), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_TemplateFactory_23()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, templateFactory: null!, AdditionalParameters))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_23()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, _ => Template, AdditionalParameters);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Template == Template)), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_TemplateFactory_24()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, templateFactory: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_24()
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
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_25()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplate(GenerationEnvironmentMock.Object, Template, DefaultFilename, AdditionalParameters);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == null
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == null
                && request.Template == Template)), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_26()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplate(GenerationEnvironmentMock.Object, Template, DefaultFilename);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == null
                && request.Template == Template)), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_27()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplate(GenerationEnvironmentMock.Object, Template, AdditionalParameters);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == null
                && request.Template == Template)), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_28()
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
        public void RenderChildTemplate_Throws_On_Null_TemplateFactory_29()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, templateFactory: null!, DefaultFilename, AdditionalParameters))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_29()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplate(GenerationEnvironmentMock.Object, () => Template, DefaultFilename, AdditionalParameters);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == null
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == null
                && request.Template == Template)), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_TemplateFactory_30()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, templateFactory: null!, DefaultFilename))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_30()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplate(GenerationEnvironmentMock.Object, () => Template, DefaultFilename);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == null
                && request.Template == Template)), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_TemplateFactory_31()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, templateFactory: null!, AdditionalParameters))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_31()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplate(GenerationEnvironmentMock.Object, () => Template, AdditionalParameters);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == null
                && request.Template == Template)), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_TemplateFactory_32()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, templateFactory: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_32()
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
    }
}
