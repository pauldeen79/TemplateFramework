namespace TemplateFramework.Core.Tests.GenerationEnvironments;

public class StringBuilderEnvironmentTests
{
    protected IFileSystem FileSystemMock { get; } = Substitute.For<IFileSystem>();
    protected IRetryMechanism RetryMechanism { get; } = new FastRetryMechanism();
    protected ICodeGenerationProvider CodeGenerationProviderMock { get; } = Substitute.For<ICodeGenerationProvider>();
    protected StringBuilder Builder { get; } = new();
    
    protected StringBuilderEnvironment CreateSut() => new(FileSystemMock, RetryMechanism, Builder);

    public class Constructor : StringBuilderEnvironmentTests
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(StringBuilderEnvironment).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }

        [Fact]
        public void Creates_Instance_Correctly_Without_Arguments()
        {
            // Act
            var instance = new StringBuilderEnvironment();

            // Assert
            instance.Builder.Should().NotBeNull();
        }
    }

    public class SaveContents : StringBuilderEnvironmentTests
    {
        [Fact]
        public void Throws_On_Null_Provider()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Awaiting(x => x.SaveContents(provider: null!, TestData.BasePath, "Filename.txt"))
               .Should().ThrowAsync<ArgumentNullException>().WithParameterName("provider");
        }

        [Fact]
        public void Throws_On_Empty_DefaultFilename()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Awaiting(x => x.SaveContents(CodeGenerationProviderMock, TestData.BasePath, defaultFilename: null!))
               .Should().ThrowAsync<ArgumentException>().WithParameterName("defaultFilename");
        }

        [Fact]
        public void Throws_On_Null_DefaultFilename()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Awaiting(x => x.SaveContents(CodeGenerationProviderMock, TestData.BasePath, defaultFilename: string.Empty))
               .Should().ThrowAsync<ArgumentException>().WithParameterName("defaultFilename");
        }

        [Fact]
        public void Writes_Contents_To_FileSystem_Without_BasePath()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.Encoding.Returns(Encoding.UTF32);
            Builder.Append("Contents");

            // Act
            sut.SaveContents(CodeGenerationProviderMock, string.Empty, "Filename.txt");

            // Arrange
            FileSystemMock.Received().WriteAllText("Filename.txt", "Contents", Encoding.UTF32);
        }

        [Fact]
        public void Writes_Contents_To_FileSystem_With_BasePath()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.Encoding.Returns(Encoding.UTF32);
            Builder.Append("Contents");

            // Act
            sut.SaveContents(CodeGenerationProviderMock, TestData.BasePath, "Filename.txt");

            // Arrange
            FileSystemMock.Received().WriteAllText(Path.Combine(TestData.BasePath, "Filename.txt"), "Contents", Encoding.UTF32);
        }
    }

    private sealed class FastRetryMechanism : RetryMechanism
    {
        protected override int WaitTimeInMs => 1;
    }
}
