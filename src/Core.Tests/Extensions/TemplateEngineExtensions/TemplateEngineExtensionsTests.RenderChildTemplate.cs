namespace TemplateFramework.Core.Tests.Extensions;

public partial class TemplateEngineExtensionsTests
{
    public class RenderChildTemplate : TemplateEngineExtensionsTests
    {
        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_1()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, Identifier, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_1()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, Identifier, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Identifier == Identifier)), Times.Once());
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_2()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, _ => Identifier, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_IdentifierFactory_2()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, identifierFactory: null!, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("identifierFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_2()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, _ => Identifier, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Identifier == Identifier)), Times.Once());
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Once());
        }
        
        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_3()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, Identifier, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_3()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(GenerationEnvironmentMock.Object, Identifier, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == null
                && request.Identifier == Identifier)), Times.Once());
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_4()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, () => Identifier, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_IdentifierFactory_4()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, identifierFactory: null!, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("identifierFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_4()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(GenerationEnvironmentMock.Object, () => Identifier, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == null
                && request.Identifier == Identifier)), Times.Once());
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_5()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, Identifier);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Identifier == Identifier)), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_IdentifierFactory_6()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, identifierFactory: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("identifierFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_6()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, _ => Identifier);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Identifier == Identifier)), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_7()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplate(GenerationEnvironmentMock.Object, Identifier);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == null
                && request.Identifier == Identifier)), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_IdentifierFactory_8()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, identifierFactory: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("identifierFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_8()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplate(GenerationEnvironmentMock.Object, () => Identifier);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == null
                && request.Identifier == Identifier)), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_9()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, context: null!, TemplateIdentifierMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Identifier_9()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, ContextMock.Object, identifier: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("identifier");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_9()
        {
            // Arrange
            ContextMock.SetupGet(x => x.DefaultFilename).Returns(DefaultFilename);
            ContextMock.SetupGet(x => x.TemplateComponentRegistry).Returns(TemplateProviderMock.Object);
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);
            TemplateProviderMock.Setup(x => x.Create(It.IsAny<ITemplateIdentifier>())).Returns(Template);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(Model, GenerationEnvironmentMock.Object, ContextMock.Object, TemplateIdentifierMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == ContextMock.Object.DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == Model
                && request.Identifier == Identifier)), Times.Once());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_10()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, context: null!, TemplateIdentifierMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Identifier_10()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock.Object, ContextMock.Object, identifier: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("identifier");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_10()
        {
            // Arrange
            ContextMock.SetupGet(x => x.DefaultFilename).Returns(DefaultFilename);
            ContextMock.SetupGet(x => x.TemplateComponentRegistry).Returns(TemplateProviderMock.Object);
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);
            TemplateProviderMock.Setup(x => x.Create(It.IsAny<ITemplateIdentifier>())).Returns(Template);

            // Act
            TemplateEngineMock.Object.RenderChildTemplate(GenerationEnvironmentMock.Object, ContextMock.Object, TemplateIdentifierMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == ContextMock.Object.DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model == null
                && request.Identifier == Identifier)), Times.Once());
        }
    }
}
