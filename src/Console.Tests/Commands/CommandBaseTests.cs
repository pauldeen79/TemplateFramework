namespace TemplateFramework.Console.Tests.Commands;

public class CommandBaseTests
{
    protected Mock<IClipboard> ClipboardMock { get; } = new();
    protected Mock<IFileSystem> FileSystemMock { get; } = new();
    protected Mock<IMultipleContentBuilder> MultipleContentBuilderMock { get; } = new();

    protected CommandBaseTest CreateSut() => new(ClipboardMock.Object, FileSystemMock.Object);

    protected const string AssemblyName = "MyAssemblyName";
    protected const string Filename = "MyFile.txt";

    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            TestHelpers.ConstructorMustThrowArgumentNullException(typeof(CommandBaseTest));
        }
    }

    public class Watch : CommandBaseTests
    {
        [Fact]
        public void Throws_On_Null_App()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.WatchPublic(app: null!, false, Filename, () => { }))
               .Should().Throw<ArgumentNullException>().WithParameterName("app");
        }

        [Fact]
        public void Throws_On_Null_Filename()
        {
            // Arrange
            var sut = CreateSut();
            using var app = new CommandLineApplication();

            // Act & Assert
            sut.Invoking(x => x.WatchPublic(app, false, filename: null!, () => { }))
               .Should().Throw<ArgumentNullException>().WithParameterName("filename");
        }

        [Fact]
        public void Throws_On_Null_Action()
        {
            // Arrange
            var sut = CreateSut();
            using var app = new CommandLineApplication();

            // Act & Assert
            sut.Invoking(x => x.WatchPublic(app, false, Filename, action: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("action");
        }

        [Fact]
        public void Executes_Action_Once_When_Watch_Argument_Is_False()
        {
            // Arrange
            var sut = CreateSut();
            using var app = new CommandLineApplication();
            var counter = 0;

            // Act
            sut.WatchPublic(app, false, Filename, () => counter++);

            // Assert
            counter.Should().Be(1);
        }

        [Fact]
        public void Writes_ErrorMessage_When_Watch_Argument_Is_True_And_File_Does_Not_Exist()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = CommandLineCommandHelper.ExecuteCommand(app => sut.WatchPublic(app, true, Filename, () => { }));

            // Assert
            result.Should().Be("Error: Could not find file [MyFile.txt]. Could not watch file for changes." + Environment.NewLine);
        }

        [Fact]
        public void Does_Not_Execute_Action_When_Watch_Argument_Is_True_And_File_Does_Not_Exist()
        {
            // Arrange
            var sut = CreateSut();
            using var app = new CommandLineApplication();
            var counter = 0;

            // Act
            sut.WatchPublic(app, true, Filename, () => counter++);

            // Assert
            counter.Should().Be(0);
        }

        [Fact]
        public void Repeats_Action_When_File_Has_Changed()
        {
            // Arrange
            var sut = CreateSut();
            using var app = new CommandLineApplication();
            var fileSystemCallCounter = 0;
            var actionCallCounter = 0;
            FileSystemMock.Setup(x => x.FileExists(Filename)).Returns(() =>
            {
                if (fileSystemCallCounter > 2)
                {
                    sut.DoAbort();
                }
                return true;
            });
            FileSystemMock.Setup(x => x.GetFileLastWriteTime(Filename)).Returns(() =>
            {
                fileSystemCallCounter++;
                return DateTime.MinValue.AddDays(fileSystemCallCounter);
            });

            // Act
            sut.WatchPublic(app, true, Filename, () => actionCallCounter++);

            // Assert
            actionCallCounter.Should().Be(4);
        }

        [Fact]
        public void Breaks_Watch_Loop_When_File_Has_Been_Removed()
        {
            // Arrange
            var sut = CreateSut();
            var fileSystemCallCounter = 0;
            var actionCallCounter = 0;
            FileSystemMock.Setup(x => x.FileExists(Filename)).Returns(() =>
            {
                return fileSystemCallCounter < 3;
            });
            FileSystemMock.Setup(x => x.GetFileLastWriteTime(Filename)).Returns(() =>
            {
                fileSystemCallCounter++;
                return DateTime.MinValue.AddDays(fileSystemCallCounter);
            });

            // Act
            var result = CommandLineCommandHelper.ExecuteCommand(app => sut.WatchPublic(app, true, Filename, () => actionCallCounter++));

            // Assert
            result.Should().Be(@"Watching file [MyFile.txt] for changes...
Error: Could not find file [MyFile.txt]. Could not watch file for changes.
");
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

        [Fact]
        public void Returns_Directory_Of_Assembly_When_AssemblyName_Ends_With_Dll_And_CurrentDirectory_Is_Empty()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetCurrentDirectoryPublic(null, assemblyName: Path.Combine("Directory", "Assembly.dll"));

            // Assert
            result.Should().Be("Directory");
        }

        [Fact]
        public void Returns_Empty_String_When_AssemblyName_Does_Not_End_With_Dll()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetCurrentDirectoryPublic(null, assemblyName: AssemblyName);

            // Assert
            result.Should().BeEmpty();
        }
    }

    public class GenerateSingleOutput : CommandBaseTests
    {
        [Fact]
        public void Throws_On_Null_Builder()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.GenerateSingleOutputPublic(builder: null!, string.Empty))
               .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_BasePath()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.GenerateSingleOutputPublic(MultipleContentBuilderMock.Object, basePath: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("basePath");
        }
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
            SleepTimeInMs = 1;
        }

        public override void Initialize(CommandLineApplication app)
            => throw new NotImplementedException("Not implemented on purpose. This method is abstract, so does not need to be unit tested");

        public void WatchPublic(CommandLineApplication app, bool watch, string filename, Action action)
            => Watch(app, watch, filename, action);

        public string? GetCurrentDirectoryPublic(string? currentDirectory, string assemblyName)
            => GetCurrentDirectory(currentDirectory, assemblyName);

        public string GenerateSingleOutputPublic(IMultipleContentBuilder builder, string basePath)
            => GenerateSingleOutput(builder, basePath);

        public void WriteOutputToHostPublic(CommandLineApplication app, string templateOutput, bool bare)
            => WriteOutputToHost(app, templateOutput, bare);

        public void WriteOutputPublic(CommandLineApplication app, MultipleContentBuilderEnvironment generationEnvironment, string basePath, bool bare, bool clipboard, bool dryRun)
            => WriteOutput(app, generationEnvironment, basePath, bare, clipboard, dryRun);

        public void WriteOutputToClipboardPublic(CommandLineApplication app, string templateOutput, bool bare)
            => WriteOutputToClipboard(app, templateOutput, bare);

        public bool GetDryRunPublic(bool dryRun, bool clipboard)
            => GetDryRun(dryRun, clipboard);

        public string GetDefaultFilenamePublic(string? defaultFilename)
            => GetDefaultFilename(defaultFilename);

        public string GetBasePathPublic(string? basePath)
            => GetBasePath(basePath);

        public void DoAbort() => Abort = true;
    }
}
