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
        public async Task Throws_On_Null_Context(ModelInitializerComponent sut)
        {
            // Act & Assert
            await sut.Awaiting(x => x.Initialize(context: null!, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().WithParameterName("context");
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
            await sut.Initialize(engineContext, CancellationToken.None);

            // Assert
            template.Model.Should().Be(model);
        }
    }
}
