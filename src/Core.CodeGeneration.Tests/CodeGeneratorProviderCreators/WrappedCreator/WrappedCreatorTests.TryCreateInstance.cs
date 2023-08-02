﻿namespace TemplateFramework.Core.CodeGeneration.Tests.CodeGeneratorProviderCreators;

public partial class WrappedCreatorTests
{
    public class TryCreateInstance : WrappedCreatorTests
    {
        [Fact]
        public void Throws_On_Null_Type()
        {
            // Arrange
            var sut = new WrappedCreator();

            // Act
            sut.Invoking(x => x.TryCreateInstance(type: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("type");
        }

        [Fact]
        public void Returns_Instance_When_Type_Implements_ICodeGenerationProvider()
        {
            // Arrange
            var sut = new WrappedCreator();
            var type = typeof(MyGeneratorProvider);

            // Act
            var result = sut.TryCreateInstance(type);

            // Assert
            result.Should().BeOfType<MyGeneratorProvider>();
        }

        [Fact]
        public void Returns_Null_When_Type_Does_Not_Implement_ICodeGenerationProvider()
        {
            // Arrange
            var sut = new WrappedCreator();
            var type = GetType();

            // Act
            var result = sut.TryCreateInstance(type);

            // Assert
            result.Should().BeNull();
        }
    }
}
