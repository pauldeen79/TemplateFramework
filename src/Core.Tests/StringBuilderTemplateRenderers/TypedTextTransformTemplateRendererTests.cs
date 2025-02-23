namespace TemplateFramework.Core.Tests.StringBuilderTemplateRenderers;

public class TypedTextTransformTemplateRendererTests
{
    public class TryRender
    {
        [Theory, AutoMockData]
        public async Task Returns_Continue_On_Null_Instance(TypedTextTransformTemplateRenderer sut)
        {
            // Act
            var result = await sut.TryRender(instance: null!, new StringBuilder(), CancellationToken.None);

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }

        [Theory, AutoMockData]
        public async Task Returns_Continue_On_NonNull_Instance_But_Wrong_Type(TypedTextTransformTemplateRenderer sut)
        {
            // Act
            var result = await sut.TryRender(instance: this, new StringBuilder(), CancellationToken.None);

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }

        [Theory, AutoMockData]
        public async Task Returns_Success_On_ITextTransformTemplate_Instance(
            [Frozen] ITextTransformTemplate textTransformTemplate,
            TypedTextTransformTemplateRenderer sut)
        {
            // Act
            var result = await sut.TryRender(instance: textTransformTemplate, new StringBuilder(), CancellationToken.None);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Theory, AutoMockData]
        public async Task Renders_Template_On_IStringBuilderTemplate_Instance(
            [Frozen] ITextTransformTemplate textTransformTemplate,
            TypedTextTransformTemplateRenderer sut)
        {
            // Act
            _ = await sut.TryRender(instance: textTransformTemplate, new StringBuilder(), CancellationToken.None);

            // Assert
            await textTransformTemplate.Received().TransformText(Arg.Any<CancellationToken>());
        }

        [Theory, AutoMockData]
        public async Task Appends_Result_From_Template_To_GenerationEnvironment_On_IStringBuilderTemplate_Instance(
            [Frozen] ITextTransformTemplate textTransformTemplate,
            TypedTextTransformTemplateRenderer sut)
        {
            // Arrange
            textTransformTemplate.TransformText(Arg.Any<CancellationToken>()).Returns("Hello world!");
            var builder = new StringBuilder();

            // Act
            _ = await sut.TryRender(instance: textTransformTemplate, builder, CancellationToken.None);

            // Assert
            builder.ToString().Should().Be("Hello world!");
        }

        [Theory, AutoMockData]
        public async Task Throws_On_Null_GenerationEnvironment(
            [Frozen] ITextTransformTemplate textTransformTemplate,
            TypedTextTransformTemplateRenderer sut)
        {
            // Arrange
            textTransformTemplate.TransformText(Arg.Any<CancellationToken>()).Returns("Hello world!");

            // Act & Assert
            Task t = sut.TryRender(textTransformTemplate, builder: null!, CancellationToken.None))
            (await t.ShouldThrowAsync<ArgumentNullException>()).ParamName.ShouldBe("builder");
        }
    }
}
