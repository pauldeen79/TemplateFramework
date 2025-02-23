namespace TemplateFramework.Core.CodeGeneration.Tests.CodeGeneratorProviderCreators;

public class TypedCreatorTests : TestBase<TypedCreator>
{
    public class TryCreateInstance : TypedCreatorTests
    {
        [Fact]
        public void Throws_On_Null_Type()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Invoking(x => x.TryCreateInstance(type: null!))
               .ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("type");
        }

        [Fact]
        public void Returns_Instance_When_Type_Implements_ICodeGenerationProvider()
        {
            // Arrange
            var type = typeof(MyGeneratorProvider);
            var sut = CreateSut();

            // Act
            var result = sut.TryCreateInstance(type);

            // Assert
            result.ShouldBeOfType<MyGeneratorProvider>();
        }

        [Fact]
        public void Returns_Null_When_Type_Does_Not_Implement_ICodeGenerationProvider()
        {
            // Arrange
            var type = GetType();
            var sut = CreateSut();

            // Act
            var result = sut.TryCreateInstance(type);

            // Assert
            result.ShouldBeNull();
        }
    }
}
