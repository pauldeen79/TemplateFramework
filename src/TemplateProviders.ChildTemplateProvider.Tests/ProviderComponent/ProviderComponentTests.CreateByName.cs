namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests;

public partial class ProviderComponentTests
{
    public class CreateByName : ProviderComponentTests
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(new TemplateByNameIdentifier(name: null!)))
               .Should().Throw<ArgumentNullException>().WithParameterName("name");
        }

        [Fact]
        public void Throws_When_Model_Is_Not_Supported()
        {
            // Arrange
            var sut = CreateSut();
            TemplateCreatorMock.SupportsName(Arg.Any<string>()).Returns(false);

            // Act & Assert
            sut.Invoking(x => x.Create(new TemplateByNameIdentifier("test")))
               .Should().Throw<NotSupportedException>().WithMessage("Name test is not supported");
        }

        [Fact]
        public void Throws_When_TemplateCreator_Returns_Null_Instance()
        {
            // Arrange
            var sut = CreateSut();
            TemplateCreatorMock.SupportsName(Arg.Any<string>()).Returns(true);
            TemplateCreatorMock.CreateByName(Arg.Any<string>()).Returns(null!);

            // Act & Assert
            sut.Invoking(x => x.Create(new TemplateByNameIdentifier("test")))
               .Should().Throw<InvalidOperationException>().WithMessage("Child template creator returned a null instance");
        }

        [Fact]
        public void Returns_Instance_When_Model_Is_Supported()
        {
            // Arrange
            var sut = CreateSut();
            var template = new object();
            TemplateCreatorMock.SupportsName(Arg.Any<string>()).Returns(x => x.ArgAt<string>(0) == "test");
            TemplateCreatorMock.CreateByName(Arg.Any<string>()).Returns(template);

            // Act
            var result = sut.Create(new TemplateByNameIdentifier("test"));

            // Assert
            result.Should().BeSameAs(template);
        }
    }
}
