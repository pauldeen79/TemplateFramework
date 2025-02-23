namespace TemplateFramework.Core.Tests.Requests;

public class RenderTemplateRequestTests
{
    public class Constructor
    {
        private StringBuilder StringBuilder { get; } = new();

        private const string DefaultFilename = "MyFile.txt";
        private object AdditionalParameters { get; } = new object();

        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(RenderTemplateRequest).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(p => !new[] { "model", "additionalParameters", "context" }.Contains(p.Name));
        }

        [Fact]
        public void Constructs_Using_StringBuilder_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), null, builder: StringBuilder, defaultFilename: DefaultFilename, additionalParameters: null, context: null))
                .ShouldNotThrow();
        }

        [Fact]
        public void Constructs_Using_StringBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), null, builder: StringBuilder))
                .ShouldNotThrow();
        }

        [Fact]
        public void Constructs_Using_StringBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), null, builder: StringBuilder, defaultFilename: DefaultFilename))
                .ShouldNotThrow();
        }

        [Fact]
        public void Constructs_Using_StringBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), null, builder: StringBuilder, defaultFilename: DefaultFilename, additionalParameters: null))
                .ShouldNotThrow();
        }

        [Fact]
        public void Constructs_Using_StringBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), null, builder: StringBuilder, additionalParameters: null))
                .ShouldNotThrow();
        }

        [Fact]
        public void Constructs_Using_StringBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), null, builder: StringBuilder, defaultFilename: DefaultFilename, context: null))
                .ShouldNotThrow();
        }

        [Fact]
        public void Constructs_Using_StringBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), null, builder: StringBuilder, context: null))
                .ShouldNotThrow();
        }

        [Fact]
        public void Constructs_Using_StringBuilder_8()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), builder: StringBuilder, defaultFilename: DefaultFilename, additionalParameters: null, context: null))
                .ShouldNotThrow();
        }

        [Fact]
        public void Constructs_Using_StringBuilder_9()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), builder: StringBuilder))
                .ShouldNotThrow();
        }

        [Fact]
        public void Constructs_Using_StringBuilder_10()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), builder: StringBuilder, defaultFilename: DefaultFilename))
                .ShouldNotThrow();
        }

        [Fact]
        public void Constructs_Using_StringBuilder_11()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), builder: StringBuilder, defaultFilename: DefaultFilename, additionalParameters: null))
                .ShouldNotThrow();
        }

        [Fact]
        public void Constructs_Using_StringBuilder_12()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), builder: StringBuilder, additionalParameters: null))
                .ShouldNotThrow();
        }

        [Fact]
        public void Constructs_Using_StringBuilder_13()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), builder: StringBuilder, defaultFilename: DefaultFilename, context: null))
                .ShouldNotThrow();
        }

        [Fact]
        public void Constructs_Using_StringBuilder_14()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), builder: StringBuilder, context: null))
                .ShouldNotThrow();
        }

        [Theory, AutoMockData]
        public void Constructs_Using_MultipleContentBuilder_1([Frozen] IMultipleContentBuilder<StringBuilder> multipleContentBuilder)
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), null, builder: multipleContentBuilder, defaultFilename: DefaultFilename, additionalParameters: null, context: null))
                .ShouldNotThrow();
        }

        [Theory, AutoMockData]
        public void Constructs_Using_MultipleContentBuilder_2([Frozen] IMultipleContentBuilder<StringBuilder> multipleContentBuilder)
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), null, builder: multipleContentBuilder))
                .ShouldNotThrow();
        }

        [Theory, AutoMockData]
        public void Constructs_Using_MultipleContentBuilder_3([Frozen] IMultipleContentBuilder<StringBuilder> multipleContentBuilder)
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), null, builder: multipleContentBuilder, defaultFilename: DefaultFilename))
                .ShouldNotThrow();
        }

        [Theory, AutoMockData]
        public void Constructs_Using_MultipleContentBuilder_4([Frozen] IMultipleContentBuilder<StringBuilder> multipleContentBuilder)
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), null, builder: multipleContentBuilder, defaultFilename: DefaultFilename, additionalParameters: null))
                .ShouldNotThrow();
        }

        [Theory, AutoMockData]
        public void Constructs_Using_MultipleContentBuilder_5([Frozen] IMultipleContentBuilder<StringBuilder> multipleContentBuilder)
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), null, builder: multipleContentBuilder, additionalParameters: null))
                .ShouldNotThrow();
        }

        [Theory, AutoMockData]
        public void Constructs_Using_MultipleContentBuilder_6([Frozen] IMultipleContentBuilder<StringBuilder> multipleContentBuilder)
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), null, builder: multipleContentBuilder, defaultFilename: DefaultFilename, context: null))
                .ShouldNotThrow();
        }

        [Theory, AutoMockData]
        public void Constructs_Using_MultipleContentBuilder_7([Frozen] IMultipleContentBuilder<StringBuilder> multipleContentBuilder)
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), null, builder: multipleContentBuilder, context: null))
                .ShouldNotThrow();
        }

        [Theory, AutoMockData]
        public void Constructs_Using_MultipleContentBuilder_8([Frozen] IMultipleContentBuilder<StringBuilder> multipleContentBuilder)
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), builder: multipleContentBuilder, defaultFilename: DefaultFilename, additionalParameters: null, context: null))
                .ShouldNotThrow();
        }

        [Theory, AutoMockData]
        public void Constructs_Using_MultipleContentBuilder_9([Frozen] IMultipleContentBuilder<StringBuilder> multipleContentBuilder)
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), builder: multipleContentBuilder))
                .ShouldNotThrow();
        }

        [Theory, AutoMockData]
        public void Constructs_Using_MultipleContentBuilder_10([Frozen] IMultipleContentBuilder<StringBuilder> multipleContentBuilder)
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), builder: multipleContentBuilder, defaultFilename: DefaultFilename))
                .ShouldNotThrow();
        }

        [Theory, AutoMockData]
        public void Constructs_Using_MultipleContentBuilder_11([Frozen] IMultipleContentBuilder<StringBuilder> multipleContentBuilder)
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), builder: multipleContentBuilder, defaultFilename: DefaultFilename, additionalParameters: null))
                .ShouldNotThrow();
        }

        [Theory, AutoMockData]
        public void Constructs_Using_MultipleContentBuilder_12([Frozen] IMultipleContentBuilder<StringBuilder> multipleContentBuilder)
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), builder: multipleContentBuilder, additionalParameters: null))
                .ShouldNotThrow();
        }

        [Theory, AutoMockData]
        public void Constructs_Using_MultipleContentBuilder_13([Frozen] IMultipleContentBuilder<StringBuilder> multipleContentBuilder)
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), builder: multipleContentBuilder, defaultFilename: DefaultFilename, context: null))
                .ShouldNotThrow();
        }

        [Theory, AutoMockData]
        public void Constructs_Using_MultipleContentBuilder_14([Frozen] IMultipleContentBuilder<StringBuilder> multipleContentBuilder)
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(new TemplateInstanceIdentifier(this), builder: multipleContentBuilder, context: null))
                .ShouldNotThrow();
        }

        [Theory, AutoMockData]
        public void Fills_All_Properties_Correclty_When_Instanciating_Using_GenerationEnvironment(
            [Frozen] IGenerationEnvironment generationEnvironment,
            [Frozen] ITemplateContext templateContext)
        {
            // Act
            var instance = new RenderTemplateRequest(new TemplateInstanceIdentifier(this), "Hello world", generationEnvironment, DefaultFilename, AdditionalParameters, templateContext);

            // Assert
            instance.ShouldNotBeNull();
            instance.Identifier.ShouldBeOfType<TemplateInstanceIdentifier>().And.Match<TemplateInstanceIdentifier>(x => x.Instance == this);
            instance.Model.ShouldBe("Hello world");
            instance.GenerationEnvironment.ShouldBeSameAs(generationEnvironment);
            instance.DefaultFilename.ShouldBe(DefaultFilename);
            instance.AdditionalParameters.ShouldBeSameAs(AdditionalParameters);
            instance.Context.ShouldBeSameAs(templateContext);
        }
    }
}
