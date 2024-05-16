namespace TemplateFramework.Core.Tests.StringBuilderTemplateRenderers;

public class TypedStringBuilderTemplateRendererTests
{
    public class TryRender
    {
        [Theory, AutoMockData]
        public async Task Returns_False_On_Null_Instance(TypedStringBuilderTemplateRenderer sut)
        {
            // Act
            var result = await sut.TryRender(instance: null!, new StringBuilder(), CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }

        [Theory, AutoMockData]
        public async Task Returns_False_On_NonNull_Instance_But_Wrong_Type(TypedStringBuilderTemplateRenderer sut)
        {
            // Act
            var result = await sut.TryRender(instance: this, new StringBuilder(), CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }

        [Theory, AutoMockData]
        public async Task Returns_True_On_IStringBuilderTemplate_Instance(
            [Frozen] IStringBuilderTemplate stringBuilderTemplate,
            TypedStringBuilderTemplateRenderer sut)
        {
            // Act
            var result = await sut.TryRender(instance: stringBuilderTemplate, new StringBuilder(), CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Theory, AutoMockData]
        public async Task Renders_Template_On_IStringBuilderTemplate_Instance(
            [Frozen] IStringBuilderTemplate stringBuilderTemplate, 
            TypedStringBuilderTemplateRenderer sut)
        {
            // Arrange
            var builder = new StringBuilder();

            // Act
            _ = await sut.TryRender(instance: stringBuilderTemplate, builder, CancellationToken.None);

            // Assert
            stringBuilderTemplate.Received().Render(builder);
        }
    }
}
