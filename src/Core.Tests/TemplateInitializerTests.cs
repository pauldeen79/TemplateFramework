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

    public class InitializeAsync : TemplateInitializerTests
    {
        [Theory, AutoMockData]
        public async Task Throws_On_Null_Context(TemplateInitializer sut)
        {
            // Act & Assert
            Task t = sut.InitializeAsync(context: null!, CancellationToken.None);
            (await t.ShouldThrowAsync<ArgumentNullException>()).ParamName.ShouldBe("context");
        }

        [Theory, AutoMockData]
        public async Task Processes_TemplateInitializeComponents_On_Non_Null_Arguments(
            [Frozen] ITemplateEngineContext templateEngineContext,
            [Frozen] ITemplateInitializerComponent templateInitializerComponent,
            TemplateInitializer sut)
        {
            // Act
            await sut.InitializeAsync(templateEngineContext, CancellationToken.None);

            // Assert
            await templateInitializerComponent.Received().InitializeAsync(Arg.Any<ITemplateEngineContext>(), Arg.Any<CancellationToken>());
        }
    }
}
