namespace TemplateFramework.Core.Tests.StringBuilderTemplateRenderers;

public class TypedStringBuilderTemplateRendererTests
{
    public class TryRender
    {
        [Theory, AutoMockData]
        public async Task Returns_Continue_On_Null_Instance(TypedBuilderTemplateRenderer<StringBuilder> sut)
        {
            // Act
            var result = await sut.TryRender(instance: null!, new StringBuilder(), CancellationToken.None);

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }

        [Theory, AutoMockData]
        public async Task Returns_Continue_On_NonNull_Instance_But_Wrong_Type(TypedBuilderTemplateRenderer<StringBuilder> sut)
        {
            // Act
            var result = await sut.TryRender(instance: this, new StringBuilder(), CancellationToken.None);

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }

        [Theory, AutoMockData]
        public async Task Returns_Success_On_IStringBuilderTemplate_Instance(
            [Frozen] IBuilderTemplate<StringBuilder> stringBuilderTemplate,
            TypedBuilderTemplateRenderer<StringBuilder> sut)
        {
            // Act
            var result = await sut.TryRender(instance: stringBuilderTemplate, new StringBuilder(), CancellationToken.None);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
        }

        [Theory, AutoMockData]
        public async Task Renders_Template_On_IStringBuilderTemplate_Instance(
            [Frozen] IBuilderTemplate<StringBuilder> stringBuilderTemplate,
            TypedBuilderTemplateRenderer<StringBuilder> sut)
        {
            // Arrange
            var builder = new StringBuilder();

            // Act
            _ = await sut.TryRender(instance: stringBuilderTemplate, builder, CancellationToken.None);

            // Assert
            await stringBuilderTemplate.Received().Render(builder, Arg.Any<CancellationToken>());
        }
    }
}
