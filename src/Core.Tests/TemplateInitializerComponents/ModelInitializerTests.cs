namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class ModelInitializerTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(ModelInitializerComponent).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }

    public class Initialize : ModelInitializerTests
    {
        private const string DefaultFilename = "DefaultFilename.txt";

        [Theory, AutoMockData]
        public void Throws_On_Null_Context(ModelInitializerComponent sut)
        {
            // Act & Assert
            sut.Invoking(x => x.Initialize(context: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Theory, AutoMockData]
        public void Sets_Model_When_Possible(
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
            valueConverter.Convert(Arg.Any<object?>(), Arg.Any<Type>()).Returns(x => x.Args()[0]);

            // Act
            sut.Initialize(engineContext);

            // Assert
            template.Model.Should().Be(model);
        }
    }
}
