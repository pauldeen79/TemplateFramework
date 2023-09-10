namespace TemplateFramework.Core.Tests.MultipleContentBuilderTemplateCreators;

public partial class TypedMultipleCreatorTests
{
    public class TryCreate : TypedMultipleCreatorTests
    {
        [Fact]
        public void Returns_Instance_When_Instance_Is_Assignable_To_IMultipleContentBuilderTemplate()
        {
            // Arrange
            var templateMock = Substitute.For<IMultipleContentBuilderTemplate>();
            var sut = CreateSut();

            // Act
            var result = sut.TryCreate(templateMock);

            // Assert
            result.Should().BeSameAs(templateMock);
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
