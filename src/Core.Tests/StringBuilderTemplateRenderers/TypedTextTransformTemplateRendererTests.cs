namespace TemplateFramework.Core.Tests.StringBuilderTemplateRenderers;

public class TypedTextTransformTemplateRendererTests
{
    public class TryRender
    {
        [Theory, AutoMockData]
        public void Returns_False_On_Null_Instance(TypedTextTransformTemplateRenderer sut)
        {
            // Act
            var result = sut.TryRender(instance: null!, new StringBuilder());

            // Assert
            result.Should().BeFalse();
        }

        [Theory, AutoMockData]
        public void Returns_False_On_NonNull_Instance_But_Wrong_Type(TypedTextTransformTemplateRenderer sut)
        {
            // Act
            var result = sut.TryRender(instance: this, new StringBuilder());

            // Assert
            result.Should().BeFalse();
        }

        [Theory, AutoMockData]
        public void Returns_True_On_ITextTransformTemplate_Instance(
            [Frozen] ITextTransformTemplate textTransformTemplate,
            TypedTextTransformTemplateRenderer sut)
        {
            // Act
            var result = sut.TryRender(instance: textTransformTemplate, new StringBuilder());

            // Assert
            result.Should().BeTrue();
        }

        [Theory, AutoMockData]
        public void Renders_Template_On_IStringBuilderTemplate_Instance(
            [Frozen] ITextTransformTemplate textTransformTemplate,
            TypedTextTransformTemplateRenderer sut)
        {
            // Act
            _ = sut.TryRender(instance: textTransformTemplate, new StringBuilder());

            // Assert
            textTransformTemplate.Received().TransformText();
        }

        [Theory, AutoMockData]
        public void Appends_Result_From_Template_To_GenerationEnvironment_On_IStringBuilderTemplate_Instance(
            [Frozen] ITextTransformTemplate textTransformTemplate,
            TypedTextTransformTemplateRenderer sut)
        {
            // Arrange
            textTransformTemplate.TransformText().Returns("Hello world!");
            var builder = new StringBuilder();

            // Act
            _ = sut.TryRender(instance: textTransformTemplate, builder);

            // Assert
            builder.ToString().Should().Be("Hello world!");
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_GenerationEnvironment(
            [Frozen] ITextTransformTemplate textTransformTemplate,
            TypedTextTransformTemplateRenderer sut)
        {
            // Arrange
            textTransformTemplate.TransformText().Returns("Hello world!");

            // Act & Assert
            sut.Invoking(x => x.TryRender(textTransformTemplate, builder: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }
    }
}
