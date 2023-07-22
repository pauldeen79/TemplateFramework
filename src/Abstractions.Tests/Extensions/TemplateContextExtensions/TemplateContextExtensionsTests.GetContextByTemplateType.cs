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
            sut.Object.GetContextByTemplateType<string>();

            // Assert
            sut.Verify(x => x.GetContextByTemplateType<string>(null), Times.Once);
        }
    }
}
