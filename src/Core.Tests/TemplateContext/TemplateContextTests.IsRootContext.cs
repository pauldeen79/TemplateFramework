namespace TemplateFramework.Core.Tests;

public partial class TemplateContextTests
{
    public class IsRootContext
    {
        [Fact]
        public void Returns_True_When_There_Is_No_ParentContext()
        {
            // Arrange
            var sut = new TemplateContext(template: this);

            // Act
            var result = sut.IsRootContext;

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_False_When_There_Is_A_ParentContext()
        {
            // Arrange
            var parentTemplateContext = new TemplateContext(template: this);
            var sut = new TemplateContext(template: this, parentContext: parentTemplateContext);

            // Act
            var result = sut.IsRootContext;

            // Assert
            result.Should().BeFalse();
        }
    }
}
