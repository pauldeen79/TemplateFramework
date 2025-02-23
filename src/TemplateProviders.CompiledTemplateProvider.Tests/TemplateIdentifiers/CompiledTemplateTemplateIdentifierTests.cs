namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests.TemplateIdentifiers;

public class CompiledTemplateTemplateIdentifierTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_AssemblyName()
        {
            // Act & Assert
            Action a = () => _ = new CompiledTemplateIdentifier(assemblyName: null!, nameof(Constructor));
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("assemblyName");
        }

        [Fact]
        public void Throws_On_Empty_AssemblyName()
        {
            // Act & Assert
            Action a = () => _ = new CompiledTemplateIdentifier(assemblyName: string.Empty, nameof(Constructor));
            a.ShouldThrow<ArgumentException>().ParamName.ShouldBe("assemblyName");
        }

        [Fact]
        public void Throws_On_Null_ClassName()
        {
            // Act & Assert
            Action a = () => _ = new CompiledTemplateIdentifier(GetType().Assembly.FullName!, className: null!);
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("className");
        }

        [Fact]
        public void Throws_On_Empty_ClassName()
        {
            // Act & Assert
            Action a = () => _ = new CompiledTemplateIdentifier(GetType().Assembly.FullName!, className: string.Empty);
            a.ShouldThrow<ArgumentException>().ParamName.ShouldBe("className");
        }

        [Fact]
        public void Constructs_With_CurrentDirectory()
        {
            // Act
            var instance = new CompiledTemplateIdentifier(GetType().Assembly.FullName!, className: GetType().FullName!, Directory.GetCurrentDirectory());

            // Assert
            instance.AssemblyName.ShouldBe(GetType().Assembly.FullName);
            instance.ClassName.ShouldBe(GetType().FullName);
            instance.CurrentDirectory.ShouldBe(Directory.GetCurrentDirectory());
        }

        [Fact]
        public void Constructs_Without_CurrentDirectory()
        {
            // Act
            var instance = new CompiledTemplateIdentifier(GetType().Assembly.FullName!, className: GetType().FullName!);

            // Assert
            instance.AssemblyName.ShouldBe(GetType().Assembly.FullName);
            instance.ClassName.ShouldBe(GetType().FullName);
            instance.CurrentDirectory.ShouldNotBeEmpty();
        }
    }
}
