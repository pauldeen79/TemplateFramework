namespace TemplateFramework.Core.Tests.Extensions;

public partial class TemplateEngineExtensionsTests
{
    public class RenderChildTemplates : TemplateEngineExtensionsTests
    {
        [Fact]
        public void Throws_On_Null_ChildModels_1()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(childModels: null!, GenerationEnvironmentMock.Object, Identifier, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Fact]
        public void Throws_On_Null_Context_1()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, Identifier, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Renders_All_Items_From_Model_1()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, Identifier, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Identifier == Identifier)), Times.Exactly(Models.Count()));
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_ChildModels_2()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(childModels: null!, GenerationEnvironmentMock.Object, _ => Identifier, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Fact]
        public void Throws_On_Null_Context_2()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, _ => Identifier, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Renders_All_Items_From_Model_2()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, _ => Identifier, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Identifier == Identifier)), Times.Exactly(Models.Count()));
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_ChildModels_3()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(childModels: null!, GenerationEnvironmentMock.Object, Identifier))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Fact]
        public void Renders_All_Items_From_Model_3()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, Identifier);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Identifier == Identifier)), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_ChildModels_4()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(childModels: null!, GenerationEnvironmentMock.Object, _ => Identifier))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Fact]
        public void Renders_All_Items_From_Model_4()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, _ => Identifier);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Identifier == Identifier)), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_ChildModels_5()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(childModels: null!, GenerationEnvironmentMock.Object, ContextMock.Object, _ => TemplateIdentifierMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Fact]
        public void Throws_On_Null_Context_5()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, context: null!, _ => TemplateIdentifierMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Throws_On_Null_TemplateIdentifierFactory_5()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, ContextMock.Object, templateIdentifierFactory: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("templateIdentifierFactory");
        }

        [Fact]
        public void Renders_All_Items_From_Model_5()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);
            ContextMock.SetupGet(x => x.DefaultFilename).Returns(DefaultFilename);
            ContextMock.SetupGet(x => x.TemplateComponentRegistry).Returns(TemplateProviderMock.Object);
            TemplateProviderMock.Setup(x => x.Create(It.IsAny<ITemplateIdentifier>())).Returns(Template);

            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, ContextMock.Object, _ => TemplateIdentifierMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == ContextMock.Object.DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Identifier == Identifier)), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_ChildModels_6()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(childModels: null!, GenerationEnvironmentMock.Object, ContextMock.Object, _ => Template))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Fact]
        public void Throws_On_Null_Context_6()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, context: null!, _ => Template))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Throws_On_Null_TemplateIdentifierFactory_6()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, ContextMock.Object, templateIdentifierFactory: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("templateIdentifierFactory");
        }

        [Fact]
        public void Renders_All_Items_From_Model_6()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);
            ContextMock.SetupGet(x => x.DefaultFilename).Returns(DefaultFilename);

            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, ContextMock.Object, _ => Template);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == ContextMock.Object.DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Identifier != null)), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_ChildModels_7()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(childModels: null!, GenerationEnvironmentMock.Object, ContextMock.Object, Identifier))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Fact]
        public void Throws_On_Null_Context_7()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, context: null!, Identifier))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Throws_On_Null_Identifier_7()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, ContextMock.Object, identifier: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("identifier");
        }

        [Fact]
        public void Renders_All_Items_From_Model_7()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);
            ContextMock.SetupGet(x => x.DefaultFilename).Returns(DefaultFilename);

            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, ContextMock.Object, Identifier);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == ContextMock.Object.DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Identifier == Identifier)), Times.Exactly(Models.Count()));
        }
    }
}
