namespace TemplateFramework.Core.Tests.TemplateInitializers;

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
            sut.Invoking(x => x.Initialize<object?>(request: null!, TemplateEngineMock.Object))
               .Should().Throw<ArgumentNullException>().WithParameterName("request");
        }

        [Fact]
        public void Throws_On_Null_Engine()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Initialize(new Mock<IRenderTemplateRequest<Initialize>>().Object, engine: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("engine");
        }
    }
}
