namespace TemplateFramework.Core.Tests.MultipleContentBuilderTemplateCreators;

public class TypedMultipleCreatorTests
{
    public class TryCreate : TypedMultipleCreatorTests
    {
        [Theory, AutoMockData]
        public void Returns_Instance_When_Instance_Is_Assignable_To_IMultipleContentBuilderTemplate(
            [Frozen] IMultipleContentBuilderTemplate multipleContentBuilderTemplate,
            TypedMultipleCreator<StringBuilder> sut)
        {
            // Act
            var result = sut.TryCreate(multipleContentBuilderTemplate);

            // Assert
            result.ShouldBeSameAs(multipleContentBuilderTemplate);
        }

        [Theory, AutoMockData]
        public void Returns_Null_When_Instance_Is_Not_Assignable_To_IMultipleContentBuilderTemplate(TypedMultipleCreator<StringBuilder> sut)
        {
            // Arrange
            var template = new object();

            // Act
            var result = sut.TryCreate(template);

            // Assert
            result.ShouldBeNull();
        }

        [Theory, AutoMockData]
        public void Returns_Null_When_Instance_Is_Null(TypedMultipleCreator<StringBuilder> sut)
        {
            // Act
            var result = sut.TryCreate(instance: null!);

            // Assert
            result.ShouldBeNull();
        }
    }
}
