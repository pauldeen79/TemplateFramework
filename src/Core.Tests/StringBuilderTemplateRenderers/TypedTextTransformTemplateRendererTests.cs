namespace TemplateFramework.Core.Tests.StringBuilderTemplateRenderers;

public class TypedTextTransformTemplateRendererTests
{
    [Fact]
    public void Returns_False_On_Null_Instance()
    {
        // Arrange
        var sut = new TypedTextTransformTemplateRenderer();

        // Act
        var result = sut.TryRender(instance: null!, new StringBuilder());

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Returns_False_On_NonNull_Instance_But_Wrong_Type()
    {
        // Arrange
        var sut = new TypedTextTransformTemplateRenderer();

        // Act
        var result = sut.TryRender(instance: this, new StringBuilder());

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Returns_True_On_ITextTransformTemplate_Instance()
    {
        // Arrange
        var sut = new TypedTextTransformTemplateRenderer();
        var templateMock = Substitute.For<ITextTransformTemplate>();

        // Act
        var result = sut.TryRender(instance: templateMock, new StringBuilder());

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Renders_Template_On_IStringBuilderTemplate_Instance()
    {
        // Arrange
        var sut = new TypedTextTransformTemplateRenderer();
        var templateMock = Substitute.For<ITextTransformTemplate>();

        // Act
        _ = sut.TryRender(instance: templateMock, new StringBuilder());

        // Assert
        templateMock.Received().TransformText();
    }

    [Fact]
    public void Appends_Result_From_Template_To_GenerationEnvironment_On_IStringBuilderTemplate_Instance()
    {
        // Arrange
        var sut = new TypedTextTransformTemplateRenderer();
        var templateMock = Substitute.For<ITextTransformTemplate>();
        templateMock.TransformText().Returns("Hello world!");
        var builder = new StringBuilder();

        // Act
        _ = sut.TryRender(instance: templateMock, builder);

        // Assert
        builder.ToString().Should().Be("Hello world!");
    }

    [Fact]
    public void Throws_On_Null_GenerationEnvironment()
    {
        // Arrange
        var sut = new TypedTextTransformTemplateRenderer();
        var templateMock = Substitute.For<ITextTransformTemplate>();
        templateMock.TransformText().Returns("Hello world!");

        // Act & Assert
        sut.Invoking(x => x.TryRender(templateMock, builder: null!))
           .Should().Throw<ArgumentNullException>().WithParameterName("builder");
    }
}
