namespace TemplateFramework.Core.Tests.Extensions;

public partial class TemplateEngineExtensionsTests
{
    public class RenderChildTemplate : TemplateEngineExtensionsTests
    {
        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_1()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock, Identifier, context: null!))
                              .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_1()
        {
            // Arrange
            ContextMock.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(ContextMock);

            // Act
            TemplateEngineMock.RenderChildTemplate(Model, GenerationEnvironmentMock, Identifier, ContextMock);

            // Assert
            TemplateEngineMock.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock
                && request.Model == Model
                && request.Identifier == Identifier));
            ContextMock.Received().CreateChildContext(Arg.Any<IChildTemplateContext>());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_2()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock, _ => Identifier, context: null!))
                              .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_IdentifierFactory_2()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock, identifierFactory: null!, ContextMock))
                              .Should().Throw<ArgumentNullException>().WithParameterName("identifierFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_2()
        {
            // Arrange
            ContextMock.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(ContextMock);

            // Act
            TemplateEngineMock.RenderChildTemplate(Model, GenerationEnvironmentMock, _ => Identifier, ContextMock);

            // Assert
            TemplateEngineMock.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock
                && request.Model == Model
                && request.Identifier == Identifier));
            ContextMock.Received().CreateChildContext(Arg.Any<IChildTemplateContext>());
        }
        
        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_3()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock, Identifier, context: null!))
                              .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_3()
        {
            // Arrange
            ContextMock.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(ContextMock);

            // Act
            TemplateEngineMock.RenderChildTemplate(GenerationEnvironmentMock, Identifier, ContextMock);

            // Assert
            TemplateEngineMock.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock
                && request.Model == null
                && request.Identifier == Identifier));
            ContextMock.Received().CreateChildContext(Arg.Any<IChildTemplateContext>());
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_4()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock, () => Identifier, context: null!))
                              .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_IdentifierFactory_4()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock, identifierFactory: null!, ContextMock))
                              .Should().Throw<ArgumentNullException>().WithParameterName("identifierFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_4()
        {
            // Arrange
            ContextMock.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(ContextMock);

            // Act
            TemplateEngineMock.RenderChildTemplate(GenerationEnvironmentMock, () => Identifier, ContextMock);

            // Assert
            TemplateEngineMock.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock
                && request.Model == null
                && request.Identifier == Identifier));
            ContextMock.Received().CreateChildContext(Arg.Any<IChildTemplateContext>());
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_5()
        {
            // Act
            TemplateEngineMock.RenderChildTemplate(Model, GenerationEnvironmentMock, Identifier);

            // Assert
            TemplateEngineMock.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock
                && request.Model == Model
                && request.Identifier == Identifier));
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_IdentifierFactory_6()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock, identifierFactory: null!))
                              .Should().Throw<ArgumentNullException>().WithParameterName("identifierFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_6()
        {
            // Act
            TemplateEngineMock.RenderChildTemplate(Model, GenerationEnvironmentMock, _ => Identifier);

            // Assert
            TemplateEngineMock.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock
                && request.Model == Model
                && request.Identifier == Identifier));
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_7()
        {
            // Act
            TemplateEngineMock.RenderChildTemplate(GenerationEnvironmentMock, Identifier);

            // Assert
            TemplateEngineMock.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock
                && request.Model == null
                && request.Identifier == Identifier));
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_IdentifierFactory_8()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock, identifierFactory: null!))
                              .Should().Throw<ArgumentNullException>().WithParameterName("identifierFactory");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_8()
        {
            // Act
            TemplateEngineMock.RenderChildTemplate(GenerationEnvironmentMock, () => Identifier);

            // Assert
            TemplateEngineMock.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock
                && request.Model == null
                && request.Identifier == Identifier));
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_9()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock, context: null!, TemplateIdentifierMock))
                              .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Identifier_9()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplate(Model, GenerationEnvironmentMock, ContextMock, identifier: null!))
                              .Should().Throw<ArgumentNullException>().WithParameterName("identifier");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_9()
        {
            // Arrange
            ContextMock.DefaultFilename.Returns(DefaultFilename);
            ContextMock.TemplateComponentRegistry.Returns(TemplateProviderMock);
            ContextMock.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(ContextMock);
            TemplateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(Template);

            // Act
            TemplateEngineMock.RenderChildTemplate(Model, GenerationEnvironmentMock, ContextMock, TemplateIdentifierMock);

            // Assert
            TemplateEngineMock.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock
                && request.DefaultFilename == ContextMock.DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock
                && request.Model == Model
                && request.Identifier == Identifier));
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Context_10()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock, context: null!, TemplateIdentifierMock))
                              .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void RenderChildTemplate_Throws_On_Null_Identifier_10()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplate(GenerationEnvironmentMock, ContextMock, identifier: null!))
                              .Should().Throw<ArgumentNullException>().WithParameterName("identifier");
        }

        [Fact]
        public void RenderChildTemplate_Renders_ChildTemplate_Correctly_10()
        {
            // Arrange
            ContextMock.DefaultFilename.Returns(DefaultFilename);
            ContextMock.TemplateComponentRegistry.Returns(TemplateProviderMock);
            ContextMock.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(ContextMock);
            TemplateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(Template);

            // Act
            TemplateEngineMock.RenderChildTemplate(GenerationEnvironmentMock, ContextMock, TemplateIdentifierMock);

            // Assert
            TemplateEngineMock.Received().Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock
                && request.DefaultFilename == ContextMock.DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock
                && request.Model == null
                && request.Identifier == Identifier));
        }
    }
}
