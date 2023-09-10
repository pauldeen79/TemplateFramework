namespace TemplateFramework.Abstractions.Tests.Extensions.TemplateContextExtensions;

public partial class TemplateContextExtensionsTests
{
    public class GetContextByTemplateType : TemplateContextExtensionsTests
    {
        [Fact]
        public void Without_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.GetContextByTemplateType<string>();

            // Assert
            sut.Received().GetContextByTemplateType<string>(null);
        }
    }
}
