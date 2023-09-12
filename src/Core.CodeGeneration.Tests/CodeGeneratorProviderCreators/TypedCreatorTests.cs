namespace TemplateFramework.Core.CodeGeneration.Tests.CodeGeneratorProviderCreators;

public class TypedCreatorTests
{
    public class TryCreateInstance
    {
        [Theory, AutoMockData]
        public void Throws_On_Null_Type(TypedCreator sut)
        {
            // Act
            sut.Invoking(x => x.TryCreateInstance(type: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("type");
        }

        [Theory, AutoMockData]
        public void Returns_Instance_When_Type_Implements_ICodeGenerationProvider(TypedCreator sut)
        {
            // Arrange
            var type = typeof(MyGeneratorProvider);

            // Act
            var result = sut.TryCreateInstance(type);

            // Assert
            result.Should().BeOfType<MyGeneratorProvider>();
        }

        [Theory, AutoMockData]
        public void Returns_Null_When_Type_Does_Not_Implement_ICodeGenerationProvider(TypedCreator sut)
        {
            // Arrange
            var type = GetType();

            // Act
            var result = sut.TryCreateInstance(type);

            // Assert
            result.Should().BeNull();
        }
    }
}
