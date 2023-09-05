namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class ModelInitializerTests
{
    protected ModelInitializerComponent CreateSut() => new(ValueConverterMock.Object);
    
    protected Mock<IValueConverter> ValueConverterMock { get; } = new();
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();
    protected Mock<ITemplateProvider> TemplateProviderMock { get; } = new();
    
    protected const string DefaultFilename = "DefaultFilename.txt";

    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Converter()
        {
            // Act & Assert
            this.Invoking(_ => new ModelInitializerComponent(converter: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("converter");
        }
    }

    public class Initialize : ModelInitializerTests
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
        public void Sets_Model_When_Possible()
        {
            // Arrange
            var sut = CreateSut();
            var model = "Hello world!";
            var template = new TestData.TemplateWithModel<string>(_ => { });
            var request = new RenderTemplateRequest(new TemplateInstanceIdentifier(template), model, new StringBuilder(), DefaultFilename);
            var engineContext = new TemplateEngineContext(request, TemplateEngineMock.Object, TemplateProviderMock.Object, template);
            ValueConverterMock.Setup(x => x.Convert(It.IsAny<object?>(), It.IsAny<Type>())).Returns<object?, Type>((value, type) => value);

            // Act
            sut.Initialize(engineContext);

            // Assert
            template.Model.Should().Be(model);
        }
    }
}
