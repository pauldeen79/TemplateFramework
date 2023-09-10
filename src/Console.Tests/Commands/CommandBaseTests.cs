namespace TemplateFramework.Console.Tests.Commands;

public class CommandBaseTests
{
    protected const string AssemblyName = "MyAssemblyName";
    protected const string Filename = "MyFile.txt";

    protected void SetupMultipleContentBuilderMock(IMultipleContentBuilder multipleContentBuilderMock, string filenamePrefix)
    {
        var contentBuilderMock1 = Substitute.For<IContent>();
        contentBuilderMock1.Filename.Returns($"{filenamePrefix}File1.txt");
        contentBuilderMock1.Contents.Returns("Contents from file1");

        var contentBuilderMock2 = Substitute.For<IContent>();
        contentBuilderMock2.Filename.Returns($"{filenamePrefix}File2.txt");
        contentBuilderMock2.Contents.Returns("Contents from file2");

        var multipleContentMock = Substitute.For<IMultipleContent>();
        multipleContentMock.Contents.Returns(new[] { contentBuilderMock1, contentBuilderMock2 });

        multipleContentBuilderMock.Build().Returns(multipleContentMock);
    }

    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            TestHelpers.ConstructorMustThrowArgumentNullException(typeof(CommandBaseTest), t => Substitute.For(new[] { t }, Array.Empty<object>()));
        }
    }

    public class Watch : CommandBaseTests
    {
        [Theory, AutoMockData]
        public void Throws_On_Null_App(CommandBaseTest sut)
        {
            // Act & Assert
            sut.Invoking(x => x.WatchPublic(app: null!, false, Filename, () => { }))
               .Should().Throw<ArgumentNullException>().WithParameterName("app");
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_Filename(CommandBaseTest sut)
        {
            // Arrange
            using var app = new CommandLineApplication();

            // Act & Assert
            sut.Invoking(x => x.WatchPublic(app, false, filename: null!, () => { }))
               .Should().Throw<ArgumentNullException>().WithParameterName("filename");
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_Action(CommandBaseTest sut)
        {
            // Arrange
            using var app = new CommandLineApplication();

            // Act & Assert
            sut.Invoking(x => x.WatchPublic(app, false, Filename, action: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("action");
        }

        [Theory, AutoMockData]
        public void Executes_Action_Once_When_Watch_Argument_Is_False(CommandBaseTest sut)
        {
            // Arrange
            using var app = new CommandLineApplication();
            var counter = 0;

            // Act
            sut.WatchPublic(app, false, Filename, () => counter++);

            // Assert
            counter.Should().Be(1);
        }

        [Theory, AutoMockData]
        public void Writes_ErrorMessage_When_Watch_Argument_Is_True_And_File_Does_Not_Exist(
            [Frozen] IFileSystem fileSystemMock,
            CommandBaseTest sut)
        {
            // Arrange
            fileSystemMock.FileExists(Filename).Returns(false);

            // Act
            var result = CommandLineCommandHelper.ExecuteCommand(app => sut.WatchPublic(app, true, Filename, () => { }));

            // Assert
            result.Should().Be("Error: Could not find file [MyFile.txt]. Could not watch file for changes." + Environment.NewLine);
        }

        [Theory, AutoMockData]
        public void Does_Not_Execute_Action_When_Watch_Argument_Is_True_And_File_Does_Not_Exist(
            [Frozen] IFileSystem fileSystemMock,
            CommandBaseTest sut)
        {
            // Arrange
            using var app = new CommandLineApplication();
            var counter = 0;
            fileSystemMock.FileExists(Filename).Returns(false);

            // Act
            sut.WatchPublic(app, true, Filename, () => counter++);

            // Assert
            counter.Should().Be(0);
        }

        [Theory, AutoMockData]
        public void Repeats_Action_When_File_Has_Changed(
            [Frozen] IFileSystem fileSystemMock,
            CommandBaseTest sut)
        {
            // Arrange
            using var app = new CommandLineApplication();
            var fileSystemCallCounter = 0;
            var actionCallCounter = 0;
            fileSystemMock.FileExists(Filename).Returns(_ =>
            {
                if (fileSystemCallCounter > 2)
                {
                    sut.DoAbort();
                }
                return true;
            });
            fileSystemMock.GetFileLastWriteTime(Filename).Returns(_ =>
            {
                fileSystemCallCounter++;
                return DateTime.MinValue.AddDays(fileSystemCallCounter);
            });

            // Act
            sut.WatchPublic(app, true, Filename, () => actionCallCounter++);

            // Assert
            actionCallCounter.Should().Be(4);
        }

        [Theory, AutoMockData]
        public void Breaks_Watch_Loop_When_File_Has_Been_Removed(
            [Frozen] IFileSystem fileSystemMock,
            CommandBaseTest sut)
        {
            // Arrange
            var fileSystemCallCounter = 0;
            var actionCallCounter = 0;
            fileSystemMock.FileExists(Filename).Returns(_ =>
            {
                return fileSystemCallCounter < 3;
            });
            fileSystemMock.GetFileLastWriteTime(Filename).Returns(_ =>
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
        [Theory, AutoMockData]
        public void Returns_Empty_String_On_Null_AssemblyName(CommandBaseTest sut)
        {
            // Act
            var result = sut.GetCurrentDirectoryPublic(string.Empty, assemblyName: null!);

            // Assert
            result.Should().BeEmpty();
        }

        [Theory, AutoMockData]
        public void Returns_CurrentDirectoryOption_Value_When_Present(CommandBaseTest sut)
        {
            // Act
            var result = sut.GetCurrentDirectoryPublic("CurrentDirectory", assemblyName: AssemblyName);

            // Assert
            result.Should().Be("CurrentDirectory");
        }

        [Theory, AutoMockData]
        public void Returns_Directory_Of_Assembly_When_AssemblyName_Ends_With_Dll_And_CurrentDirectory_Is_Empty(CommandBaseTest sut)
        {
            // Act
            var result = sut.GetCurrentDirectoryPublic(null, assemblyName: Path.Combine("Directory", "Assembly.dll"));

            // Assert
            result.Should().Be("Directory");
        }

        [Theory, AutoMockData]
        public void Returns_Empty_String_When_AssemblyName_Does_Not_End_With_Dll(CommandBaseTest sut)
        {
            // Act
            var result = sut.GetCurrentDirectoryPublic(null, assemblyName: AssemblyName);

            // Assert
            result.Should().BeEmpty();
        }
    }

    public class GenerateSingleOutput : CommandBaseTests
    {
        [Theory, AutoMockData]
        public void Throws_On_Null_Builder(CommandBaseTest sut)
        {
            // Act & Assert
            sut.Invoking(x => x.GenerateSingleOutputPublic(builder: null!, string.Empty))
               .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_BasePath(
            [Frozen] IMultipleContentBuilder multipleContentBuilderMock,
            CommandBaseTest sut)
        {
            // Act & Assert
            sut.Invoking(x => x.GenerateSingleOutputPublic(multipleContentBuilderMock, basePath: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("basePath");
        }

        [Theory, AutoMockData]
        public void Uses_Filename_From_Content_When_BasePath_Is_Empty(
            [Frozen] IMultipleContentBuilder multipleContentBuilderMock,
            CommandBaseTest sut)
        {
            // Arrange
            SetupMultipleContentBuilderMock(multipleContentBuilderMock, string.Empty);

            // Act
            var result = sut.GenerateSingleOutputPublic(multipleContentBuilderMock, string.Empty);

            // Assert
            result.Should().Be(@"File1.txt:
Contents from file1
File2.txt:
Contents from file2
");
        }

        [Theory, AutoMockData]
        public void Uses_Combined_Filename_When_Content_Filename_Is_PathRooted_And_BasePath_Is_Filled(
            [Frozen] IMultipleContentBuilder multipleContentBuilderMock,
            CommandBaseTest sut)
        {
            // Arrange
            SetupMultipleContentBuilderMock(multipleContentBuilderMock, Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar);

            // Act
            var result = sut.GenerateSingleOutputPublic(multipleContentBuilderMock, "BasePath" + Path.DirectorySeparatorChar);

            // Assert
            result.Should().Be($@"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}File1.txt:
Contents from file1
{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}File2.txt:
Contents from file2
");
        }

        [Theory, AutoMockData]
        public void Uses_Combined_Filename_When_Content_Filename_Is_PathRooted_And_BasePath_Is_Not_Filled(
            [Frozen] IMultipleContentBuilder multipleContentBuilderMock,
            CommandBaseTest sut)
        {
            // Arrange
            SetupMultipleContentBuilderMock(multipleContentBuilderMock, string.Empty);

            // Act
            var result = sut.GenerateSingleOutputPublic(multipleContentBuilderMock, "BasePath" + Path.DirectorySeparatorChar);

            // Assert
            result.Should().Be($@"BasePath{Path.DirectorySeparatorChar}File1.txt:
Contents from file1
BasePath{Path.DirectorySeparatorChar}File2.txt:
Contents from file2
");

        }

        [Theory, AutoMockData]
        public void Uses_Filename_From_Content_When_Content_Filename_Is_Not_PathRooted_And_BasePath_Is_Empty(
            [Frozen] IMultipleContentBuilder multipleContentBuilderMock,
            CommandBaseTest sut)
        {
            // Arrange
            SetupMultipleContentBuilderMock(multipleContentBuilderMock, string.Empty);

            // Act
            var result = sut.GenerateSingleOutputPublic(multipleContentBuilderMock, string.Empty);

            // Assert
            result.Should().Be($@"File1.txt:
Contents from file1
File2.txt:
Contents from file2
");
        }
    }

    public class WriteOutputToHost : CommandBaseTests
    {
        [Theory, AutoMockData]
        public void Throws_On_Null_App(CommandBaseTest sut)
        {
            // Act & Assert
            sut.Invoking(x => CommandLineCommandHelper.ExecuteCommand(_ => x.WriteOutputToHostPublic(app: null!, "TemplateOutput", true)))
               .Should().Throw<ArgumentNullException>().WithParameterName("app");
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_TemplateOutput(CommandBaseTest sut)
        {
            // Act & Assert
            sut.Invoking(x => CommandLineCommandHelper.ExecuteCommand(app => x.WriteOutputToHostPublic(app, templateOutput: null!, true)))
               .Should().Throw<ArgumentNullException>().WithParameterName("templateOutput");
        }

        [Theory, AutoMockData]
        public void Writes_Output_To_Host_Correctly_When_Bare_Is_True(CommandBaseTest sut)
        {
            // Act
            var result = CommandLineCommandHelper.ExecuteCommand(app => sut.WriteOutputToHostPublic(app, "TemplateOutput", true));

            // Assert
            result.Should().Be("TemplateOutput" + Environment.NewLine);
        }

        [Theory, AutoMockData]
        public void Writes_Output_To_Host_Correctly_When_Bare_Is_False(CommandBaseTest sut)
        {
            // Act
            var result = CommandLineCommandHelper.ExecuteCommand(app => sut.WriteOutputToHostPublic(app, "TemplateOutput", false));

            // Assert
            result.Should().Be(@"Code generation output:
TemplateOutput
");
        }
    }

    public class WriteOutputToClipboard : CommandBaseTests
    {
        [Theory, AutoMockData]
        public void Throws_On_Null_App(CommandBaseTest sut)
        {
            // Act & Assert
            sut.Invoking(x => CommandLineCommandHelper.ExecuteCommand(_ => x.WriteOutputToClipboardPublic(app: null!, "TemplateOutput", true)))
               .Should().Throw<ArgumentNullException>().WithParameterName("app");
        }

        [Theory, AutoMockData]
        public void Throws_On_Null_TemplateOutput(CommandBaseTest sut)
        {
            // Act & Assert
            sut.Invoking(x => CommandLineCommandHelper.ExecuteCommand(app => x.WriteOutputToClipboardPublic(app, templateOutput: null!, true)))
               .Should().Throw<ArgumentNullException>().WithParameterName("templateOutput");
        }

        [Theory, AutoMockData]
        public void Copies_Output_To_Clipboard(
            [Frozen] IClipboard clipboardMock,
            CommandBaseTest sut)
        {
            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(app => sut.WriteOutputToClipboardPublic(app, "TemplateOutput", true));

            // Assert
            clipboardMock.Received().SetText("TemplateOutput");
        }

        [Theory, AutoMockData]
        public void Writes_Message_To_Host_When_Base_Is_False(CommandBaseTest sut)
        {
            // Act
            var result = CommandLineCommandHelper.ExecuteCommand(app => sut.WriteOutputToClipboardPublic(app, "TemplateOutput", false));

            // Assert
            result.Should().Be("Copied code generation output to clipboard" + Environment.NewLine);
        }

        [Theory, AutoMockData]
        public void Does_Not_Write_Message_To_Host_When_Base_Is_True(CommandBaseTest sut)
        {
            // Act
            var result = CommandLineCommandHelper.ExecuteCommand(app => sut.WriteOutputToClipboardPublic(app, "TemplateOutput", true));

            // Assert
            result.Should().BeEmpty();
        }
    }

    public class GetDryRun : CommandBaseTests
    {
        [Theory,
            InlineAutoMockData(true, false, true),
            InlineAutoMockData(true, true, true),
            InlineAutoMockData(false, true, true),
            InlineAutoMockData(false, false, false)]
        public void Returns_Correct_Result(bool dryRun, bool clipboard, bool expectedResult, CommandBaseTest sut)
        {
            // Act
            var result = sut.GetDryRunPublic(dryRun, clipboard);

            // Assert
            result.Should().Be(expectedResult);
        }
    }

    public class GetDefaultFilename : CommandBaseTests
    {
        [Theory,
            InlineAutoMockData("Filled", "Filled"),
            InlineAutoMockData("", ""),
            InlineAutoMockData(null, "")]
        public void Returns_Correct_Result(string? defaultFilename, string expectedResult, CommandBaseTest sut)
        {
            // Act
            var result = sut.GetDefaultFilenamePublic(defaultFilename);

            // Assert
            result.Should().Be(expectedResult);
        }
    }

    public class GetBasePath : CommandBaseTests
    {
        [Theory,
            InlineAutoMockData("Filled", "Filled"),
            InlineAutoMockData("", ""),
            InlineAutoMockData(null, "")]
        public void Returns_Correct_Result(string? basePath, string expectedResult, CommandBaseTest sut)
        {
            // Act
            var result = sut.GetBasePathPublic(basePath);

            // Assert
            result.Should().Be(expectedResult);
        }
    }

    public sealed class CommandBaseTest : CommandBase
    {
        public CommandBaseTest(IClipboard clipboard, IFileSystem fileSystem, IUserInput userInput) : base(clipboard, fileSystem, userInput)
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
