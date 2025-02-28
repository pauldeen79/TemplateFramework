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
        public void Throws_On_Null_Identifier()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Action a = () => sut.Create(identifier: null!);
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("identifier");
        }

        [Fact]
        public void Throws_On_Unsupported_Request()
        {
            // Arrange
            var identifier = Fixture.Create<ITemplateIdentifier>();
            var sut = CreateSut();

            // Act & Assert
            Action a = () => sut.Create(identifier);
            a.ShouldThrow<NotSupportedException>();
        }
    }

    public class CreateByModel : ProviderComponentTests
    {
        [Fact]
        public void Does_Not_Throw_On_Null_Argument()
        {
            // Arrange
            var templateCreator = Fixture.Freeze<ITemplateCreator>();
            templateCreator.SupportsModel(Arg.Any<object?>()).Returns(true);
            templateCreator.CreateByModel(Arg.Any<object?>()).Returns(new object());
            var sut = CreateSut();

            // Act & Assert
            Action a = () => sut.Create(new TemplateByModelIdentifier(null));
            a.ShouldNotThrow();
        }

        [Fact]
        public void Throws_When_Model_Not_Null_Is_Not_Supported()
        {
            // Arrange
            var templateCreator = Fixture.Freeze<ITemplateCreator>();
            templateCreator.SupportsModel(Arg.Any<object?>()).Returns(false);
            var sut = CreateSut();

            // Act & Assert
            Action a = () => sut.Create(new TemplateByModelIdentifier(1));
            a.ShouldThrow<NotSupportedException>().Message.ShouldBe("Model of type System.Int32 is not supported");
        }

        [Fact]
        public void Throws_When_Model_Null_Is_Not_Supported()
        {
            // Arrange
            var templateCreator = Fixture.Freeze<ITemplateCreator>();
            templateCreator.SupportsModel(Arg.Any<object?>()).Returns(false);
            var sut = CreateSut();

            // Act & Assert
            Action a = () => sut.Create(new TemplateByModelIdentifier(null));
            a.ShouldThrow<NotSupportedException>().Message.ShouldBe("Model of type  is not supported");
        }

        [Fact]
        public void Throws_When_TemplateCreator_Returns_Null_Instance()
        {
            // Arrange
            var templateCreator = Fixture.Freeze<ITemplateCreator>();
            templateCreator.SupportsModel(Arg.Any<object?>()).Returns(true);
            templateCreator.CreateByModel(Arg.Any<object?>()).Returns(null!);
            var sut = CreateSut();

            // Act & Assert
            Action a = () => sut.Create(new TemplateByModelIdentifier(null!));
            a.ShouldThrow<InvalidOperationException>().Message.ShouldBe("Child template creator returned a null instance");
        }

        [Fact]
        public void Returns_Instance_When_Model_Is_Supported()
        {
            // Arrange
            var template = new object();
            var templateCreator = Fixture.Freeze<ITemplateCreator>();
            templateCreator.SupportsModel(Arg.Any<object?>()).Returns<object>(x => x.Args()[0] is int);
            templateCreator.CreateByModel(Arg.Any<object?>()).Returns(template);
            var sut = CreateSut();

            // Act
            var result = sut.Create(new TemplateByModelIdentifier(1));

            // Assert
            result.ShouldBeSameAs(template);
        }
    }

    public class CreateByName : ProviderComponentTests
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            // Arrange
            var sut = CreateSut();
            // Act & Assert
            Action a = () => sut.Create(new TemplateByNameIdentifier(name: null!));
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("name");
        }

        [Fact]
        public void Throws_When_Model_Is_Not_Supported()
        {
            // Arrange
            var templateCreator = Fixture.Freeze<ITemplateCreator>();
            templateCreator.SupportsName(Arg.Any<string>()).Returns(false);
            var sut = CreateSut();

            // Act & Assert
            Action a = () => sut.Create(new TemplateByNameIdentifier("test"));
            a.ShouldThrow<NotSupportedException>().Message.ShouldBe("Template with name test is not supported");
        }

        [Fact]
        public void Throws_When_TemplateCreator_Returns_Null_Instance()
        {
            // Arrange
            var templateCreator = Fixture.Freeze<ITemplateCreator>();
            templateCreator.SupportsName(Arg.Any<string>()).Returns(true);
            templateCreator.CreateByName(Arg.Any<string>()).Returns(null!);
            var sut = CreateSut();

            // Act & Assert
            Action a = () => sut.Create(new TemplateByNameIdentifier("test"));
            a.ShouldThrow<InvalidOperationException>().Message.ShouldBe("Child template creator returned a null instance");
        }

        [Fact]
        public void Returns_Instance_When_Model_Is_Supported()
        {
            // Arrange
            var template = new object();
            var templateCreator = Fixture.Freeze<ITemplateCreator>();
            templateCreator.SupportsName(Arg.Any<string>()).Returns(x => x.ArgAt<string>(0) == "test");
            templateCreator.CreateByName(Arg.Any<string>()).Returns(template);
            var sut = CreateSut();

            // Act
            var result = sut.Create(new TemplateByNameIdentifier("test"));

            // Assert
            result.ShouldBeSameAs(template);
        }
    }

    public class Supports : ProviderComponentTests
    {
        [Fact]
        public void Returns_False_On_Null_Identifier()
        {
            // Arrange
            var identifier = (ITemplateIdentifier)null!;
            var sut = CreateSut();

            // Act
            var result = sut.Supports(identifier);

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void Returns_False_On_Unsupported_Identifier()
        {
            // Arrange
            var identifier = Fixture.Create<ITemplateIdentifier>();
            var sut = CreateSut();

            // Act
            var result = sut.Supports(identifier);

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void Returns_True_On_CreateTemplateByModelRequest()
        {
            // Arrange
            var identifier = new TemplateByModelIdentifier(this);
            var templateCreator = Fixture.Freeze<ITemplateCreator>();
            templateCreator.SupportsModel(Arg.Any<object?>()).Returns(true);
            var sut = CreateSut();

            // Act
            var result = sut.Supports(identifier);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void Returns_True_On_CreateTemplateByNameRequest()
        {
            // Arrange
            var identifier = new TemplateByNameIdentifier(nameof(Returns_True_On_CreateTemplateByNameRequest));
            var templateCreator = Fixture.Freeze<ITemplateCreator>();
            templateCreator.SupportsName(Arg.Any<string>()).Returns(true);
            var sut = CreateSut();

            // Act
            var result = sut.Supports(identifier);

            // Assert
            result.ShouldBeTrue();
        }
    }
}
