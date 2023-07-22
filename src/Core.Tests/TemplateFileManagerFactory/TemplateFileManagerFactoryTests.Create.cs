namespace TemplateFramework.Core.Tests;

public class TemplateFileManagerFactoryTests
{
    public class Create
    {
        [Fact]
        public void Creates_Instance()
        {
            // Arrange
            var sut = new TemplateFileManagerFactory();

            // Act
            var instance = sut.Create();

            // Assert
            instance.Should().NotBeNull();
        }
    }
}
