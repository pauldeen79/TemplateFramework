namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests.Requests;

public class CreateCompiledTemplateRequestTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_AssemblyName()
        {
            // Act & Assert
            this.Invoking(_ => new CreateCompiledTemplateRequest(assemblyName: null!, nameof(Constructor)))
                .Should().Throw<ArgumentNullException>().WithParameterName("assemblyName");
        }

        [Fact]
        public void Throws_On_Empty_AssemblyName()
        {
            // Act & Assert
            this.Invoking(_ => new CreateCompiledTemplateRequest(assemblyName: string.Empty, nameof(Constructor)))
                .Should().Throw<ArgumentException>().WithParameterName("assemblyName");
        }

        [Fact]
        public void Throws_On_Null_ClassName()
        {
            // Act & Assert
            this.Invoking(_ => new CreateCompiledTemplateRequest(GetType().Assembly.FullName!, className: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("className");
        }

        [Fact]
        public void Throws_On_Empty_ClassName()
        {
            // Act & Assert
            this.Invoking(_ => new CreateCompiledTemplateRequest(GetType().Assembly.FullName!, className: string.Empty))
                .Should().Throw<ArgumentException>().WithParameterName("className");
        }
    }
}
