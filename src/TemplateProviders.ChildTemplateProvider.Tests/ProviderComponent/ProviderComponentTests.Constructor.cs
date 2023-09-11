namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests;

public partial class ProviderComponentTests
{
    public class Constructor : ProviderComponentTests
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            typeof(ProviderComponent).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
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
