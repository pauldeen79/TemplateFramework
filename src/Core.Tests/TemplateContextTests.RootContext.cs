namespace TemplateFramework.Core.Tests;

public partial class TemplateContextTests
{
    public class RootContext : TemplateContextTests
    {
        [Fact]
        public void Returns_Same_Instance_When_There_Is_No_ParentContext()
        {
            // Arrange
            var sut = new TemplateContext(EngineMock, ProviderMock, DefaultFilename, new TemplateInstanceIdentifier(this), template: this);

            // Act
            var result = sut.RootContext;

            // Assert
            result.RootContext.Should().BeSameAs(sut);
        }

        [Fact]
        public void Returns_RootContext_One_Level_Deep()
        {
            // Arrange
            var parentTemplateContext = new TemplateContext(EngineMock, ProviderMock, DefaultFilename, new TemplateInstanceIdentifier(this), template: this);
            var sut = new TemplateContext(EngineMock, ProviderMock, DefaultFilename, new TemplateInstanceIdentifier(this), template: this, parentContext: parentTemplateContext);

            // Act
            var result = sut.RootContext;

            // Assert
            result.RootContext.Should().BeSameAs(parentTemplateContext);
        }

        [Fact]
        public void Returns_RootContext_Two_Levels_Deep()
        {
            // Arrange
            var rootTemplateContext = new TemplateContext(EngineMock, ProviderMock, DefaultFilename, new TemplateInstanceIdentifier(this), template: this);
            var parentTemplateContext = new TemplateContext(EngineMock, ProviderMock, DefaultFilename, new TemplateInstanceIdentifier(this), template: this, parentContext: rootTemplateContext);
            var sut = new TemplateContext(EngineMock, ProviderMock, DefaultFilename, new TemplateInstanceIdentifier(this), template: this, parentContext: parentTemplateContext);

            // Act
            var result = sut.RootContext;

            // Assert
            result.RootContext.Should().BeSameAs(rootTemplateContext);
        }
    }
}
