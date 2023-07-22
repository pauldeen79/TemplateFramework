namespace TemplateFramework.Core.Tests;

public partial class TemplateFactoryTests
{
    public class CreateByModel : TemplateFactoryTests
    {
        [Fact]
        public void Does_Not_Throw_On_Null_Argument()
        {
            // Arrange
            var sut = CreateSut();
            TemplateCreatorMock.Setup(x => x.SupportsModel(It.IsAny<object?>())).Returns(true);
            TemplateCreatorMock.Setup(x => x.CreateByModel(It.IsAny<object?>())).Returns(new object());

            // Act & Assert
            sut.Invoking(x => x.CreateByModel(null!))
               .Should().NotThrow();
        }

        [Fact]
        public void Throws_When_Model_Not_Null_Is_Not_Supported()
        {
            // Arrange
            var sut = CreateSut();
            TemplateCreatorMock.Setup(x => x.SupportsModel(It.IsAny<object?>())).Returns(false);

            // Act & Assert
            sut.Invoking(x => x.CreateByModel(1))
               .Should().Throw<NotSupportedException>().WithMessage("Model of type System.Int32 is not supported");
        }

        [Fact]
        public void Throws_When_Model_Null_Is_Not_Supported()
        {
            // Arrange
            var sut = CreateSut();
            TemplateCreatorMock.Setup(x => x.SupportsModel(It.IsAny<object?>())).Returns(false);

            // Act & Assert
            sut.Invoking(x => x.CreateByModel(null))
               .Should().Throw<NotSupportedException>().WithMessage("Model of type  is not supported");
        }

        [Fact]
        public void Throws_When_TemplateCreator_Returns_Null_Instance()
        {
            // Arrange
            var sut = CreateSut();
            TemplateCreatorMock.Setup(x => x.SupportsModel(It.IsAny<object?>())).Returns(true);
            TemplateCreatorMock.Setup(x => x.CreateByModel(It.IsAny<object?>())).Returns(null!);

            // Act & Assert
            sut.Invoking(x => x.CreateByModel(null!))
               .Should().Throw<InvalidOperationException>().WithMessage("Child template creator returned a null instance");
        }

        [Fact]
        public void Returns_Instance_When_Model_Is_Supported()
        {
            // Arrange
            var sut = CreateSut();
            var template = new object();
            TemplateCreatorMock.Setup(x => x.SupportsModel(It.IsAny<object?>())).Returns<object>(x => x is int);
            TemplateCreatorMock.Setup(x => x.CreateByModel(It.IsAny<object?>())).Returns(template);

            // Act
            var result = sut.CreateByModel(1);

            // Assert
            result.Should().BeSameAs(template);
        }
    }
}
