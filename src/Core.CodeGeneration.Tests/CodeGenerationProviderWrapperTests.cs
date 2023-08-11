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
    public void Throws_On_Required_Null_Path()
    {
        // Arrange
        var providerMock = new Mock<ICodeGenerationProvider>();
        var sut = new CodeGenerationProviderWrapper(providerMock.Object);

        // Act & Assert
        sut.Invoking(x => _ = x.Path)
           .Should().Throw<InvalidOperationException>()
           .WithMessage("Path of provider Castle.Proxies.ICodeGenerationProviderProxy was null");
    }

    [Fact]
    public void Throws_On_NonExisting_Path_Property()
    {
        // Arrange
        var sut = new CodeGenerationProviderWrapper(new object());

        // Act & Assert
        sut.Invoking(x => _ = x.Path)
           .Should().Throw<InvalidOperationException>()
           .WithMessage("Path of provider System.Object was null");
    }

    [Fact]
    public void Throws_On_Required_Null_RecurseOnDeleteGeneratedFiles()
    {
        // Arrange
        var sut = new CodeGenerationProviderWrapper(new MyThing());

        // Act & Assert
        sut.Invoking(x => _ = x.RecurseOnDeleteGeneratedFiles)
           .Should().Throw<InvalidOperationException>()
           .WithMessage("RecurseOnDeleteGeneratedFiles of provider TemplateFramework.Core.CodeGeneration.Tests.CodeGenerationProviderWrapperTests+MyThing was null");
    }

    [Fact]
    public void Throws_On_NonExisting_RecurseOnDeleteGeneratedFiles_Property()
    {
        // Arrange
        var sut = new CodeGenerationProviderWrapper(new object());

        // Act & Assert
        sut.Invoking(x => _ = x.RecurseOnDeleteGeneratedFiles)
           .Should().Throw<InvalidOperationException>()
           .WithMessage("RecurseOnDeleteGeneratedFiles of provider System.Object was null");
    }

    [Fact]
    public void Throws_On_Required_Null_LastGeneratedFilesFilename()
    {
        // Arrange
        var providerMock = new Mock<ICodeGenerationProvider>();
        var sut = new CodeGenerationProviderWrapper(providerMock.Object);

        // Act & Assert
        sut.Invoking(x => _ = x.LastGeneratedFilesFilename)
           .Should().Throw<InvalidOperationException>()
           .WithMessage("LastGeneratedFilesFilename of provider Castle.Proxies.ICodeGenerationProviderProxy was null");
    }

    [Fact]
    public void Throws_On_NonExisting_LastGeneratedFilesFilename_Property()
    {
        // Arrange
        var sut = new CodeGenerationProviderWrapper(new object());

        // Act & Assert
        sut.Invoking(x => _ = x.LastGeneratedFilesFilename)
           .Should().Throw<InvalidOperationException>()
           .WithMessage("LastGeneratedFilesFilename of provider System.Object was null");
    }

    [Fact]
    public void Throws_On_Required_Null_Encoding()
    {
        // Arrange
        var providerMock = new Mock<ICodeGenerationProvider>();
        var sut = new CodeGenerationProviderWrapper(providerMock.Object);

        // Act & Assert
        sut.Invoking(x => _ = x.Encoding)
           .Should().Throw<InvalidOperationException>()
           .WithMessage("Encoding of provider Castle.Proxies.ICodeGenerationProviderProxy was null");
    }

    [Fact]
    public void Throws_On_NonExisting_Encoding_Property()
    {
        // Arrange
        var sut = new CodeGenerationProviderWrapper(new object());

        // Act & Assert
        sut.Invoking(x => _ = x.Encoding)
           .Should().Throw<InvalidOperationException>()
           .WithMessage("Encoding of provider System.Object was null");
    }

    [Fact]
    public void CreateAdditionalParameters_Returns_Null_When_Method_Is_Not_Found_On_Wrapped_Instance()
    {
        // Arrange
        var sut = new CodeGenerationProviderWrapper(new object());

        // Act
        var result = sut.CreateAdditionalParameters();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void CreateGenerator_Throws_When_Method_Is_Not_Found_On_Wrapped_Instance()
    {
        // Arrange
        var sut = new CodeGenerationProviderWrapper(new object());

        // Act & Assert
        sut.Invoking(x => _ = x.CreateGenerator())
           .Should().Throw<InvalidOperationException>()
           .WithMessage("CreateGenerator of provider System.Object was null");
    }

    [Fact]
    public void CreateModel_Returns_Null_When_Method_Is_Not_Found_On_Wrapped_Instance()
    {
        // Arrange
        var sut = new CodeGenerationProviderWrapper(new object());

        // Act
        var result = sut.CreateModel();

        // Assert
        result.Should().BeNull();
    }

    private sealed class MyThing
    {
#pragma warning disable S1144 // Unused private types or members should be removed
        public bool? DryRun => null;
#pragma warning restore S1144 // Unused private types or members should be removed
    }
}
