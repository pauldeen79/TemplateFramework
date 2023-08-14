namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class ModelInitializerTests
{
    protected ModelInitializer CreateSut() => new(ValueConverterMock.Object);
    
    protected Mock<IValueConverter> ValueConverterMock { get; } = new();
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();
    
    protected const string DefaultFilename = "DefaultFilename.txt";

    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Converter()
        {
            // Act & Assert
            this.Invoking(_ => new ModelInitializer(converter: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("converter");
        }
    }

    public class Initialize : ModelInitializerTests
    {
        [Fact]
        public void Sets_Model_When_Possible()
        {
            // Arrange
            var sut = CreateSut();
            var model = "Hello world!";
            var template = new TestData.TemplateWithModel<string>(_ => { });
            var request = new RenderTemplateRequest(template, model, new StringBuilder(), DefaultFilename);
            ValueConverterMock.Setup(x => x.Convert(It.IsAny<object?>(), It.IsAny<Type>())).Returns<object?, Type>((value, type) => value);

            // Act
            sut.Initialize(request, TemplateEngineMock.Object);

            // Assert
            template.Model.Should().Be(model);
        }
    }
}
