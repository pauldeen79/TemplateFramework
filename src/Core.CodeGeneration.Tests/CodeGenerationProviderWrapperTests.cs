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
    public void Throws_On_Required_Null_Property()
    {
        // Arrange
        var providerMock = new Mock<ICodeGenerationProvider>();
        var sut = new CodeGenerationProviderWrapper(providerMock.Object);

        // Act & Assert
        sut.Invoking(x => _ = x.Path)
           .Should().Throw<InvalidOperationException>()
           .WithMessage("Path of template Castle.Proxies.ICodeGenerationProviderProxy was null");
    }
}
