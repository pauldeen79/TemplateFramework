namespace TemplateFramework.Core.Tests.StringBuilderTemplateRenderers;

public class TypedStringBuilderTemplateRendererTests
{
    [Fact]
    public void Returns_False_On_Null_Instance()
    {
        // Arrange
        var sut = new TypedStringBuilderTemplateRenderer();

        // Act
        var result = sut.TryRender(instance: null!, new StringBuilder());

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Returns_False_On_NonNull_Instance_But_Wrong_Type()
    {
        // Arrange
        var sut = new TypedStringBuilderTemplateRenderer();

        // Act
        var result = sut.TryRender(instance: this, new StringBuilder());

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Returns_True_On_IStringBuilderTemplate_Instance()
    {
        // Arrange
        var sut = new TypedStringBuilderTemplateRenderer();
        var templateMock = new Mock<IStringBuilderTemplate>();

        // Act
        var result = sut.TryRender(instance: templateMock.Object, new StringBuilder());

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Renders_Template_On_IStringBuilderTemplate_Instance()
    {
        // Arrange
        var sut = new TypedStringBuilderTemplateRenderer();
        var templateMock = new Mock<IStringBuilderTemplate>();
        var builder = new StringBuilder();

        // Act
        _ = sut.TryRender(instance: templateMock.Object, builder);

        // Assert
        templateMock.Verify(x => x.Render(builder), Times.Once);
    }
}
