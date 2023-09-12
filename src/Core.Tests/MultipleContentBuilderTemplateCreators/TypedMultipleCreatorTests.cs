namespace TemplateFramework.Core.Tests.MultipleContentBuilderTemplateCreators;

public class TypedMultipleCreatorTests
{
    public class TryCreate : TypedMultipleCreatorTests
    {
        [Theory, AutoMockData]
        public void Returns_Instance_When_Instance_Is_Assignable_To_IMultipleContentBuilderTemplate(
            [Frozen] IMultipleContentBuilderTemplate multipleContentBuilderTemplate,
            TypedMultipleCreator sut)
        {
            // Act
            var result = sut.TryCreate(multipleContentBuilderTemplate);

            // Assert
            result.Should().BeSameAs(multipleContentBuilderTemplate);
        }

        [Theory, AutoMockData]
        public void Returns_Null_When_Instance_Is_Not_Assignable_To_IMultipleContentBuilderTemplate(TypedMultipleCreator sut)
        {
            // Arrange
            var template = new object();

            // Act
            var result = sut.TryCreate(template);

            // Assert
            result.Should().BeNull();
        }

        [Theory, AutoMockData]
        public void Returns_Null_When_Instance_Is_Null(TypedMultipleCreator sut)
        {
            // Act
            var result = sut.TryCreate(instance: null!);

            // Assert
            result.Should().BeNull();
        }
    }
}
