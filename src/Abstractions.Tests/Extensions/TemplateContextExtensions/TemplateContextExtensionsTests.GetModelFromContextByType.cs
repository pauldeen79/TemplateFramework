namespace TemplateFramework.Abstractions.Tests.Extensions.TemplateContextExtensions;

public partial class TemplateContextExtensionsTests
{
    public class GetModelFromContextByType : TemplateContextExtensionsTests
    {
        [Fact]
        public void Without_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.GetModelFromContextByType<string>();

            // Assert
            sut.Verify(x => x.GetModelFromContextByType<string>(null), Times.Once);
        }
    }
}
