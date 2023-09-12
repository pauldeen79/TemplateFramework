namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests.TemplateIdentifiers;

public class CompiledTemplateTemplateIdentifierTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_AssemblyName()
        {
            // Act & Assert
            this.Invoking(_ => new CompiledTemplateIdentifier(assemblyName: null!, nameof(Constructor)))
                .Should().Throw<ArgumentNullException>().WithParameterName("assemblyName");
        }

        [Fact]
        public void Throws_On_Empty_AssemblyName()
        {
            // Act & Assert
            this.Invoking(_ => new CompiledTemplateIdentifier(assemblyName: string.Empty, nameof(Constructor)))
                .Should().Throw<ArgumentException>().WithParameterName("assemblyName");
        }

        [Fact]
        public void Throws_On_Null_ClassName()
        {
            // Act & Assert
            this.Invoking(_ => new CompiledTemplateIdentifier(GetType().Assembly.FullName!, className: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("className");
        }

        [Fact]
        public void Throws_On_Empty_ClassName()
        {
            // Act & Assert
            this.Invoking(_ => new CompiledTemplateIdentifier(GetType().Assembly.FullName!, className: string.Empty))
                .Should().Throw<ArgumentException>().WithParameterName("className");
        }

        [Fact]
        public void Constructs_With_CurrentDirectory()
        {
            // Act
            var instance = new CompiledTemplateIdentifier(GetType().Assembly.FullName!, className: GetType().FullName!, Directory.GetCurrentDirectory());

            // Assert
            instance.AssemblyName.Should().Be(GetType().Assembly.FullName);
            instance.ClassName.Should().Be(GetType().FullName);
            instance.CurrentDirectory.Should().Be(Directory.GetCurrentDirectory());
        }

        [Fact]
        public void Constructs_Without_CurrentDirectory()
        {
            // Act
            var instance = new CompiledTemplateIdentifier(GetType().Assembly.FullName!, className: GetType().FullName!);

            // Assert
            instance.AssemblyName.Should().Be(GetType().Assembly.FullName);
            instance.ClassName.Should().Be(GetType().FullName);
            instance.CurrentDirectory.Should().NotBeEmpty();
        }
    }
}
