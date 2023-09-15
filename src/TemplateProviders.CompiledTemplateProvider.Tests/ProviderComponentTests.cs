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
        public void Throws_On_Null_Identifier()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(identifier: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("identifier");
        }

        [Fact]
        public void Throws_On_Identifier_Other_Than_CreateCompiledTemplateRequest()
        {
            // Arrange
            var templateIdentifier = Fixture.Freeze<ITemplateIdentifier>();
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(identifier: templateIdentifier))
               .Should().Throw<ArgumentException>().WithParameterName("identifier");
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
            instance.Should().NotBeNull();
            instance.Should().BeOfType<Create>();
        }

        [Fact]
        public void Throws_On_Wrong_ClassName()
        {
            // Arrange
            var templateFactory = Fixture.Freeze<ITemplateFactory>();
            var assemblyService = Fixture.Freeze<IAssemblyService>();
            SetupAssemblyService(assemblyService);
            templateFactory.Create(Arg.Any<Type>()).Returns(default(object?));
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(new CompiledTemplateIdentifier(GetType().Assembly.FullName!, "WrongName")))
               .Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Throws_When_Factory_Does_Not_Create_A_Template_Instance()
        {
            // Arrange
            var assemblyService = Fixture.Freeze<IAssemblyService>();
            var templateFactory = Fixture.Freeze<ITemplateFactory>();
            SetupAssemblyService(assemblyService);
            templateFactory.Create(Arg.Any<Type>()).Returns(default(object));
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(new CompiledTemplateIdentifier(GetType().Assembly.FullName!, GetType().FullName!)))
               .Should().Throw<InvalidOperationException>().WithMessage("Could not create instance of type TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests.ProviderComponentTests+Create");
        }
    }

    public class Supports : ProviderComponentTests
    {
        [Fact]
        public void Returns_False_On_Null_Request()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(null!);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_False_On_Request_Other_Than_CreateCompiledTemplateRequest()
        {
            // Arrange
            var templateIdentifier = Fixture.Freeze<ITemplateIdentifier>();
            var sut = CreateSut();

            // Act
            var result = sut.Supports(templateIdentifier);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_True_On_CreateCompiledTemplateRequest()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(new CompiledTemplateIdentifier(GetType().Assembly.FullName!, GetType().FullName!, string.Empty));

            // Assert
            result.Should().BeTrue();
        }
    }
}
