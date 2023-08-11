namespace TemplateFramework.Core.Tests.MultipleContentBuilderTemplateCreators;

public partial class WrappedMultipleCreatorTests
{
    public class TryCreate : WrappedMultipleCreatorTests
    {
        [Fact]
        public void Returns_Wrapped_Instance_When_Instance_Implements_IMultipleContentBuilderTemplate()
        {
            // Arrange
            var templateMock = new Mock<IMultipleContentBuilderTemplate>();
            var sut = CreateSut();

            // Act
            var result = sut.TryCreate(templateMock.Object);

            // Assert
            result.Should().BeOfType<MultipleContentBuilderTemplateWrapper>();
        }

        [Fact]
        public void Returns_Null_When_Instance_Is_Not_Assignable_To_IMultipleContentBuilderTemplate()
        {
            // Arrange
            var template = new object();
            var sut = CreateSut();

            // Act
            var result = sut.TryCreate(template);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Returns_Null_When_Instance_Is_Null()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.TryCreate(instance: null!);

            // Assert
            result.Should().BeNull();
        }
    }
}
