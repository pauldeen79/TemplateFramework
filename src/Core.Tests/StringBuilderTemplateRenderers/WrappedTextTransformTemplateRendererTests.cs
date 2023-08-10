﻿namespace TemplateFramework.Core.Tests.TextTransformTemplateRenderers;

public class WrappedTextTransformTemplateRendererTests
{
    protected WrappedTextTransformTemplateRenderer CreateSut() => new();
    protected StringBuilder Builder { get; } = new();

    public class TryRender : WrappedTextTransformTemplateRendererTests
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
        public void Returns_False_When_Instance_Has_TransformText_Method_With_Parameter()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.TryRender(instance: new TextTransformTemplateWithParameters(), Builder);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_False_When_Instance_Has_TransformText_Method_Of_Wrong_ReturnType()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.TryRender(instance: new TextTransformTemplateWithWrongReturnType(), Builder);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_True_When_Instance_Has_TransformText_Method_Without_Parameter()
        {
            // Arrange
            var sut = CreateSut();
            var templateMock = new Mock<ITextTransformTemplate>();

            // Act
            var result = sut.TryRender(instance: templateMock.Object, Builder);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Calls_Render_Method_When_Instance_Has_Render_Method_Without_Parameter()
        {
            // Arrange
            var sut = CreateSut();
            var templateMock = new Mock<ITextTransformTemplate>();

            // Act
            var result = sut.TryRender(instance: templateMock.Object, Builder);

            // Assert
            templateMock.Verify(x => x.TransformText(), Times.Once);
        }

        private sealed class TextTransformTemplateWithParameters
        {
#pragma warning disable S1172 // Unused method parameters should be removed
            public string TransformText(int someParameter) => string.Empty;
#pragma warning restore S1172 // Unused method parameters should be removed
        }

        private sealed class TextTransformTemplateWithWrongReturnType
        {
            public object TransformText() => string.Empty;
        }
    }
}
