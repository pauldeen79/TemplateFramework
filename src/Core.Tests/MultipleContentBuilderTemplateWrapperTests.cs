namespace TemplateFramework.Core.Tests;

public class MultipleContentBuilderTemplateWrapperTests
{
    [Fact]
    public void Throws_On_Null_Instance()
    {
        // Act & Assert
        this.Invoking(_ => new MultipleContentBuilderTemplateWrapper(instance: null!))
            .Should().Throw<ArgumentNullException>().WithParameterName("instance");
    }

    [Fact]
    public void Wraps_Instance_Correctly()
    {
        // Arrange
        var multipleContentBuilderMock = new Mock<IMultipleContentBuilder>();
        var wrappedTemplate = new MyMultipleContentBuilderTemplate();
        var instance = new MultipleContentBuilderTemplateWrapper(wrappedTemplate);

        // Act
        instance.Render(multipleContentBuilderMock.Object);

        // Assert
        wrappedTemplate.Builder.Should().NotBeNull();
    }

    [Fact]
    public void Throws_When_Wrapped_Instance_Does_Not_Have_Render_Method()
    {
        // Arrange
        var sut = new MultipleContentBuilderTemplateWrapper(new object());
        var multipleContentBuilderMock = new Mock<IMultipleContentBuilder>();

        // Act & Assert
        sut.Invoking(x => x.Render(multipleContentBuilderMock.Object))
            .Should().Throw<InvalidOperationException>();
    }

    private sealed class MyMultipleContentBuilderTemplate
    {
        public IMultipleContentBuilder? Builder { get; private set; }

#pragma warning disable S1144 // Unused private types or members should be removed
        public void Render(IMultipleContentBuilder builder) => Builder = builder;
#pragma warning restore S1144 // Unused private types or members should be removed
    }
}
