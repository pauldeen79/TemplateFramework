namespace TemplateFramework.Core.Tests;

public partial class TemplateContextTests
{
    public class GetContextByTemplateType : TemplateContextTests
    {
        [Fact]
        public void Returns_Null_When_Template_Type_Could_Not_Be_Found()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetContextByTemplateType<StringBuilder>();

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void Returns_Context_When_Correct_Not_Using_Predicate()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetContextByTemplateType<GetContextByTemplateType>();

            // Assert
            result.ShouldBeSameAs(this);
        }

        [Fact]
        public void Returns_Context_When_Correct_Using_Predicate_Returns_True()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetContextByTemplateType<GetContextByTemplateType>(_ => true);

            // Assert
            result.ShouldBeSameAs(this);
        }

        [Fact]
        public void Returns_Context_When_Correct_Using_Predicate_Returns_False()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetContextByTemplateType<GetContextByTemplateType>(_ => false);

            // Assert
            result.ShouldBe(default);
        }
    }
}
