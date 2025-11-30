namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests;

public class ProviderComponentTests : TestBase<ProviderComponent>
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(ProviderComponent).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }

    public class Create : ProviderComponentTests
    {
        [Fact]
        public void Returns_Continue_On_Null_Identifier()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Create(identifier: null!);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue); // note that the caller has already validated this, so we can simply ignore it
        }

        [Fact]
        public void Returns_Continue_On_Unsupported_Request()
        {
            // Arrange
            var identifier = Fixture.Create<ITemplateIdentifier>();
            var sut = CreateSut();

            // Act
            var result = sut.Create(identifier);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }
    }

    public class CreateByModel : ProviderComponentTests
    {
        [Fact]
        public void Returns_NotSupported_When_Model_Is_Not_Supported()
        {
            // Arrange
            var templateCreator = Fixture.Freeze<ITemplateCreator>();
            templateCreator.CreateByName(Arg.Any<string>()).Returns(Result.Continue<object>());
            templateCreator.CreateByModel(Arg.Any<object?>()).Returns(Result.Continue<object>());
            var sut = CreateSut();

            // Act
            var result = sut.Create(new TemplateByModelIdentifier(1));

            // Assert
            result.Status.ShouldBe(ResultStatus.NotSupported);
            result.ErrorMessage.ShouldBe("Model of type System.Int32 is not supported");
        }

        [Fact]
        public void Returns_Error_When_TemplateCreator_Returns_Null_Instance()
        {
            // Arrange
            var templateCreator = Fixture.Freeze<ITemplateCreator>();
            templateCreator.CreateByModel(Arg.Any<object?>()).Returns(default(Result<object>));
            var sut = CreateSut();

            // Act
            var result = sut.Create(new TemplateByModelIdentifier(null!));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Child template creator returned a null instance");
        }

        [Fact]
        public void Returns_Instance_When_Model_Is_Supported()
        {
            // Arrange
            var template = new object();
            var templateCreator = Fixture.Freeze<ITemplateCreator>();
            templateCreator.CreateByModel(Arg.Any<object?>()).Returns(Result.Success(template));
            var sut = CreateSut();

            // Act
            var result = sut.Create(new TemplateByModelIdentifier(1));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeSameAs(template);
        }
    }

    public class CreateByName : ProviderComponentTests
    {
        [Fact]
        public void Returns_NotSupported_When_Model_Is_Not_Supported()
        {
            // Arrange
            var templateCreator = Fixture.Freeze<ITemplateCreator>();
            templateCreator.CreateByName(Arg.Any<string>()).Returns(Result.Continue<object>());
            var sut = CreateSut();

            // Act
            var result = sut.Create(new TemplateByNameIdentifier("test"));

            // Assert
            result.Status.ShouldBe(ResultStatus.NotSupported);
            result.ErrorMessage.ShouldBe("Template with name test is not supported");
        }

        [Fact]
        public void Returns_Error_When_TemplateCreator_Returns_Null_Instance()
        {
            // Arrange
            var templateCreator = Fixture.Freeze<ITemplateCreator>();
            templateCreator.CreateByName(Arg.Any<string>()).Returns(default(Result<object>));
            var sut = CreateSut();

            // Act
            var result = sut.Create(new TemplateByNameIdentifier("test"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Child template creator returned a null instance");
        }

        [Fact]
        public void Returns_Instance_When_Model_Is_Supported()
        {
            // Arrange
            var template = new object();
            var templateCreator = Fixture.Freeze<ITemplateCreator>();
            templateCreator.CreateByName(Arg.Any<string>()).Returns(Result.Success(template));
            var sut = CreateSut();

            // Act
            var result = sut.Create(new TemplateByNameIdentifier("test"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeSameAs(template);
        }
    }
}
