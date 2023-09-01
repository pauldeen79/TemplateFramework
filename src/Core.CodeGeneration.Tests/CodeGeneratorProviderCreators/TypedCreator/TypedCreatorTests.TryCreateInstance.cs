namespace TemplateFramework.Core.CodeGeneration.Tests.CodeGeneratorProviderCreators;

public partial class TypedCreatorTests
{
    public class TryCreateInstance : TypedCreatorTests
    {
        [Fact]
        public void Throws_On_Null_Type()
        {
            // Arrange
            var sut = new TypedCreator();

            // Act
            sut.Invoking(x => x.TryCreateInstance(type: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("type");
        }

        [Fact]
        public void Returns_Instance_When_Type_Implements_ICodeGenerationProvider()
        {
            // Arrange
            var sut = new TypedCreator();
            var type = typeof(MyGeneratorProvider);

            // Act
            var result = sut.TryCreateInstance(type);

            // Assert
            result.Should().BeOfType<MyGeneratorProvider>();
        }
    }
}
