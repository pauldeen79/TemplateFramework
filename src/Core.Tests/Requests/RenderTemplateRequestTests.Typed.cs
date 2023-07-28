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
        public void Throws_On_Null_StringBuilder_8()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, defaultFilename: DefaultFilename, additionalParameters: null, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_8()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: StringBuilder, defaultFilename: DefaultFilename, additionalParameters: null, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_9()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_9()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: StringBuilder))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_10()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, defaultFilename: DefaultFilename))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_10()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: StringBuilder, defaultFilename: DefaultFilename))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_11()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, defaultFilename: DefaultFilename, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_11()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: StringBuilder, defaultFilename: DefaultFilename, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_12()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_12()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: StringBuilder, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_13()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, defaultFilename: DefaultFilename, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_13()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: StringBuilder, defaultFilename: DefaultFilename, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_14()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_14()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: StringBuilder, context: null))
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
        public void Throws_On_Null_MultipleContentBuilder_8()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, defaultFilename: DefaultFilename, additionalParameters: null, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_8()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: MultipleContentBuilderMock.Object, defaultFilename: DefaultFilename, additionalParameters: null, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_9()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_9()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: MultipleContentBuilderMock.Object))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_10()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, defaultFilename: DefaultFilename))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_10()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: MultipleContentBuilderMock.Object, defaultFilename: DefaultFilename))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_11()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, defaultFilename: DefaultFilename, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_11()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: MultipleContentBuilderMock.Object, defaultFilename: DefaultFilename, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_12()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_12()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: MultipleContentBuilderMock.Object, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_13()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, defaultFilename: DefaultFilename, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_13()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: MultipleContentBuilderMock.Object, defaultFilename: DefaultFilename, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_14()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_14()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: MultipleContentBuilderMock.Object, context: null))
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

        [Fact]
        public void Fills_All_Properties_Correclty_When_Instanciating_Using_GenerationEnvironment()
        {
            // Act
            var instance = new RenderTemplateRequest(this, "Hello world", GenerationEnvironmentMock.Object, DefaultFilename, AdditionalParameters, TemplateContextMock.Object);

            // Assert
            instance.Should().NotBeNull();
            instance.Template.Should().BeSameAs(this);
            instance.Model.Should().Be("Hello world");
            instance.GenerationEnvironment.Should().BeSameAs(GenerationEnvironmentMock.Object);
            instance.DefaultFilename.Should().Be(DefaultFilename);
            instance.AdditionalParameters.Should().BeSameAs(AdditionalParameters);
            instance.Context.Should().BeSameAs(TemplateContextMock.Object);
        }
    }
}
