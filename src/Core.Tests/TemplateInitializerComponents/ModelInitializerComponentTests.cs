namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class ModelInitializerComponentTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(ModelInitializerComponent).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }

    public class Initialize : ModelInitializerComponentTests
    {
        private const string DefaultFilename = "DefaultFilename.txt";

        [Theory, AutoMockData]
        public void Throws_On_Null_Context(ModelInitializerComponent sut)
        {
            // Act & Assert
            Action a = () => sut.InitializeAsync(context: null!, CancellationToken.None);
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("context");
        }

        [Theory, AutoMockData]
        public async Task Sets_Model_When_Possible(
            [Frozen] ITemplateEngine templateEngine,
            [Frozen] ITemplateProvider templateProvider,
            [Frozen] IValueConverter valueConverter,
            ModelInitializerComponent sut)
        {
            // Arrange
            var model = "Hello world!";
            var template = new TestData.TemplateWithModel<string>(_ => { });
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), model, new StringBuilder(), DefaultFilename);
            var engineContext = new TemplateEngineContext(request, templateEngine, templateProvider, template);
            valueConverter.Convert(Arg.Any<object?>(), Arg.Any<Type>(), Arg.Any<ITemplateEngineContext>()).Returns(x => x.Args()[0]);

            // Act
            await sut.InitializeAsync(engineContext, CancellationToken.None);

            // Assert
            template.Model.ShouldBe(model);
        }
    }
}
