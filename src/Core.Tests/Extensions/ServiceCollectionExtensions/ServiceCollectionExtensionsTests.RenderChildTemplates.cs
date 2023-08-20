namespace TemplateFramework.Core.Tests.Extensions;

public partial class ServiceCollectionExtensionsTests
{
    public class RenderChildTemplates : ServiceCollectionExtensionsTests
    {
        [Fact]
        public void Throws_On_Null_Models_1()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(models: null!, GenerationEnvironmentMock.Object, Template, DefaultFilename, AdditionalParameters, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("models");
        }

        [Fact]
        public void Throws_On_Null_Context_1()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, Template, DefaultFilename, AdditionalParameters, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Renders_All_Items_From_Model_1()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, Template, DefaultFilename, AdditionalParameters, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == ContextMock.Object
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Template == Template)), Times.Exactly(Models.Count()));
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_Models_2()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(models: null!, GenerationEnvironmentMock.Object, Template, DefaultFilename, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("models");
        }

        [Fact]
        public void Throws_On_Null_Context_2()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, Template, DefaultFilename, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Renders_All_Items_From_Model_2()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, Template, DefaultFilename, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Template == Template)), Times.Exactly(Models.Count()));
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_Models_3()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(models: null!, GenerationEnvironmentMock.Object, Template, AdditionalParameters, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("models");
        }

        [Fact]
        public void Throws_On_Null_Context_3()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, Template, AdditionalParameters, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Renders_All_Items_From_Model_3()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, Template, AdditionalParameters, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == ContextMock.Object
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Template == Template)), Times.Exactly(Models.Count()));
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_Models_4()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(models: null!, GenerationEnvironmentMock.Object, Template, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("models");
        }

        [Fact]
        public void Throws_On_Null_Context_4()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, Template, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Renders_All_Items_From_Model_4()
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
        public void Throws_On_Null_Models_5()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(models: null!, GenerationEnvironmentMock.Object, _ => Template, DefaultFilename, AdditionalParameters, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("models");
        }

        [Fact]
        public void Throws_On_Null_Context_5()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, _ => Template, DefaultFilename, AdditionalParameters, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Renders_All_Items_From_Model_5()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, _ => Template, DefaultFilename, AdditionalParameters, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == ContextMock.Object
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Template == Template)), Times.Exactly(Models.Count()));
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_Models_6()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(models: null!, GenerationEnvironmentMock.Object, _ => Template, DefaultFilename, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("models");
        }

        [Fact]
        public void Throws_On_Null_Context_6()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, _ => Template, DefaultFilename, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Renders_All_Items_From_Model_6()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, _ => Template, DefaultFilename, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == ContextMock.Object
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Template == Template)), Times.Exactly(Models.Count()));
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_Models_7()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(models: null!, GenerationEnvironmentMock.Object, _ => Template, AdditionalParameters, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("models");
        }

        [Fact]
        public void Throws_On_Null_Context_7()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, _ => Template, AdditionalParameters, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Renders_All_Items_From_Model_7()
        {
            // Arrange
            ContextMock.Setup(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>())).Returns(ContextMock.Object);

            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, _ => Template, AdditionalParameters, ContextMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == ContextMock.Object
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Template == Template)), Times.Exactly(Models.Count()));
            ContextMock.Verify(x => x.CreateChildContext(It.IsAny<IChildTemplateContext>()), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_Models_8()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(models: null!, GenerationEnvironmentMock.Object, _ => Template, ContextMock.Object))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("models");
        }

        [Fact]
        public void Throws_On_Null_Context_8()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, _ => Template, context: null!))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Renders_All_Items_From_Model_8()
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
        public void Throws_On_Null_Models_9()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(models: null!, GenerationEnvironmentMock.Object, Template, DefaultFilename, AdditionalParameters))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("models");
        }

        [Fact]
        public void Renders_All_Items_From_Model_9()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, Template, DefaultFilename, AdditionalParameters);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == null
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Template == Template)), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_Models_10()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(models: null!, GenerationEnvironmentMock.Object, Template, DefaultFilename))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("models");
        }

        [Fact]
        public void Renders_All_Items_From_Model_10()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, Template, DefaultFilename);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Template == Template)), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_Models_11()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(models: null!, GenerationEnvironmentMock.Object, Template, AdditionalParameters))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("models");
        }

        [Fact]
        public void Renders_All_Items_From_Model_11()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, Template, AdditionalParameters);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == null
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Template == Template)), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_Models_12()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(models: null!, GenerationEnvironmentMock.Object, Template))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("models");
        }

        [Fact]
        public void Renders_All_Items_From_Model_12()
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
        public void Throws_On_Null_Models_13()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(models: null!, GenerationEnvironmentMock.Object, _ => Template, DefaultFilename, AdditionalParameters))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("models");
        }

        [Fact]
        public void Renders_All_Items_From_Model_13()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, _ => Template, DefaultFilename, AdditionalParameters);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == null
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Template == Template)), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_Models_14()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(models: null!, GenerationEnvironmentMock.Object, _ => Template, DefaultFilename))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("models");
        }

        [Fact]
        public void Renders_All_Items_From_Model_14()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, _ => Template, DefaultFilename);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == null
                && request.Context == null
                && request.DefaultFilename == DefaultFilename
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Template == Template)), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_Models_15()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(models: null!, GenerationEnvironmentMock.Object, _ => Template, AdditionalParameters))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("models");
        }

        [Fact]
        public void Renders_All_Items_From_Model_15()
        {
            // Act
            TemplateEngineMock.Object.RenderChildTemplates(Models, GenerationEnvironmentMock.Object, _ => Template, AdditionalParameters);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(request =>
                request.AdditionalParameters == AdditionalParameters
                && request.Context == null
                && request.GenerationEnvironment == GenerationEnvironmentMock.Object
                && request.Model != null
                && request.Template == Template)), Times.Exactly(Models.Count()));
        }

        [Fact]
        public void Throws_On_Null_Models_16()
        {
            // Act & Assert
            TemplateEngineMock.Object.Invoking(x => x.RenderChildTemplates(models: null!, GenerationEnvironmentMock.Object, _ => Template))
                                     .Should().Throw<ArgumentNullException>().WithParameterName("models");
        }

        [Fact]
        public void Renders_All_Items_From_Model_16()
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
    }
}
