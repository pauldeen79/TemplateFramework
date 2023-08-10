namespace TemplateFramework.Core.Tests.StringBuilderTemplateRenderers;

public class WrappedStringBuilderTemplateRendererTests
{
    protected WrappedStringBuilderTemplateRenderer CreateSut() => new();
    protected StringBuilder Builder { get; } = new();

    public class TryRender : WrappedStringBuilderTemplateRendererTests
    {
        [Fact]
        public void Returns_False_When_Instance_Is_Null()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.TryRender(instance: null!, Builder);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_False_When_Instance_Does_Not_Have_Render_Method()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.TryRender(instance: new object(), Builder);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_False_When_Instance_Has_Render_Method_Without_StringBuilder_Parameter()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.TryRender(instance: new StringBuilderTemplateWithoutParameters(), Builder);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_False_When_Instance_Has_Render_Method_With_Two_StringBuilder_Parameters()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.TryRender(instance: new StringBuilderTemplateWithTwoParameters(), Builder);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_True_When_Instance_Has_Render_Method_With_StringBuilder_Parameter()
        {
            // Arrange
            var sut = CreateSut();
            var templateMock = new Mock<IStringBuilderTemplate>();

            // Act
            var result = sut.TryRender(instance: templateMock.Object, Builder);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Calls_Render_Method_When_Instance_Has_Render_Method_With_StringBuilder_Parameter()
        {
            // Arrange
            var sut = CreateSut();
            var templateMock = new Mock<IStringBuilderTemplate>();

            // Act
            _ = sut.TryRender(instance: templateMock.Object, Builder);

            // Assert
            templateMock.Verify(x => x.Render(Builder), Times.Once);
        }

        private sealed class StringBuilderTemplateWithoutParameters
        {
#pragma warning disable S1186 // Methods should not be empty
            public void Render() { }
#pragma warning restore S1186 // Methods should not be empty
        }

        private sealed class StringBuilderTemplateWithTwoParameters
        {
#pragma warning disable S1186 // Methods should not be empty
            public void Render(StringBuilder builder1, StringBuilder builder2) { }
#pragma warning restore S1186 // Methods should not be empty
        }
    }
}
