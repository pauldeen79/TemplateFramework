namespace TemplateFramework.Core.Tests;

public partial class TemplateContextTests
{
    public class GetModelFromContextByType : TemplateContextTests
    {
        [Fact]
        public void Returns_Null_When_ViewModel_Type_Could_Not_Be_Found()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetModelFromContextByType<GetModelFromContextByType>();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Returns_Model_From_Parent_When_Correct_Not_Using_Predicate()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetModelFromContextByType<string>();

            // Assert
            result.Should().Be("test model");
        }

        [Fact]
        public void Returns_Model_From_Root_When_Correct_Using_Predicate_Returns_True()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetModelFromContextByType<int>(_ => true);

            // Assert
            result.Should().Be(1);
        }

        [Fact]
        public void Returns_Model_From_Root_When_Correct_Using_Predicate_Returns_False()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetModelFromContextByType<int>(_ => false);

            // Assert
            result.Should().Be(default);
        }
    }
}
