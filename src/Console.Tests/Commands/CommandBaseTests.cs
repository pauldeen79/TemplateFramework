namespace TemplateFramework.Console.Tests.Commands;

public class CommandBaseTests
{
    protected Mock<IClipboard> ClipboardMock { get; } = new();
    protected Mock<IFileSystem> FileSystemMock { get; } = new();

    protected CommandBaseTest CreateSut() => new(ClipboardMock.Object, FileSystemMock.Object);

    protected const string AssemblyName = "MyAssemblyName";
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            TestHelpers.ConstructorMustThrowArgumentNullException(typeof(CommandBaseTest));
        }
    }

    public class GetCurrentDirectory : CommandBaseTests
    {
        [Fact]
        public void Throws_On_Null_AssemblyName()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.GetCurrentDirectoryPublic(string.Empty, assemblyName: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("assemblyName");
        }

        [Fact]
        public void Returns_CurrentDirectoryOption_Value_When_Present()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetCurrentDirectoryPublic("CurrentDirectory", assemblyName: AssemblyName);

            // Assert
            result.Should().Be("CurrentDirectory");
        }
    }

    public class GenerateSingleOutput : CommandBaseTests
    {
        // TODO: Add tests
    }

    public class WriteOutputToHost : CommandBaseTests
    {
        // TODO: Add tests
    }

    public class WriteOutputToClipboard : CommandBaseTests
    {
        // TODO: Add tests
    }

    public class GetDryRun : CommandBaseTests
    {
        // TODO: Add tests
    }

    public class GetDefaultFilename : CommandBaseTests
    {
        // TODO: Add tests
    }

    public class GetBasePath : CommandBaseTests
    {
        // TODO: Add tests
    }

    public sealed class CommandBaseTest : CommandBase
    {
        public CommandBaseTest(IClipboard clipboard, IFileSystem fileSystem) : base(clipboard, fileSystem)
        {
        }

        public override void Initialize(CommandLineApplication app)
        {
            throw new NotImplementedException();
        }

        public string? GetCurrentDirectoryPublic(string? currentDirectory, string assemblyName)
            => GetCurrentDirectory(currentDirectory, assemblyName);

        public string GenerateSingleOutputPublic(IMultipleContentBuilder builder, string basePath)
            => GenerateSingleOutput(builder, basePath);

        public void WriteOutputToHostPublic(CommandLineApplication app, string templateOutput, CommandOption<bool> bareOption)
            => WriteOutputToHost(app, templateOutput, bareOption);

        public void WriteOutputPublic(CommandLineApplication app, MultipleContentBuilderEnvironment generationEnvironment, string basePath, CommandOption<bool> bareOption, CommandOption<bool> clipboardOption, bool dryRun)
            => WriteOutput(app, generationEnvironment, basePath, bareOption, clipboardOption, dryRun);

        public void WriteOutputToClipboardPublic(CommandLineApplication app, string templateOutput, CommandOption<bool> bareOption)
            => WriteOutputToClipboard(app, templateOutput, bareOption);

        public bool GetDryRunPublic(CommandOption<bool> dryRunOption, CommandOption<bool> clipboardOption)
            => GetDryRun(dryRunOption, clipboardOption);

        public string GetDefaultFilenamePublic(CommandOption<string> defaultFilenameOption)
            => GetDefaultFilename(defaultFilenameOption);

        public string GetBasePathPublic(CommandOption<string> basePathOption)
            => GetBasePath(basePathOption);
    }
}
