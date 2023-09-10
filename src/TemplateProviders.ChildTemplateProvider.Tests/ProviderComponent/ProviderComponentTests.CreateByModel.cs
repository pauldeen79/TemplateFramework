namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests;

public partial class ProviderComponentTests
{
    public class CreateByModel : ProviderComponentTests
    {
        [Fact]
        public void Does_Not_Throw_On_Null_Argument()
        {
            // Arrange
            var sut = CreateSut();
            TemplateCreatorMock.SupportsModel(Arg.Any<object?>()).Returns(true);
            TemplateCreatorMock.CreateByModel(Arg.Any<object?>()).Returns(new object());

            // Act & Assert
            sut.Invoking(x => x.Create(new TemplateByModelIdentifier(null)))
               .Should().NotThrow();
        }

        [Fact]
        public void Throws_When_Model_Not_Null_Is_Not_Supported()
        {
            // Arrange
            var sut = CreateSut();
            TemplateCreatorMock.SupportsModel(Arg.Any<object?>()).Returns(false);

            // Act & Assert
            sut.Invoking(x => x.Create(new TemplateByModelIdentifier(1)))
               .Should().Throw<NotSupportedException>().WithMessage("Model of type System.Int32 is not supported");
        }

        [Fact]
        public void Throws_When_Model_Null_Is_Not_Supported()
        {
            // Arrange
            var sut = CreateSut();
            TemplateCreatorMock.SupportsModel(Arg.Any<object?>()).Returns(false);

            // Act & Assert
            sut.Invoking(x => x.Create(new TemplateByModelIdentifier(null)))
               .Should().Throw<NotSupportedException>().WithMessage("Model of type  is not supported");
        }

        [Fact]
        public void Throws_When_TemplateCreator_Returns_Null_Instance()
        {
            // Arrange
            var sut = CreateSut();
            TemplateCreatorMock.SupportsModel(Arg.Any<object?>()).Returns(true);
            TemplateCreatorMock.CreateByModel(Arg.Any<object?>()).Returns(null!);

            // Act & Assert
            sut.Invoking(x => x.Create(new TemplateByModelIdentifier(null!)))
               .Should().Throw<InvalidOperationException>().WithMessage("Child template creator returned a null instance");
        }

        [Fact]
        public void Returns_Instance_When_Model_Is_Supported()
        {
            // Arrange
            var sut = CreateSut();
            var template = new object();
            TemplateCreatorMock.SupportsModel(Arg.Any<object?>()).Returns<object>(x => x.Args()[0] is int);
            TemplateCreatorMock.CreateByModel(Arg.Any<object?>()).Returns(template);

            // Act
            var result = sut.Create(new TemplateByModelIdentifier(1));

            // Assert
            result.Should().BeSameAs(template);
        }
    }
}
