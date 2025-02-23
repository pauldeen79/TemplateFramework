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
            instance.Builder.ShouldNotBeNull();
        }
    }

    public class SaveContents : StringBuilderEnvironmentTests
    {
        [Fact]
        public async Task Throws_On_Null_Provider()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Task t = sut.SaveContents(provider: null!, TestData.BasePath, "Filename.txt", CancellationToken.None))
                     .Should().ThrowAsync<ArgumentNullException>().ParamName.ShouldBe("provider");
        }

        [Fact]
        public async Task Throws_On_Empty_DefaultFilename()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Task t = sut.SaveContents(CodeGenerationProviderMock, TestData.BasePath, defaultFilename: null!, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentException>().ParamName.ShouldBe("defaultFilename");
        }

        [Fact]
        public async Task Throws_On_Null_DefaultFilename()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Task t = sut.SaveContents(CodeGenerationProviderMock, TestData.BasePath, defaultFilename: string.Empty, CancellationToken.None))
                     .Should().ThrowAsync<ArgumentException>().ParamName.ShouldBe("defaultFilename");
        }

        [Fact]
        public async Task Writes_Contents_To_FileSystem_Without_BasePath()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.Encoding.Returns(Encoding.UTF32);
            Builder.Append("Contents");

            // Act
            await sut.SaveContents(CodeGenerationProviderMock, string.Empty, "Filename.txt", CancellationToken.None);

            // Arrange
            FileSystemMock.Received().WriteAllText("Filename.txt", "Contents", Encoding.UTF32);
        }

        [Fact]
        public async Task Writes_Contents_To_FileSystem_With_BasePath()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.Encoding.Returns(Encoding.UTF32);
            Builder.Append("Contents");

            // Act
            await sut.SaveContents(CodeGenerationProviderMock, TestData.BasePath, "Filename.txt", CancellationToken.None);

            // Arrange
            FileSystemMock.Received().WriteAllText(Path.Combine(TestData.BasePath, "Filename.txt"), "Contents", Encoding.UTF32);
        }
    }

    private sealed class FastRetryMechanism : RetryMechanism
    {
        protected override int WaitTimeInMs => 1;
    }
}
