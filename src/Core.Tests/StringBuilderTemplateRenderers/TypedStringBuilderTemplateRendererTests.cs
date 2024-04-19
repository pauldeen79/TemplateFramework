namespace TemplateFramework.Core.Tests.StringBuilderTemplateRenderers;

public class TypedStringBuilderTemplateRendererTests
{
    public class TryRender
    {
        [Theory, AutoMockData]
        public void Returns_False_On_Null_Instance(TypedStringBuilderTemplateRenderer sut)
        {
            // Act
            var result = sut.TryRender(instance: null!, new StringBuilder());

            // Assert
            result.Should().BeFalse();
        }

        [Theory, AutoMockData]
        public void Returns_False_On_NonNull_Instance_But_Wrong_Type(TypedStringBuilderTemplateRenderer sut)
        {
            // Act
            var result = sut.TryRender(instance: this, new StringBuilder());

            // Assert
            result.Should().BeFalse();
        }

        [Theory, AutoMockData]
        public void Returns_True_On_IStringBuilderTemplate_Instance(
            [Frozen] IStringBuilderTemplate stringBuilderTemplate,
            TypedStringBuilderTemplateRenderer sut)
        {
            // Act
            var result = sut.TryRender(instance: stringBuilderTemplate, new StringBuilder());

            // Assert
            result.Should().BeTrue();
        }

        [Theory, AutoMockData]
        public void Renders_Template_On_IStringBuilderTemplate_Instance(
            [Frozen] IStringBuilderTemplate stringBuilderTemplate, 
            TypedStringBuilderTemplateRenderer sut)
        {
            // Arrange
            var builder = new StringBuilder();

            // Act
            _ = sut.TryRender(instance: stringBuilderTemplate, builder);

            // Assert
            stringBuilderTemplate.Received().Render(builder);
        }
    }
}
