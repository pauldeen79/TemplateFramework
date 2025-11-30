namespace TemplateFramework.Core.Tests;

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
        public void Returns_Invalid_On_Null_Identifier(
            [Frozen] ITemplateProviderComponent templateProviderComponent,
            TemplateProvider sut)
        {
            // Arrange
            templateProviderComponent.Create(Arg.Any<ITemplateIdentifier>()).Returns(Result.Continue<object>());

            // Act
            var result = sut.Create(identifier: null!);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Identifier is required");
        }

        [Theory, AutoMockData]
        public void Returns_NotSupported_On_Unsupported_Type(
            [Frozen] ITemplateProviderComponent templateProviderComponent,
            [Frozen] ITemplateIdentifier identifier,
            TemplateProvider sut)
        {
            // Arrange
            templateProviderComponent.Create(Arg.Any<ITemplateIdentifier>()).Returns(Result.Continue<object>());

            // Act
            var result = sut.Create(identifier: identifier);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotSupported);
        }

        [Theory, AutoMockData]
        public void Returns_Template_Instance_From_Component_On_Supported_Type(
            [Frozen] ITemplateProviderComponent templateProviderComponent,
            [Frozen] ITemplateIdentifier identifier,
            TemplateProvider sut)
        {
            // Arrange
            var expectedTemplate = new object();
            templateProviderComponent.Create(Arg.Any<ITemplateIdentifier>()).Returns(Result.Success(expectedTemplate));

            // Act
            var template = sut.Create(identifier);

            // Assert
            template.Status.ShouldBe(ResultStatus.Ok);
            template.Value.ShouldBeSameAs(expectedTemplate);
        }
    }

    public class RegisterComponent
    {
        [Theory, AutoMockData]
        public void Throws_On_Null_Component(TemplateProvider sut)
        {
            // Act & Assert
            Action a = () => sut.RegisterComponent(component: null!);
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("component");
        }

        [Theory, AutoMockData]
        public void Adds_Component_Registration(
            [Frozen] ITemplateProviderComponent customTemplateProviderComponent,
            [Frozen] ITemplateIdentifier identifier,
            TemplateProvider sut)
        {
            // Arrange
            var expectedTemplate = new object();
            customTemplateProviderComponent.Create(Arg.Any<ITemplateIdentifier>()).Returns(Result.Success(expectedTemplate));

            // Act
            sut.RegisterComponent(customTemplateProviderComponent);

            // Assert
            sut.Create(identifier);
            customTemplateProviderComponent.Received().Create(identifier);
        }
    }

    public class StartSessionAsync : TemplateProviderTests
    {
        [Theory, AutoMockData]
        public async Task Clears_Registration_Performed_On_Current_Instance(
            [Frozen] ITemplateProviderComponent newTemplateProviderComponent,
            [Frozen] ITemplateIdentifier identifier)
        {
            // Arrange
            var sut = new TemplateProvider(Enumerable.Empty<ITemplateProviderComponent>()); //important for this test to begin without any template provider components
            newTemplateProviderComponent.Create(Arg.Any<ITemplateIdentifier>()).Returns(Result.Success<object>(this));
            sut.RegisterComponent(newTemplateProviderComponent);

            // Act
            await sut.StartSessionAsync(CancellationToken.None);

            // Assert
            var result = sut.Create(identifier);
            result.Status.ShouldBe(ResultStatus.NotSupported);
        }

        [Fact]
        public async Task Calls_StartSession_On_All_Session_Aware_Components()
        {
            // Arrange
            var sessionAwareTemplateProviderComponent = new SessionAwareTemplateProviderComponent();
            var sut = new TemplateProvider([sessionAwareTemplateProviderComponent]);

            // Act
            var result = await sut.StartSessionAsync(CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            sessionAwareTemplateProviderComponent.Counter.ShouldBe(1);
        }

        [Fact]
        public async Task Returns_Success_When_Initialization_Succeeds()
        {
            // Arrange
            var sessionAwareTemplateProviderComponent = new SessionAwareTemplateProviderComponent();
            var sut = new TemplateProvider([sessionAwareTemplateProviderComponent]);

            // Act
            var result = await sut.StartSessionAsync(CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public async Task Returns_Error_When_Initialization_Fails()
        {
            // Arrange
            var sessionAwareTemplateProviderComponent = new SessionAwareTemplateProviderComponent(success: false);
            var sut = new TemplateProvider([sessionAwareTemplateProviderComponent]);

            // Act
            var result = await sut.StartSessionAsync(CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("An error occured while starting the session. See the inner results for more details.");
            result.InnerResults.Count.ShouldBe(1);
            result.InnerResults.First().ErrorMessage.ShouldBe("Kaboom");
        }

        private sealed class SessionAwareTemplateProviderComponent : ISessionAwareComponent, ITemplateProviderComponent
        {
            public int Counter { get; private set; }
            public bool Success { get; }

            public SessionAwareTemplateProviderComponent(bool success = true)
            {
                Success = success;
            }

            public Result<object> Create(ITemplateIdentifier identifier)
            {
                throw new NotImplementedException();
            }

            public Task<Result> StartSessionAsync(CancellationToken token)
            {
                Counter++;
                return Task.FromResult(Success
                    ? Result.Success()
                    : Result.Error("Kaboom"));
            }
        }
    }
}
