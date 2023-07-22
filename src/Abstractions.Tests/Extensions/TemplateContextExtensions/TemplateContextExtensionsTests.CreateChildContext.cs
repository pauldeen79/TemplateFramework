namespace TemplateFramework.Abstractions.Tests.Extensions.TemplateContextExtensions;

public partial class TemplateContextExtensionsTests
{
    public class CreateChildContext : TemplateContextExtensionsTests
    {
        [Fact]
        public void With_Template_Argument_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.CreateChildContext(Template);

            // Assert
            sut.Verify(x => x.CreateChildContext(Template, null, null, null), Times.Once);
        }

        [Fact]
        public void With_Template_And_Model_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.CreateChildContext(Template, Model);

            // Assert
            sut.Verify(x => x.CreateChildContext(Template, Model, null, null), Times.Once);
        }
    }
}
