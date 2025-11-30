namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests;

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
        private void SetupAssemblyService(IAssemblyService assemblyService)
        {
            assemblyService.GetAssembly(Arg.Any<string>(), Arg.Any<string>())
                           .Returns(GetType().Assembly);
        }

        [Fact]
        public void Returns_Continue_On_Null_Identifier()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Create(identifier: null!);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Continue_On_Identifier_Other_Than_CreateCompiledTemplateRequest()
        {
            // Arrange
            var templateIdentifier = Fixture.Freeze<ITemplateIdentifier>();
            var sut = CreateSut();

            // Act
            var result = sut.Create(identifier: templateIdentifier);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Template_Instance_Correctly()
        {
            // Arrange
            var templateFactory = Fixture.Freeze<ITemplateFactory>();
            var assemblyService = Fixture.Freeze<IAssemblyService>();
            SetupAssemblyService(assemblyService);
            templateFactory.Create(Arg.Any<Type>()).Returns(x => Activator.CreateInstance(x.ArgAt<Type>(0))!);
            var sut = CreateSut();

            // Act
            var instance = sut.Create(new CompiledTemplateIdentifier(GetType().Assembly.FullName!, GetType().FullName!));

            // Assert
            instance.Status.ShouldBe(ResultStatus.Ok);
            instance.Value.ShouldBeOfType<Create>();
        }

        [Fact]
        public void Returns_Error_On_Wrong_ClassName()
        {
            // Arrange
            var templateFactory = Fixture.Freeze<ITemplateFactory>();
            var assemblyService = Fixture.Freeze<IAssemblyService>();
            SetupAssemblyService(assemblyService);
            templateFactory.Create(Arg.Any<Type>()).Returns(default(object?));
            var sut = CreateSut();

            // Act
            var result = sut.Create(new CompiledTemplateIdentifier(GetType().Assembly.FullName!, "WrongName"));
            
            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
        }

        [Fact]
        public void Returns_Error_When_Factory_Does_Not_Create_A_Template_Instance()
        {
            // Arrange
            var assemblyService = Fixture.Freeze<IAssemblyService>();
            var templateFactory = Fixture.Freeze<ITemplateFactory>();
            SetupAssemblyService(assemblyService);
            templateFactory.Create(Arg.Any<Type>()).Returns(default(object));
            var sut = CreateSut();

            // Act
            var result = sut.Create(new CompiledTemplateIdentifier(GetType().Assembly.FullName!, GetType().FullName!));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Could not create instance of type TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests.ProviderComponentTests+Create");
        }
    }
}
