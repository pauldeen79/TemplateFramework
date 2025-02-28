namespace TemplateFramework.Core.Tests;

public partial class ViewModelTemplateParameterConverterTests
{
    public class TryConvert : ViewModelTemplateParameterConverterTests
    {
        [Fact]
        public void Throws_On_Null_Context()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Action a = () => sut.TryConvert(null, Type, context: null!, out _);
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("context");
        }

        [Fact]
        public void Returns_False_When_Value_Is_Null()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.TryConvert(null, Type, Context, out var convertedValue);

            // Assert
            result.ShouldBeFalse();
            convertedValue.ShouldBeNull();
        }

        [Fact]
        public void Returns_False_When_No_ViewModels_Have_Model_Of_The_Correct_Type()
        {
            // Arrange
            var sut = CreateSut();
            var viewModel = new TestData.MyViewModel<TestData.MyModel<string>>();
            ViewModels.Add(viewModel);

            // Act
            var result = sut.TryConvert("some model of the wrong type", Type, Context, out var convertedValue);

            // Assert
            result.ShouldBeFalse();
            convertedValue.ShouldBeNull();
        }

        [Fact]
        public void Returns_True_When_A_ViewModel_Has_Model_Of_The_Correct_Type()
        {
            // Arrange
            var sut = CreateSut();
            var viewModel = new TestData.MyViewModel<TestData.MyModel<string>>();
            ViewModels.Add(Substitute.For<IViewModel>()); // Let's add a ViewModel that does not have a Model property, to cover all code :)
            ViewModels.Add(viewModel);

            // Act
            var result = sut.TryConvert(new TestData.MyModel<string> { Model = "Hello world!" }, Type, Context, out var convertedValue);

            // Assert
            result.ShouldBeTrue();
            convertedValue.ShouldBeOfType<TestData.MyViewModel<TestData.MyModel<string>>>();
            var vm = (TestData.MyViewModel<TestData.MyModel<string>>)convertedValue!;
            vm.Model.ShouldNotBeNull();
            vm.Model!.Model.ShouldBe("Hello world!");
        }
    }
}
