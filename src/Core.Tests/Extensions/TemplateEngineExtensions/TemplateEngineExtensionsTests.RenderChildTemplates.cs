namespace TemplateFramework.Core.Tests.Extensions;

public partial class TemplateEngineExtensionsTests
{
    public class RenderChildTemplates : TemplateEngineExtensionsTests
    {
        [Fact]
        public void Throws_On_Null_ChildModels_1()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplates(childModels: null!, GenerationEnvironmentMock, Identifier, ContextMock))
                              .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Fact]
        public void Throws_On_Null_Context_1()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock, Identifier, context: null!))
                              .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Renders_All_Items_From_Model_1()
        {
            // Arrange
            ContextMock.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(ContextMock);

            // Act
            TemplateEngineMock.RenderChildTemplates(Models, GenerationEnvironmentMock, Identifier, ContextMock);

            // Assert
            TemplateEngineMock.Received(Models.Count()).Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock
                && request.Model != null
                && request.Identifier == Identifier));
            ContextMock.Received(Models.Count()).CreateChildContext(Arg.Any<IChildTemplateContext>());
        }

        [Fact]
        public void Throws_On_Null_ChildModels_2()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplates(childModels: null!, GenerationEnvironmentMock, _ => Identifier, ContextMock))
                              .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Fact]
        public void Throws_On_Null_Context_2()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock, _ => Identifier, context: null!))
                              .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Renders_All_Items_From_Model_2()
        {
            // Arrange
            ContextMock.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(ContextMock);

            // Act
            TemplateEngineMock.RenderChildTemplates(Models, GenerationEnvironmentMock, _ => Identifier, ContextMock);

            // Assert
            TemplateEngineMock.Received(Models.Count()).Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock
                && request.Model != null
                && request.Identifier == Identifier));
            ContextMock.Received(Models.Count()).CreateChildContext(Arg.Any<IChildTemplateContext>());
        }

        [Fact]
        public void Throws_On_Null_ChildModels_3()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplates(childModels: null!, GenerationEnvironmentMock, Identifier))
                              .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Fact]
        public void Renders_All_Items_From_Model_3()
        {
            // Act
            TemplateEngineMock.RenderChildTemplates(Models, GenerationEnvironmentMock, Identifier);

            // Assert
            TemplateEngineMock.Received(Models.Count()).Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock
                && request.Model != null
                && request.Identifier == Identifier));
        }

        [Fact]
        public void Throws_On_Null_ChildModels_4()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplates(childModels: null!, GenerationEnvironmentMock, _ => Identifier))
                              .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Fact]
        public void Renders_All_Items_From_Model_4()
        {
            // Act
            TemplateEngineMock.RenderChildTemplates(Models, GenerationEnvironmentMock, _ => Identifier);

            // Assert
            TemplateEngineMock.Received(Models.Count()).Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock
                && request.Model != null
                && request.Identifier == Identifier));
        }

        [Fact]
        public void Throws_On_Null_ChildModels_5()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplates(childModels: null!, GenerationEnvironmentMock, ContextMock, _ => TemplateIdentifierMock))
                              .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Fact]
        public void Throws_On_Null_Context_5()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock, context: null!, _ => TemplateIdentifierMock))
                              .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Throws_On_Null_TemplateIdentifierFactory_5()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock, ContextMock, templateIdentifierFactory: null!))
                              .Should().Throw<ArgumentNullException>().WithParameterName("templateIdentifierFactory");
        }

        [Fact]
        public void Renders_All_Items_From_Model_5()
        {
            // Arrange
            ContextMock.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(ContextMock);
            ContextMock.DefaultFilename.Returns(DefaultFilename);
            ContextMock.TemplateComponentRegistry.Returns(TemplateProviderMock);
            TemplateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(Template);

            // Act
            TemplateEngineMock.RenderChildTemplates(Models, GenerationEnvironmentMock, ContextMock, _ => TemplateIdentifierMock);

            // Assert
            TemplateEngineMock.Received(Models.Count()).Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock
                && request.DefaultFilename == ContextMock.DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock
                && request.Model != null
                && request.Identifier == Identifier));
        }

        [Fact]
        public void Throws_On_Null_ChildModels_6()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplates(childModels: null!, GenerationEnvironmentMock, ContextMock, _ => Template))
                              .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Fact]
        public void Throws_On_Null_Context_6()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock, context: null!, _ => Template))
                              .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Throws_On_Null_TemplateIdentifierFactory_6()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock, ContextMock, templateIdentifierFactory: null!))
                              .Should().Throw<ArgumentNullException>().WithParameterName("templateIdentifierFactory");
        }

        [Fact]
        public void Renders_All_Items_From_Model_6()
        {
            // Arrange
            ContextMock.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(ContextMock);
            ContextMock.DefaultFilename.Returns(DefaultFilename);

            // Act
            TemplateEngineMock.RenderChildTemplates(Models, GenerationEnvironmentMock, ContextMock, _ => Template);

            // Assert
            TemplateEngineMock.Received(Models.Count()).Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock
                && request.DefaultFilename == ContextMock.DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock
                && request.Model != null
                && request.Identifier != null));
        }

        [Fact]
        public void Throws_On_Null_ChildModels_7()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplates(childModels: null!, GenerationEnvironmentMock, ContextMock, Identifier))
                              .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Fact]
        public void Throws_On_Null_Context_7()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock, context: null!, Identifier))
                              .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Throws_On_Null_Identifier_7()
        {
            // Act & Assert
            TemplateEngineMock.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock, ContextMock, identifier: null!))
                              .Should().Throw<ArgumentNullException>().WithParameterName("identifier");
        }

        [Fact]
        public void Renders_All_Items_From_Model_7()
        {
            // Arrange
            ContextMock.CreateChildContext(Arg.Any<IChildTemplateContext>()).Returns(ContextMock);
            ContextMock.DefaultFilename.Returns(DefaultFilename);

            // Act
            TemplateEngineMock.RenderChildTemplates(Models, GenerationEnvironmentMock, ContextMock, Identifier);

            // Assert
            TemplateEngineMock.Received(Models.Count()).Render(Arg.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock
                && request.DefaultFilename == ContextMock.DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock
                && request.Model != null
                && request.Identifier == Identifier));
        }
    }
}
