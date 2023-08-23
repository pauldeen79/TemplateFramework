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
            sut.Invoking(x => x.Create(new ChildTemplateByNameRequest(name: null!)))
               .Should().Throw<ArgumentNullException>().WithParameterName("name");
        }

        [Fact]
        public void Throws_When_Model_Is_Not_Supported()
        {
            // Arrange
            var sut = CreateSut();
            TemplateCreatorMock.Setup(x => x.SupportsName(It.IsAny<string>())).Returns(false);

            // Act & Assert
            sut.Invoking(x => x.Create(new ChildTemplateByNameRequest("test")))
               .Should().Throw<NotSupportedException>().WithMessage("Name test is not supported");
        }

        [Fact]
        public void Throws_When_TemplateCreator_Returns_Null_Instance()
        {
            // Arrange
            var sut = CreateSut();
            TemplateCreatorMock.Setup(x => x.SupportsName(It.IsAny<string>())).Returns(true);
            TemplateCreatorMock.Setup(x => x.CreateByName(It.IsAny<string>())).Returns(null!);

            // Act & Assert
            sut.Invoking(x => x.Create(new ChildTemplateByNameRequest("test")))
               .Should().Throw<InvalidOperationException>().WithMessage("Child template creator returned a null instance");
        }

        [Fact]
        public void Returns_Instance_When_Model_Is_Supported()
        {
            // Arrange
            var sut = CreateSut();
            var template = new object();
            TemplateCreatorMock.Setup(x => x.SupportsName(It.IsAny<string>())).Returns<string>(x => x == "test");
            TemplateCreatorMock.Setup(x => x.CreateByName(It.IsAny<string>())).Returns(template);

            // Act
            var result = sut.Create(new ChildTemplateByNameRequest("test"));

            // Assert
            result.Should().BeSameAs(template);
        }
    }
}
