namespace TemplateFramework.Core.Tests.Requests;

public partial class RenderTemplateRequestTests
{
    public class Typed : RenderTemplateRequestTests
    {
        [Fact]
        public void Throws_On_Null_StringBuilder_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (StringBuilder)null!, null, DefaultFilename, null, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: StringBuilder, null, DefaultFilename, null, null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (StringBuilder)null!, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: StringBuilder, null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (StringBuilder)null!, null, DefaultFilename))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: StringBuilder, null, DefaultFilename))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (StringBuilder)null!, null, DefaultFilename, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: StringBuilder, null, DefaultFilename, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (StringBuilder)null!, null, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: StringBuilder, null, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (StringBuilder)null!, null, DefaultFilename, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: StringBuilder, null, DefaultFilename, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (StringBuilder)null!, null, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: StringBuilder, null, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (IMultipleContentBuilder)null!, null, DefaultFilename, null, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: MultipleContentBuilderMock.Object, null, DefaultFilename, null, null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (IMultipleContentBuilder)null!, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: MultipleContentBuilderMock.Object, null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (IMultipleContentBuilder)null!, null, DefaultFilename))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: MultipleContentBuilderMock.Object, null, DefaultFilename))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (IMultipleContentBuilder)null!, null, DefaultFilename, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: MultipleContentBuilderMock.Object, null, DefaultFilename, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (IMultipleContentBuilder)null!, null, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: MultipleContentBuilderMock.Object, null, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (IMultipleContentBuilder)null!, null, DefaultFilename, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: MultipleContentBuilderMock.Object, null, DefaultFilename, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (IMultipleContentBuilder)null!, null, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: MultipleContentBuilderMock.Object, null, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_Template()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(template: null!, StringBuilder, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("template");
        }

        [Fact]
        public void Throws_On_Null_GenerationEnvironment()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, generationEnvironment: default!, null, DefaultFilename, null, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("generationEnvironment");
        }

        [Fact]
        public void Throws_On_Null_DefaultFileName()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, StringBuilder, null, defaultFilename: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("defaultFilename");
        }

        [Fact]
        public void Throws_On_Null_Request()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(request: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("request");
        }

        [Fact]
        public void Constructs_Using_Request()
        {
            // Arrange
            var requestMock = new Mock<IRenderTemplateRequest>();
            requestMock.SetupGet(x => x.DefaultFilename).Returns(DefaultFilename);
            requestMock.SetupGet(x => x.GenerationEnvironment).Returns(GenerationEnvironmentMock.Object);
            requestMock.SetupGet(x => x.Template).Returns(this);

            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(request: requestMock.Object))
                .Should().NotThrow();
        }
    }
}
