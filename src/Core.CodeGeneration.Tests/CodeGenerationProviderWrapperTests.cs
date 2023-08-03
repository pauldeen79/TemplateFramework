namespace TemplateFramework.Core.CodeGeneration.Tests;

public class CodeGenerationProviderWrapperTests
{
    [Fact]
    public void Throws_On_Null_Instance()
    {
        // Act & Assert
        this.Invoking(_ => new CodeGenerationProviderWrapper(instance: null!))
            .Should().Throw<ArgumentNullException>().WithParameterName("instance");
    }

    [Fact]
    public void Wraps_Instance_Correctly()
    {
        // Act
        var instance = new CodeGenerationProviderWrapper(new MyGeneratorProvider());
        var compareTo = new MyGeneratorProvider();

        // Assert
        instance.Path.Should().Be(compareTo.Path);
        instance.RecurseOnDeleteGeneratedFiles.Should().Be(compareTo.RecurseOnDeleteGeneratedFiles);
        instance.LastGeneratedFilesFilename.Should().Be(compareTo.LastGeneratedFilesFilename);
        instance.Encoding.Should().Be(compareTo.Encoding);
        instance.CreateAdditionalParameters().Should().BeEquivalentTo(compareTo.CreateAdditionalParameters());
        instance.CreateGenerator().Should().BeOfType(compareTo.CreateGenerator().GetType());
        instance.CreateModel().Should().BeEquivalentTo(compareTo.CreateModel());
    }

    [Fact]
    public void Throws_On_Required_Null_Property_Non_Value_Type()
    {
        // Arrange
        var providerMock = new Mock<ICodeGenerationProvider>();
        var sut = new CodeGenerationProviderWrapper(providerMock.Object);

        // Act & Assert
        sut.Invoking(x => _ = x.Path)
           .Should().Throw<InvalidOperationException>()
           .WithMessage("Path of template Castle.Proxies.ICodeGenerationProviderProxy was null");
    }

    [Fact]
    public void Throws_On_Required_Null_Property_Value_Type()
    {
        // Arrange
        var sut = new CodeGenerationProviderWrapper(new MyThing());

        // Act & Assert
        sut.Invoking(x => _ = x.RecurseOnDeleteGeneratedFiles)
           .Should().Throw<InvalidOperationException>()
           .WithMessage("RecurseOnDeleteGeneratedFiles of template TemplateFramework.Core.CodeGeneration.Tests.CodeGenerationProviderWrapperTests+MyThing was null");
    }

    private sealed class MyThing
    {
#pragma warning disable S1144 // Unused private types or members should be removed
        public bool? DryRun => null;
#pragma warning restore S1144 // Unused private types or members should be removed
    }
}
