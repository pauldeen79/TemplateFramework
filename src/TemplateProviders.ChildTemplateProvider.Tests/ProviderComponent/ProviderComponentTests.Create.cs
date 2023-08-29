namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests;
    
public partial class ProviderComponentTests
{
    public class Create : ProviderComponentTests
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(request: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("request");
        }

        [Fact]
        public void Throws_On_Unsupported_Request()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(new Mock<ITemplateIdentifier>().Object))
               .Should().Throw<NotSupportedException>();
        }
    }
}
