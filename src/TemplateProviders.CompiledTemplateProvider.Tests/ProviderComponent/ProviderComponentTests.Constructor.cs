namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests;

public partial class ProviderComponentTests
{
    public class Constructor : ProviderComponentTests
    {
        [Fact]
        public void Throws_On_Null_AssemblyService()
        {
            // Act & Assert
            this.Invoking(_ => new ProviderComponent(assemblyService: null!, CompiledTemplateFactoryMock))
                .Should().Throw<ArgumentNullException>().WithParameterName("assemblyService");
        }

        [Fact]
        public void Throws_On_Null_Factory()
        {
            // Act & Assert
            this.Invoking(_ => new ProviderComponent(AssemblyServiceMock, templateFactory: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }
    }
}
