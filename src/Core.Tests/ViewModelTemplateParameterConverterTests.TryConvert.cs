namespace TemplateFramework.Core.Tests;

public partial class ViewModelTemplateParameterConverterTests
{
    public class TryConvert : ViewModelTemplateParameterConverterTests
    {
        [Fact]
        public void Returns_Continue_When_Value_Is_Null()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Convert(null, Type, Context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Continue_When_No_ViewModels_Have_Model_Of_The_Correct_Type()
        {
            // Arrange
            var sut = CreateSut();
            var viewModel = new TestData.MyViewModel<TestData.MyModel<string>>();
            ViewModels.Add(viewModel);

            // Act
            var result = sut.Convert("some model of the wrong type", Type, Context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Ok_When_A_ViewModel_Has_Model_Of_The_Correct_Type()
        {
            // Arrange
            var sut = CreateSut();
            var viewModel = new TestData.MyViewModel<TestData.MyModel<string>>();
            ViewModels.Add(Substitute.For<IViewModel>()); // Let's add a ViewModel that does not have a Model property, to cover all code :)
            ViewModels.Add(viewModel);

            // Act
            var result = sut.Convert(new TestData.MyModel<string> { Model = "Hello world!" }, Type, Context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeOfType<TestData.MyViewModel<TestData.MyModel<string>>>();
            var vm = (TestData.MyViewModel<TestData.MyModel<string>>)result.Value;
            vm.Model.ShouldNotBeNull();
            vm.Model!.Model.ShouldBe("Hello world!");
        }
    }
}
