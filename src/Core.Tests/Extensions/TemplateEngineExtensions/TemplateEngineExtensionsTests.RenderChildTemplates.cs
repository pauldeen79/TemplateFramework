namespace TemplateFramework.Core.Tests.Extensions;

public partial class TemplateEngineExtensionsTests
{
    public class RenderChildTemplates : TemplateEngineExtensionsTests
    {
        [Fact]
        public void Throws_On_Null_ChildModels_1()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(childModels: null!, GenerationEnvironmentMock.Object, Template, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Fact]
        public void Throws_On_Null_Context_1()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, Template, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Renders_All_Items_From_Model_1()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, Template, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Template == Template)), Times.Exactly(Models.Count()));
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_ChildModels_2()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(childModels: null!, GenerationEnvironmentMock.Object, _ => Template, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Fact]
        public void Throws_On_Null_Context_2()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, _ => Template, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Renders_All_Items_From_Model_2()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, _ => Template, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Template == Template)), Times.Exactly(Models.Count()));
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_ChildModels_3()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(childModels: null!, GenerationEnvironmentMock.Object, Template))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Fact]
        public void Renders_All_Items_From_Model_3()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, Template);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Template == Template)), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_ChildModels_4()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(childModels: null!, GenerationEnvironmentMock.Object, _ => Template))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Fact]
        public void Renders_All_Items_From_Model_4()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, _ => Template);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == string.Empty
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Template == Template)), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_ChildModels_5()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(childModels: null!, GenerationEnvironmentMock.Object, ContextMock.Object, _ => CreateTemplateRequestMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Fact]
        public void Throws_On_Null_Context_5()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, context: null!, _ => CreateTemplateRequestMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Throws_On_Null_CreateTemplateRequestFactory_5()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, ContextMock.Object, createTemplateRequestFactory: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("createTemplateRequestFactory");
        }

        [Fact]
        public void Renders_All_Items_From_Model_5()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);
            ContextMock.SetupGet(x => x.DefaultFilename).Returns(DefaultFilename);
            ContextMock.SetupGet(x => x.Provider).Returns(TemplateProviderMock.Object);
            TemplateProviderMock.Setup(x => x.Create(It.IsAny<ITemplateIdentifier>())).Returns(Template);

            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, ContextMock.Object, _ => CreateTemplateRequestMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == ContextMock.Object.DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Template == Template)), Times.Exactly(Models.Count()));
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
        public void Throws_On_Null_CreateTemplateFactory_6()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, ContextMock.Object, createTemplateFactory: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("createTemplateFactory");
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
                && request.Template == Template)), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_ChildModels_7()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(childModels: null!, GenerationEnvironmentMock.Object, ContextMock.Object, Template))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("childModels");
        }

        [Fact]
        public void Throws_On_Null_Context_7()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, context: null!, Template))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Throws_On_Null_Template_7()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, ContextMock.Object, template: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("template");
        }

        [Fact]
        public void Renders_All_Items_From_Model_7()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);
            ContextMock.SetupGet(x => x.DefaultFilename).Returns(DefaultFilename);

            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, ContextMock.Object, Template);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == ContextMock.Object.DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Template == Template)), Times.Exactly(Models.Count()));
        }
    }
}
