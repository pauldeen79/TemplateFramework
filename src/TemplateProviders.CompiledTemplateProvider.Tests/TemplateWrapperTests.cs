namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests;

public class TemplateWrapperTests
{
    protected const string DefaultFilename = "Filename.txt";

    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();
    protected Mock<ITemplateProvider> TemplateProviderMock { get; } = new();
    protected Mock<ITemplateContext> TemplateContextMock { get; } = new();

    protected TemplateWrapperTests()
    {
        TemplateContextMock.SetupGet(x => x.DefaultFilename).Returns(DefaultFilename);
        TemplateContextMock.SetupGet(x => x.Engine).Returns(TemplateEngineMock.Object);
        TemplateContextMock.SetupGet(x => x.Provider).Returns(TemplateProviderMock.Object);
    }

    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Instance()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateWrapper(instance: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("instance");
        }
    }

    public class GetParameters : TemplateWrapperTests
    {
        [Fact]
        public void Returns_Parameters_From_Wrapped_Instance_When_Wrapped_Instance_Implements_The_Interface_Shape_Correctly()
        {
            // Arrange
            var wrappedInstance = new TestData.PlainTemplateWithAdditionalParameters();
            var sut = new TemplateWrapper(wrappedInstance);

            // Act
            var result = sut.GetParameters();

            // Assert
            result.Should().BeEquivalentTo(wrappedInstance.GetParameters());
        }

        [Fact]
        public void Returns_Empty_Array_When_Wrapped_Instance_Does_Not_Have_GetParameters_Method_And_Does_Not_Have_Public_Properties()
        {
            // Arrange
            var wrappedInstance = new object();
            var sut = new TemplateWrapper(wrappedInstance);

            // Act
            var result = sut.GetParameters();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Returns_Public_Properties_When_Wrapped_Instance_Does_Not_Have_GetParameters_Method_But_Has_Public_Properties()
        {
            // Arrange
            var wrappedInstance = new TemplateWithTypedParameters();
            var sut = new TemplateWrapper(wrappedInstance);

            // Act
            var result = sut.GetParameters();

            // Assert
            result.Select(x => x.Name).Should().BeEquivalentTo(nameof(TemplateWithTypedParameters.Prop1), nameof(TemplateWithTypedParameters.Prop2));
            result.Select(x => x.Type).Should().AllBeEquivalentTo(typeof(string));
        }

        [Fact]
        public void Throws_When_Wrapped_Instance_Returns_Result_From_GetParameters_Using_Wrong_Type()
        {
            // Arrange
            var wrappedInstance = new TestData.AdditionalParametersWrongTypeTemplate();
            var sut = new TemplateWrapper(wrappedInstance);

            // Act & Assert
            sut.Invoking(x => x.GetParameters())
               .Should().Throw<NotSupportedException>();
        }

        private sealed class TemplateWithTypedParameters
        {
            public string? Prop1 { get; set; }
            public string? Prop2 { get; set; }
            public string? Skip1 { get; }
#pragma warning disable S2376 // Write-only properties should not be used
            public string? Skip2
#pragma warning restore S2376 // Write-only properties should not be used
            {
#pragma warning disable S3237 // "value" contextual keyword should be used
                set
#pragma warning restore S3237 // "value" contextual keyword should be used
                {
                    // Left empty intentionally
                }
            }
        }
    }

    public class Render_MultipleContentBuilder : TemplateWrapperTests
    {
        [Fact]
        public void Throws_On_Null_Builder()
        {
            // Arrange
            var wrappedInstance = new TestData.PlainTemplateWithAdditionalParameters();
            var sut = new TemplateWrapper(wrappedInstance);

            // Act & Assert
            sut.Invoking(x => x.Render(builder: default(IMultipleContentBuilder)!))
               .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Initializes_Model_Correctly_When_Available()
        {
            // Arrange
            var wrappedInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<int>();
            var sut = new TemplateWrapper(wrappedInstance)
            {
                Model = 13 // Note that normally, this would get set using RenderTemplateRequest in the TemplateEngine. But we're unit testing here...
            };
            var builder = new MultipleContentBuilder();

            // Act
            sut.Render(builder);

            // Assert
            wrappedInstance.Model.Should().Be(13);
        }

        [Fact]
        public void Renders_To_Wrapped_TransformText_Template_Correctly()
        {
            // Arrange
            var wrappedInstance = new TestData.TextTransformTemplate(() => "Hello world!");
            var sut = new TemplateWrapper(wrappedInstance);
            var builder = new MultipleContentBuilder();

            // Act
            sut.Render(builder);

            // Assert
            builder.Contents.Should().ContainSingle();
            builder.Contents.Single().Builder.ToString().Should().Be("Hello world!");
            builder.Contents.Single().Filename.Should().BeEmpty();
        }

        [Fact]
        public void Sets_Filename_Correctly_With_Wrapped_TransformText_Template()
        {
            // Arrange
            var wrappedInstance = new TestData.TextTransformTemplate(() => "Hello world!");
            var sut = new TemplateWrapper(wrappedInstance);
            sut.Context = TemplateContextMock.Object;
            var builder = new MultipleContentBuilder();

            // Act
            sut.Render(builder);

            // Assert
            builder.Contents.Should().ContainSingle();
            builder.Contents.Single().Filename.Should().Be(DefaultFilename);
        }

        [Fact]
        public void Renders_To_Wrapped_Plain_Template_Correctly()
        {
            // Arrange
            var wrappedInstance = new TestData.PlainTemplateWithAdditionalParameters();
            var sut = new TemplateWrapper(wrappedInstance);
            sut.SetParameter(nameof(TestData.PlainTemplateWithAdditionalParameters.AdditionalParameter), "Hello world!");
            var builder = new MultipleContentBuilder();

            // Act
            sut.Render(builder);

            // Assert
            builder.Contents.Should().ContainSingle();
            builder.Contents.Single().Builder.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void Renders_To_Wrapped_Template_With_Invalid_Render_Method_Signature_Two_Arguments_Correctly()
        {
            // Arrange
            var wrappedInstance = new TestData.MultipleContentBuilderTwoWrongArgumentsTemplate(() => "Hello world!");
            var sut = new TemplateWrapper(wrappedInstance);
            var builder = new MultipleContentBuilder();

            // Act
            sut.Render(builder);

            // Assert
            builder.Contents.Should().ContainSingle();
            builder.Contents.Single().Builder.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void Sets_Filename_Correctly_With_Wrapped_Plain_Template()
        {
            // Arrange
            var wrappedInstance = new TestData.PlainTemplateWithAdditionalParameters();
            var sut = new TemplateWrapper(wrappedInstance);
            sut.SetParameter(nameof(TestData.PlainTemplateWithAdditionalParameters.AdditionalParameter), "Hello world!");
            sut.Context = TemplateContextMock.Object;
            var builder = new MultipleContentBuilder();

            // Act
            sut.Render(builder);

            // Assert
            builder.Contents.Should().ContainSingle();
            builder.Contents.Single().Filename.Should().Be(DefaultFilename);
        }

        [Fact]
        public void Renders_To_Wrapped_MultipleContentBuilder_Template_Correctly()
        {
            // Arrange
            var wrappedInstance = new TestData.MultipleContentBuilderTemplate(x => x.AddContent("Filename.txt", false, null).Builder.Append("Hello world!"));
            var sut = new TemplateWrapper(wrappedInstance);
            var builder = new MultipleContentBuilder();

            // Act
            sut.Render(builder);

            // Assert
            builder.Contents.Should().ContainSingle();
            builder.Contents.Single().Builder.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void Renders_To_Wrapped_StringBuilder_Template_Correctly()
        {
            // Arrange
            var wrappedInstance = new TestData.StringBuilderTemplate(stringBuilder => stringBuilder.Append("Hello world!"));
            var sut = new TemplateWrapper(wrappedInstance);
            var builder = new MultipleContentBuilder();

            // Act
            sut.Render(builder);

            // Assert
            builder.Contents.Should().ContainSingle();
            builder.Contents.Single().Builder.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void Sets_Filename_Correctly_With_Wrapped_StringBuilder_Template()
        {
            // Arrange
            var wrappedInstance = new TestData.StringBuilderTemplate(stringBuilder => stringBuilder.Append("Hello world!"));
            var sut = new TemplateWrapper(wrappedInstance);
            sut.Context = TemplateContextMock.Object;
            var builder = new MultipleContentBuilder();

            // Act
            sut.Render(builder);

            // Assert
            builder.Contents.Should().ContainSingle();
            builder.Contents.Single().Filename.Should().Be(DefaultFilename);
        }

        [Fact]
        public void Renders_To_Wrapped_Template_With_Invalid_Render_Method_Signature_One_Argument_Correctly()
        {
            // Arrange
            var wrappedInstance = new TestData.MultipleContentBuilderOneWrongArgumentTemplate(() => "Hello world!");
            var sut = new TemplateWrapper(wrappedInstance);
            var builder = new MultipleContentBuilder();

            // Act
            sut.Render(builder);

            // Assert
            builder.Contents.Should().ContainSingle();
            builder.Contents.Single().Builder.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void Sets_Filename_Correctly_With_Invalid_Render_Method_Signature_One_Argument_Template()
        {
            // Arrange
            var wrappedInstance = new TestData.MultipleContentBuilderOneWrongArgumentTemplate(() => "Hello world!");
            var sut = new TemplateWrapper(wrappedInstance);
            sut.Context = TemplateContextMock.Object;
            var builder = new MultipleContentBuilder();

            // Act
            sut.Render(builder);

            // Assert
            builder.Contents.Should().ContainSingle();
            builder.Contents.Single().Filename.Should().Be(DefaultFilename);
        }

        [Fact]
        public void Does_Not_Set_Context_On_Wrapped_Template_When_Context_Is_Not_Typed()
        {
            // Arrange
            var wrappedInstance = new TestData.PlainTemplateWithTemplateContext(_ => "Hello world!");
            var sut = new TemplateWrapper(wrappedInstance);
            sut.Context = TemplateContextMock.Object;
            var builder = new MultipleContentBuilder();

            // Act
            sut.Render(builder);

            // Assert
            wrappedInstance.Context.Should().BeNull();
        }

        [Fact]
        public void Sets_Context_On_Wrapped_Template_When_Context_Is_Typed()
        {
            // Arrange
            var wrappedInstance = new TestData.PlainTemplateWithTypedTemplateContext(_ => "Hello world!");
            var sut = new TemplateWrapper(wrappedInstance);
            sut.Context = TemplateContextMock.Object;
            var builder = new MultipleContentBuilder();

            // Act
            sut.Render(builder);

            // Assert
            wrappedInstance.Context.Should().BeSameAs(sut.Context);
        }
    }

    public class Render_StringBuilder : TemplateWrapperTests
    {
        [Fact]
        public void Throws_On_Null_Builder()
        {
            // Arrange
            var wrappedInstance = new TestData.PlainTemplateWithAdditionalParameters();
            var sut = new TemplateWrapper(wrappedInstance);

            // Act & Assert
            sut.Invoking(x => x.Render(builder: default(StringBuilder)!))
               .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Initializes_Model_Correctly_When_Available()
        {
            // Arrange
            var wrappedInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<int>();
            var sut = new TemplateWrapper(wrappedInstance)
            {
                Model = 13 // Note that normally, this would get set using RenderTemplateRequest in the TemplateEngine. But we're unit testing here...
            };
            var builder = new StringBuilder();

            // Act
            sut.Render(builder);

            // Assert
            wrappedInstance.Model.Should().Be(13);
        }

        [Fact]
        public void Sets_Context_On_Wrapped_Template_When_Context_Is_Typed()
        {
            // Arrange
            var wrappedInstance = new TestData.PlainTemplateWithTypedTemplateContext(_ => "Hello world!");
            var sut = new TemplateWrapper(wrappedInstance);
            sut.Context = TemplateContextMock.Object;
            var builder = new StringBuilder();

            // Act
            sut.Render(builder);

            // Assert
            wrappedInstance.Context.Should().BeSameAs(sut.Context);
        }

        [Fact]
        public void Renders_To_Wrapped_TransformText_Template_Correctly()
        {
            // Arrange
            var wrappedInstance = new TestData.TextTransformTemplate(() => "Hello world!");
            var sut = new TemplateWrapper(wrappedInstance);
            var builder = new StringBuilder();

            // Act
            sut.Render(builder);

            // Assert
            builder.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void Renders_To_Wrapped_Plain_Template_Correctly()
        {
            // Arrange
            var wrappedInstance = new TestData.PlainTemplateWithAdditionalParameters();
            var sut = new TemplateWrapper(wrappedInstance);
            sut.SetParameter(nameof(TestData.PlainTemplateWithAdditionalParameters.AdditionalParameter), "Hello world!");
            var builder = new StringBuilder();

            // Act
            sut.Render(builder);

            // Assert
            builder.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void Renders_To_Wrapped_Template_With_Invalid_Render_Method_Signature_Two_Arguments_Correctly()
        {
            // Arrange
            var wrappedInstance = new TestData.MultipleContentBuilderTwoWrongArgumentsTemplate(() => "Hello world!");
            var sut = new TemplateWrapper(wrappedInstance);
            var builder = new StringBuilder();

            // Act
            sut.Render(builder);

            // Assert
            builder.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void Renders_To_Wrapped_MultipleContentBuilder_Template_Correctly()
        {
            // Arrange
            var wrappedInstance = new TestData.MultipleContentBuilderTemplate(x => x.AddContent("Filename.txt", false, null).Builder.Append("Hello world!"));
            var sut = new TemplateWrapper(wrappedInstance);
            var builder = new StringBuilder();

            // Act
            sut.Render(builder);

            // Assert
            builder.ToString().Should().Be(@"Filename.txt:
Hello world!
");
        }

        [Fact]
        public void Renders_To_Wrapped_StringBuilder_Template_Correctly()
        {
            // Arrange
            var wrappedInstance = new TestData.StringBuilderTemplate(stringBuilder => stringBuilder.Append("Hello world!"));
            var sut = new TemplateWrapper(wrappedInstance);
            var builder = new StringBuilder();

            // Act
            sut.Render(builder);

            // Assert
            builder.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void Renders_To_Wrapped_Template_With_Invalid_Render_Method_Signature_One_Argument_Correctly()
        {
            // Arrange
            var wrappedInstance = new TestData.MultipleContentBuilderOneWrongArgumentTemplate(() => "Hello world!");
            var sut = new TemplateWrapper(wrappedInstance);
            var builder = new StringBuilder();

            // Act
            sut.Render(builder);

            // Assert
            builder.ToString().Should().Be("Hello world!");
        }
    }

    public class SetParameter : TemplateWrapperTests
    {
        [Fact]
        public void Does_Not_Throw_When_Wrapped_Template_Does_Not_Have_SetParameter_Method()
        {
            // Arrange
            var wrappedInstance = new TestData.StringBuilderTemplate(_ => { });
            var sut = new TemplateWrapper(wrappedInstance);

            // Act
            sut.Invoking(x => x.SetParameter("NameDoesNotMatterHere", "ValueDoesNotMatterEither"))
               .Should().NotThrow();
        }

        [Fact]
        public void Renders_To_Wrapped_Plain_Template_Correctly()
        {
            // Arrange
            var wrappedInstance = new TestData.PlainTemplateWithAdditionalParameters();
            var sut = new TemplateWrapper(wrappedInstance);

            // Act
            sut.SetParameter(nameof(TestData.PlainTemplateWithAdditionalParameters.AdditionalParameter), "Hello world!");

            // Assert
            wrappedInstance.AdditionalParameter.Should().Be("Hello world!");
        }
    }
}
