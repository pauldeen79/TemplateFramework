namespace TemplateFramework.Core.Tests;

public partial class TemplateInitializerTests
{
    public class Initialize : TemplateInitializerTests
    {
        [Fact]
        public void Throws_On_Null_Request()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Initialize(request: null!, TemplateEngineMock.Object))
               .Should().Throw<ArgumentNullException>().WithParameterName("request");
        }

        [Fact]
        public void Throws_On_Null_Engine()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Initialize(RenderTemplateRequestMock.Object, engine: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("engine");
        }

        [Fact]
        public void Processes_TemplateInitializeComponents_On_Non_Null_Arguments()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Initialize(RenderTemplateRequestMock.Object, TemplateEngineMock.Object);

            // Assert
            TemplateInitializerComponentMock.Verify(x => x.Initialize(RenderTemplateRequestMock.Object, TemplateEngineMock.Object), Times.Once);
        }
    }
}
