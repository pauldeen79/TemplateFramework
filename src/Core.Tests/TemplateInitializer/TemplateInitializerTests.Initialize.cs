namespace TemplateFramework.Core.Tests;

public partial class TemplateInitializerTests
{
    public class Initialize : TemplateInitializerTests
    {
        [Fact]
        public void Throws_On_Null_Context()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Initialize(context: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Processes_TemplateInitializeComponents_On_Non_Null_Arguments()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Initialize(new TemplateEngineContext(RenderTemplateRequestMock, TemplateEngineMock, TemplateProviderMock, new object()));

            // Assert
            TemplateInitializerComponentMock.Received().Initialize(Arg.Is<ITemplateEngineContext>(x =>
                x.Engine == TemplateEngineMock
                && x.Identifier == RenderTemplateRequestMock.Identifier));
        }
    }
}
