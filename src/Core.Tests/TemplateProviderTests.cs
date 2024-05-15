﻿namespace TemplateFramework.Core.Tests;

public class TemplateProviderTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(TemplateProvider).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }

    public class Create
    {
        [Theory, AutoMockData]
        public void Throws_On_Null_Identifier(TemplateProvider sut)
        {
            // Act & Assert
            sut.Invoking(x => x.Create(identifier: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("identifier");
        }

        [Theory, AutoMockData]
        public void Throws_On_Unsupported_Type(
            [Frozen] ITemplateProviderComponent templateProviderComponent,
            [Frozen] ITemplateIdentifier identifier,
            TemplateProvider sut)
        {
            // Arrange
            templateProviderComponent.Supports(identifier).Returns(false);

            // Act & Assert
            sut.Invoking(x => x.Create(identifier: identifier))
               .Should().Throw<NotSupportedException>();
        }

        [Theory, AutoMockData]
        public void Returns_Template_Instance_From_Component_On_Supported_Type(
            [Frozen] ITemplateProviderComponent templateProviderComponent,
            [Frozen] ITemplateIdentifier identifier,
            TemplateProvider sut)
        {
            // Arrange
            var expectedTemplate = new object();
            templateProviderComponent.Supports(identifier).Returns(true);
            templateProviderComponent.Create(identifier).Returns(expectedTemplate);

            // Act
            var template = sut.Create(identifier);

            // Assert
            template.Should().BeSameAs(expectedTemplate);
        }
    }

    public class RegisterComponent
    {
        [Theory, AutoMockData]
        public void Throws_On_Null_Component(TemplateProvider sut)
        {
            // Act & Assert
            sut.Invoking(x => x.RegisterComponent(component: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("component");
        }

        [Theory, AutoMockData]
        public void Adds_Component_Registration(
            [Frozen] ITemplateProviderComponent customTemplateProviderComponent,
            [Frozen] ITemplateIdentifier identifier,
            TemplateProvider sut)
        {
            // Arrange
            var expectedTemplate = new object();
            customTemplateProviderComponent.Supports(identifier).Returns(true);
            customTemplateProviderComponent.Create(identifier).Returns(expectedTemplate);

            // Act
            sut.RegisterComponent(customTemplateProviderComponent);

            // Assert
            sut.Create(identifier);
            customTemplateProviderComponent.Received().Create(identifier);
        }
    }

    public class StartSession : TemplateProviderTests
    {
        [Theory, AutoMockData]
        public async Task Clears_Registration_Performed_On_Current_Instance(
            [Frozen] ITemplateProviderComponent newTemplateProviderComponent,
            [Frozen] ITemplateIdentifier identifier)
        {
            // Arrange
            var sut = new TemplateProvider(Enumerable.Empty<ITemplateProviderComponent>()); //important for this test to begin without any template provider components
            newTemplateProviderComponent.Supports(Arg.Any<ITemplateIdentifier>()).Returns(true);
            newTemplateProviderComponent.Create(Arg.Any<ITemplateIdentifier>()).Returns(this);
            sut.RegisterComponent(newTemplateProviderComponent);

            // Act
            await sut.StartSession();

            // Assert
            sut.Invoking(x => x.Create(identifier))
               .Should().Throw<NotSupportedException>();
        }

        [Fact]
        public async Task Calls_StartSession_On_All_Session_Aware_Components()
        {
            // Arrange
            var sessionAwareTemplateProviderComponent = new SessionAwareTemplateProviderComponent();
            var sut = new TemplateProvider(new[] { sessionAwareTemplateProviderComponent });

            // Act
            await sut.StartSession();

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

            public Task StartSession()
            {
                Counter++;
                return Task.CompletedTask;
            }

            public bool Supports(ITemplateIdentifier identifier)
            {
                throw new NotImplementedException();
            }
        }
    }
}
