namespace TemplateFramework.Core.Tests.Requests;

public partial class RenderTemplateRequestTests
{
    public class UnTyped : RenderTemplateRequestTests
    {
        [Fact]
        public void Throws_On_Null_StringBuilder_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, DefaultFilename, null, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_1()
        {
            // Act
            this.Invoking(_ => new RenderTemplateRequest(this, builder: StringBuilder, DefaultFilename, null, null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_2()
        {
            // Act
            this.Invoking(_ => new RenderTemplateRequest(this, builder: StringBuilder))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, DefaultFilename))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_3()
        {
            // Act
            this.Invoking(_ => new RenderTemplateRequest(this, builder: StringBuilder, DefaultFilename))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, DefaultFilename, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_4()
        {
            // Act
            this.Invoking(_ => new RenderTemplateRequest(this, builder: StringBuilder, DefaultFilename, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_5()
        {
            // Act
            this.Invoking(_ => new RenderTemplateRequest(this, builder: StringBuilder, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, DefaultFilename, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_6()
        {
            // Act
            this.Invoking(_ => new RenderTemplateRequest(this, builder: StringBuilder, DefaultFilename, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_7()
        {
            // Act
            this.Invoking(_ => new RenderTemplateRequest(this, builder: StringBuilder, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, DefaultFilename, null, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: MultipleContentBuilderMock.Object, DefaultFilename, null, null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: MultipleContentBuilderMock.Object))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, DefaultFilename))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: MultipleContentBuilderMock.Object, DefaultFilename))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, DefaultFilename, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: MultipleContentBuilderMock.Object, DefaultFilename, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: MultipleContentBuilderMock.Object, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, DefaultFilename, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: MultipleContentBuilderMock.Object, DefaultFilename, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: MultipleContentBuilderMock.Object, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!, DefaultFilename, null, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilderContainer_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: MultipleContentBuilderContainerMock.Object, DefaultFilename, null, null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilderContainer_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: MultipleContentBuilderContainerMock.Object))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!, DefaultFilename))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilderContainer_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: MultipleContentBuilderContainerMock.Object, DefaultFilename))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!, DefaultFilename, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilderContainer_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: MultipleContentBuilderContainerMock.Object, DefaultFilename, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilderContainer_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: MultipleContentBuilderContainerMock.Object, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!, DefaultFilename, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilderContainer_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: MultipleContentBuilderContainerMock.Object, DefaultFilename, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilderContainer_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: MultipleContentBuilderContainerMock.Object, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_Template()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(template: null!, StringBuilder))
                .Should().Throw<ArgumentNullException>().WithParameterName("template");
        }

        [Fact]
        public void Throws_On_Null_GenerationEnvironment()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, generationEnvironment: default!, DefaultFilename, null, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("generationEnvironment");
        }

        [Fact]
        public void Throws_On_Null_DefaultFileName()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, StringBuilder, defaultFilename: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("defaultFilename");
        }
    }
}
