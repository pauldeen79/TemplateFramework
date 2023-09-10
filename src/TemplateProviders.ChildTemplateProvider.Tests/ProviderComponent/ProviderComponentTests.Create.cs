namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests;
    
public partial class ProviderComponentTests
{
    public class Create : ProviderComponentTests
    {
        [Fact]
        public void Throws_On_Null_Identifier()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(identifier: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("identifier");
        }

        [Fact]
        public void Throws_On_Unsupported_Request()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(Substitute.For<ITemplateIdentifier>()))
               .Should().Throw<NotSupportedException>();
        }
    }
}
