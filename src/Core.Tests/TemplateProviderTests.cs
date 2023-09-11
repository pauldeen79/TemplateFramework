namespace TemplateFramework.Core.Tests;

public class TemplateProviderTests
{
    protected TemplateProvider CreateSut() => new(new[] { TemplateProviderComponentMock });

    protected ITemplateProviderComponent TemplateProviderComponentMock { get; } = Substitute.For<ITemplateProviderComponent>();

    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            typeof(TemplateProvider).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }

    public class Create : TemplateProviderTests
    {
        [Fact]
        public void Throws_On_Null_Identifier()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(identifier: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("identifier");
        }

        [Fact]
        public void Throws_On_Unsupported_Type()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(identifier: Substitute.For<ITemplateIdentifier>()))
               .Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void Returns_Template_Instance_From_Component_On_Supported_Type()
        {
            // Arrange
            var sut = CreateSut();
            var identifier = Substitute.For<ITemplateIdentifier>();
            var expectedTemplate = new object();
            TemplateProviderComponentMock.Supports(identifier).Returns(true);
            TemplateProviderComponentMock.Create(identifier).Returns(expectedTemplate);

            // Act
            var template = sut.Create(identifier);

            // Assert
            template.Should().BeSameAs(expectedTemplate);
        }
    }

    public class RegisterComponent : TemplateProviderTests
    {
        [Fact]
        public void Throws_On_Null_Component()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.RegisterComponent(component: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("component");
        }

        [Fact]
        public void Adds_Component_Registration()
        {
            // Arrange
            var sut = CreateSut();
            var customTemplateProviderComponentMock = Substitute.For<ITemplateProviderComponent>();
            var identifier = Substitute.For<ITemplateIdentifier>();
            var expectedTemplate = new object();
            customTemplateProviderComponentMock.Supports(identifier).Returns(true);
            customTemplateProviderComponentMock.Create(identifier).Returns(expectedTemplate);

            // Act
            sut.RegisterComponent(customTemplateProviderComponentMock);

            // Assert
            sut.Create(identifier);
            customTemplateProviderComponentMock.Received().Create(identifier);
        }
    }

    public class StartSession : TemplateProviderTests
    {
        [Fact]
        public void Clears_Registration_Performed_On_Current_Instance()
        {
            // Arrange
            var sut = CreateSut();
            var newTemplateProviderComponentMock = Substitute.For<ITemplateProviderComponent>();
            newTemplateProviderComponentMock.Supports(Arg.Any<ITemplateIdentifier>()).Returns(true);
            newTemplateProviderComponentMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(this);
            var templateIdentifierMock = Substitute.For<ITemplateIdentifier>();
            sut.RegisterComponent(newTemplateProviderComponentMock);

            // Act
            sut.StartSession();

            // Assert
            sut.Invoking(x => x.Create(templateIdentifierMock))
               .Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void Calls_StartSession_On_All_Session_Aware_Components()
        {
            // Arrange
            var sessionAwareTemplateProviderComponent = new SessionAwareTemplateProviderComponent();
            var sut = new TemplateProvider(new[] { sessionAwareTemplateProviderComponent });

            // Act
            sut.StartSession();

            // Assert
            sessionAwareTemplateProviderComponent.Counter.Should().Be(1);
        }

        private sealed class SessionAwareTemplateProviderComponent : ISessionAwareComponent, ITemplateProviderComponent
        {
            public int Counter { get; private set; }

            public object Create(ITemplateIdentifier identifier)
            {
                throw new NotImplementedException();
            }

            public void StartSession() => Counter++;

            public bool Supports(ITemplateIdentifier identifier)
            {
                throw new NotImplementedException();
            }
        }
    }
}
