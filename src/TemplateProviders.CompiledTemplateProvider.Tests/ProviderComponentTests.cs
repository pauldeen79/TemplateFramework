using System.Reflection;

namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests;

public partial class ProviderComponentTests
{
    public class Constructor : ProviderComponentTests
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

        [Theory, AutoMockData]
        public void Throws_On_Null_Identifier(ProviderComponent sut)
        {
            // Act & Assert
            sut.Invoking(x => x.Create(identifier: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("identifier");
        }

        [Theory, AutoMockData]
        public void Throws_On_Identifier_Other_Than_CreateCompiledTemplateRequest(ProviderComponent sut)
        {
            // Act & Assert
            sut.Invoking(x => x.Create(identifier: Substitute.For<ITemplateIdentifier>()))
               .Should().Throw<ArgumentException>().WithParameterName("identifier");
        }

        [Theory, AutoMockData]
        public void Returns_Template_Instance_Correctly(
            [Frozen] ITemplateFactory compiledTemplateFactory,
            [Frozen] IAssemblyService assemblyService,
            ProviderComponent sut)
        {
            // Arrange
            SetupAssemblyService(assemblyService);
            compiledTemplateFactory.Create(Arg.Any<Type>()).Returns(x => Activator.CreateInstance(x.ArgAt<Type>(0))!);

            // Act
            var instance = sut.Create(new CompiledTemplateIdentifier(GetType().Assembly.FullName!, GetType().FullName!));

            // Assert
            instance.Should().NotBeNull();
            instance.Should().BeOfType<Create>();
        }

        [Theory, AutoMockData]
        public void Throws_On_Wrong_ClassName(ProviderComponent sut)
        {
            // Act & Assert
            sut.Invoking(x => x.Create(new CompiledTemplateIdentifier(GetType().Assembly.FullName!, "WrongName")))
               .Should().Throw<InvalidOperationException>();
        }

        [Theory, AutoMockData]
        public void Throws_When_Factory_Does_Not_Create_A_Template_Instance(
            [Frozen] IAssemblyService assemblyService,
            [Frozen] ITemplateFactory templateFactory,
            ProviderComponent sut)
        {
            // Arrange
            SetupAssemblyService(assemblyService);
            templateFactory.Create(Arg.Any<Type>()).Returns(default(object));

            // Act & Assert
            sut.Invoking(x => x.Create(new CompiledTemplateIdentifier(GetType().Assembly.FullName!, GetType().FullName!)))
               .Should().Throw<InvalidOperationException>().WithMessage("Could not create instance of type TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests.ProviderComponentTests+Create");
        }
    }

    public class Supports : ProviderComponentTests
    {
        [Theory, AutoMockData]
        public void Returns_False_On_Null_Request(ProviderComponent sut)
        {
            // Act
            var result = sut.Supports(null!);

            // Assert
            result.Should().BeFalse();
        }

        [Theory, AutoMockData]
        public void Returns_False_On_Request_Other_Than_CreateCompiledTemplateRequest(ProviderComponent sut)
        {
            // Act
            var result = sut.Supports(Substitute.For<ITemplateIdentifier>());

            // Assert
            result.Should().BeFalse();
        }

        [Theory, AutoMockData]
        public void Returns_True_On_CreateCompiledTemplateRequest(ProviderComponent sut)
        {
            // Act
            var result = sut.Supports(new CompiledTemplateIdentifier(GetType().Assembly.FullName!, GetType().FullName!, string.Empty));

            // Assert
            result.Should().BeTrue();
        }
    }
}
