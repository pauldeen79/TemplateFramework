namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests;

public class ProviderComponentTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(ProviderComponent).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }

    public class Create
    {
        [Theory, AutoMockData]
        public void Throws_On_Null_Identifier(ProviderComponent sut)
        {
            // Act & Assert
            sut.Invoking(x => x.Create(identifier: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("identifier");
        }

        [Theory, AutoMockData]
        public void Throws_On_Unsupported_Request(
            [Frozen] ITemplateIdentifier identifier,
            ProviderComponent sut)
        {
            // Act & Assert
            sut.Invoking(x => x.Create(identifier))
               .Should().Throw<NotSupportedException>();
        }
    }

    public class CreateByModel
    {
        [Theory, AutoMockData]
        public void Does_Not_Throw_On_Null_Argument(
            [Frozen] ITemplateCreator templateCreator,
            ProviderComponent sut)
        {
            // Arrange
            templateCreator.SupportsModel(Arg.Any<object?>()).Returns(true);
            templateCreator.CreateByModel(Arg.Any<object?>()).Returns(new object());

            // Act & Assert
            sut.Invoking(x => x.Create(new TemplateByModelIdentifier(null)))
               .Should().NotThrow();
        }

        [Theory, AutoMockData]
        public void Throws_When_Model_Not_Null_Is_Not_Supported(
            [Frozen] ITemplateCreator templateCreator, 
            ProviderComponent sut)
        {
            // Arrange
            templateCreator.SupportsModel(Arg.Any<object?>()).Returns(false);

            // Act & Assert
            sut.Invoking(x => x.Create(new TemplateByModelIdentifier(1)))
               .Should().Throw<NotSupportedException>().WithMessage("Model of type System.Int32 is not supported");
        }

        [Theory, AutoMockData]
        public void Throws_When_Model_Null_Is_Not_Supported(
            [Frozen] ITemplateCreator templateCreator,
            ProviderComponent sut)
        {
            // Arrange
            templateCreator.SupportsModel(Arg.Any<object?>()).Returns(false);

            // Act & Assert
            sut.Invoking(x => x.Create(new TemplateByModelIdentifier(null)))
               .Should().Throw<NotSupportedException>().WithMessage("Model of type  is not supported");
        }

        [Theory, AutoMockData]
        public void Throws_When_TemplateCreator_Returns_Null_Instance(
            [Frozen] ITemplateCreator templateCreator, 
            ProviderComponent sut)
        {
            // Arrange
            templateCreator.SupportsModel(Arg.Any<object?>()).Returns(true);
            templateCreator.CreateByModel(Arg.Any<object?>()).Returns(null!);

            // Act & Assert
            sut.Invoking(x => x.Create(new TemplateByModelIdentifier(null!)))
               .Should().Throw<InvalidOperationException>().WithMessage("Child template creator returned a null instance");
        }

        [Theory, AutoMockData]
        public void Returns_Instance_When_Model_Is_Supported(
            [Frozen] ITemplateCreator templateCreator, 
            ProviderComponent sut)
        {
            // Arrange
            var template = new object();
            templateCreator.SupportsModel(Arg.Any<object?>()).Returns<object>(x => x.Args()[0] is int);
            templateCreator.CreateByModel(Arg.Any<object?>()).Returns(template);

            // Act
            var result = sut.Create(new TemplateByModelIdentifier(1));

            // Assert
            result.Should().BeSameAs(template);
        }
    }

    public class CreateByName
    {
        [Theory, AutoMockData]
        public void Throws_On_Null_Argument(ProviderComponent sut)
        {
            // Act & Assert
            sut.Invoking(x => x.Create(new TemplateByNameIdentifier(name: null!)))
               .Should().Throw<ArgumentNullException>().WithParameterName("name");
        }

        [Theory, AutoMockData]
        public void Throws_When_Model_Is_Not_Supported(
            [Frozen] ITemplateCreator templateCreator, 
            ProviderComponent sut)
        {
            // Arrange
            templateCreator.SupportsName(Arg.Any<string>()).Returns(false);

            // Act & Assert
            sut.Invoking(x => x.Create(new TemplateByNameIdentifier("test")))
               .Should().Throw<NotSupportedException>().WithMessage("Name test is not supported");
        }

        [Theory, AutoMockData]
        public void Throws_When_TemplateCreator_Returns_Null_Instance(
            [Frozen] ITemplateCreator templateCreator, 
            ProviderComponent sut)
        {
            // Arrange
            templateCreator.SupportsName(Arg.Any<string>()).Returns(true);
            templateCreator.CreateByName(Arg.Any<string>()).Returns(null!);

            // Act & Assert
            sut.Invoking(x => x.Create(new TemplateByNameIdentifier("test")))
               .Should().Throw<InvalidOperationException>().WithMessage("Child template creator returned a null instance");
        }

        [Theory, AutoMockData]
        public void Returns_Instance_When_Model_Is_Supported(
            [Frozen] ITemplateCreator templateCreator, 
            ProviderComponent sut)
        {
            // Arrange
            var template = new object();
            templateCreator.SupportsName(Arg.Any<string>()).Returns(x => x.ArgAt<string>(0) == "test");
            templateCreator.CreateByName(Arg.Any<string>()).Returns(template);

            // Act
            var result = sut.Create(new TemplateByNameIdentifier("test"));

            // Assert
            result.Should().BeSameAs(template);
        }
    }

    public class Supports
    {
        [Theory, AutoMockData]
        public void Returns_False_On_Null_Identifier(ProviderComponent sut)
        {
            // Arrange
            var identifier = (ITemplateIdentifier)null!;

            // Act
            var result = sut.Supports(identifier);

            // Assert
            result.Should().BeFalse();
        }

        [Theory, AutoMockData]
        public void Returns_False_On_Unsupported_Identifier(
            [Frozen] ITemplateIdentifier identifier,
            ProviderComponent sut)
        {
            // Act
            var result = sut.Supports(identifier);

            // Assert
            result.Should().BeFalse();
        }

        [Theory, AutoMockData]
        public void Returns_True_On_CreateTemplateByModelRequest(
            [Frozen] ITemplateCreator templateCreator, 
            ProviderComponent sut)
        {
            // Arrange
            var identifier = new TemplateByModelIdentifier(this);
            templateCreator.SupportsModel(Arg.Any<object?>()).Returns(true);

            // Act
            var result = sut.Supports(identifier);

            // Assert
            result.Should().BeTrue();
        }

        [Theory, AutoMockData]
        public void Returns_True_On_CreateTemplateByNameRequest(
            [Frozen] ITemplateCreator templateCreator, 
            ProviderComponent sut)
        {
            // Arrange
            var identifier = new TemplateByNameIdentifier(nameof(Returns_True_On_CreateTemplateByNameRequest));
            templateCreator.SupportsName(Arg.Any<string>()).Returns(true);

            // Act
            var result = sut.Supports(identifier);

            // Assert
            result.Should().BeTrue();
        }
    }
}
