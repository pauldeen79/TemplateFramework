namespace TemplateFramework.Core.Tests.TemplateInitializerComponents;

public class ModelInitializerTests
{
    protected ModelInitializer CreateSut() => new(new[] { TemplateParameterConverterMock.Object });
    
    protected Mock<ITemplateParameterConverter> TemplateParameterConverterMock { get; } = new();
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();
    
    protected const string DefaultFilename = "DefaultFilename.txt";

    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Converters()
        {
            // Act & Assert
            this.Invoking(_ => new ModelInitializer(converters: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("converters");
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

            // Act
            sut.Initialize(request, TemplateEngineMock.Object);

            // Assert
            template.Model.Should().Be(model);
        }
    }
}
