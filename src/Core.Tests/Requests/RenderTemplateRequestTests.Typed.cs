namespace TemplateFramework.Core.Tests.Requests;

public partial class RenderTemplateRequestTests
{
    public class Typed : RenderTemplateRequestTests
    {
        [Fact]
        public void Throws_On_Null_StringBuilder_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: (StringBuilder)null!, defaultFilename: DefaultFilename, additionalParameters: null, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: StringBuilder, defaultFilename: DefaultFilename, additionalParameters: null, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: (StringBuilder)null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: StringBuilder))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: (StringBuilder)null!, defaultFilename: DefaultFilename))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: StringBuilder, defaultFilename: DefaultFilename))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: (StringBuilder)null!, defaultFilename: DefaultFilename, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: StringBuilder, defaultFilename: DefaultFilename, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: (StringBuilder)null!, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: StringBuilder, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: (StringBuilder)null!, defaultFilename: DefaultFilename, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: StringBuilder, defaultFilename: DefaultFilename, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: (StringBuilder)null!, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: StringBuilder, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: (IMultipleContentBuilder)null!, defaultFilename: DefaultFilename, additionalParameters: null, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: MultipleContentBuilderMock.Object, defaultFilename: DefaultFilename, additionalParameters: null, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: (IMultipleContentBuilder)null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: MultipleContentBuilderMock.Object))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: (IMultipleContentBuilder)null!, defaultFilename: DefaultFilename))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: MultipleContentBuilderMock.Object, defaultFilename: DefaultFilename))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: (IMultipleContentBuilder)null!, defaultFilename: DefaultFilename, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: MultipleContentBuilderMock.Object, defaultFilename: DefaultFilename, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: (IMultipleContentBuilder)null!, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: MultipleContentBuilderMock.Object, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: (IMultipleContentBuilder)null!, defaultFilename: DefaultFilename, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: MultipleContentBuilderMock.Object, defaultFilename: DefaultFilename, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: (IMultipleContentBuilder)null!, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, builder: MultipleContentBuilderMock.Object, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_Template()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(template: null!, model: null, builder: StringBuilder))
                .Should().Throw<ArgumentNullException>().WithParameterName("template");
        }

        [Fact]
        public void Throws_On_Null_GenerationEnvironment()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, generationEnvironment: default!, defaultFilename: DefaultFilename, additionalParameters: null, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("generationEnvironment");
        }

        [Fact]
        public void Throws_On_Null_DefaultFileName()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, null, StringBuilder, defaultFilename: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("defaultFilename");
        }
    }
}
