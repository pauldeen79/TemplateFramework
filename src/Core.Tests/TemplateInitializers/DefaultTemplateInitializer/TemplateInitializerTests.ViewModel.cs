namespace TemplateFramework.Core.Tests.TemplateInitializers;

public partial class TemplateInitializerTests
{
    public class ViewModel : TemplateInitializerTests
    {
        [Fact]
        public void Can_Inject_ViewModel_On_Template_Using_AdditionalParameters()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TemplateWithViewModel<TestData.NonConstructableViewModel>(_ => { });
            var viewModel = new TestData.NonConstructableViewModel("Some value");
            var request = new RenderTemplateRequest(template, new StringBuilder(), DefaultFilename, additionalParameters: new { ViewModel = viewModel });

            // Act
            sut.Initialize(request, TemplateEngineMock.Object);

            // Assert
            template.ViewModel.Should().BeSameAs(viewModel);
        }
    }
}
