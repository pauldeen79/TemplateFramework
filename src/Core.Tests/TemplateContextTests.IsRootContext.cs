namespace TemplateFramework.Core.Tests;

public partial class TemplateContextTests
{
    public class IsRootContext : TemplateContextTests
    {
        [Fact]
        public void Returns_True_When_There_Is_No_ParentContext()
        {
            // Arrange
            var sut = new TemplateContext(EngineMock, ProviderMock, DefaultFilename, new TemplateInstanceIdentifier(this), template: this);

            // Act
            var result = sut.IsRootContext;

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void Returns_False_When_There_Is_A_ParentContext()
        {
            // Arrange
            var parentTemplateContext = new TemplateContext(EngineMock, ProviderMock, DefaultFilename, new TemplateInstanceIdentifier(this), template: this);
            var sut = new TemplateContext(EngineMock, ProviderMock, DefaultFilename, new TemplateInstanceIdentifier(this), template: this, parentContext: parentTemplateContext);

            // Act
            var result = sut.IsRootContext;

            // Assert
            result.ShouldBeFalse();
        }
    }
}
