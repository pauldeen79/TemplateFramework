namespace TemplateFramework.Core.Tests;

public class TemplateInitializerTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(TemplateInitializer).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }

    public class Initialize : TemplateInitializerTests
    {
        [Theory, AutoMockData]
        public void Throws_On_Null_Context(TemplateInitializer sut)
        {
            // Act & Assert
            sut.Invoking(x => x.Initialize(context: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Theory, AutoMockData]
        public void Processes_TemplateInitializeComponents_On_Non_Null_Arguments(
            [Frozen] ITemplateEngineContext templateEngineContext,
            [Frozen] ITemplateInitializerComponent templateInitializerComponent,
            TemplateInitializer sut)
        {
            // Act
            sut.Initialize(templateEngineContext);

            // Assert
            templateInitializerComponent.Received().Initialize(Arg.Any<ITemplateEngineContext>());
        }
    }
}
