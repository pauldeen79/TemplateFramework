namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests;

public partial class ProviderTests
{
    public class Constructor : ProviderTests
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            // Act & Assert
            this.Invoking(_ => new Provider(childTemplateCreators: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("childTemplateCreators");
        }

        [Fact]
        public void Creates_New_Instance_Correctly()
        {
            // Act
            var sut = CreateSut();

            // Assert
            sut.Should().NotBeNull();
        }
    }
}
