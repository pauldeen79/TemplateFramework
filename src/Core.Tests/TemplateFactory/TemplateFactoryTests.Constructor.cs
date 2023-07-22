namespace TemplateFramework.Core.Tests;

public partial class TemplateFactoryTests
{
    public class Constructor : TemplateFactoryTests
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateFactory(childTemplateCreators: null!))
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
