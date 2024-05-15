﻿namespace TemplateFramework.Console.Tests.Commands;

public class CommandBaseTests : TestBase<CommandBaseTests.CommandBaseTest>
{
    protected const string AssemblyName = "MyAssemblyName";
    protected const string Filename = "MyFile.txt";

    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(CommandBaseTest).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }

    public class Watch : CommandBaseTests
    {
        private IFileSystem fileSystem { get; }

        public Watch()
        {
            fileSystem = Fixture.Freeze<IFileSystem>();
        }

        [Fact]
        public void Throws_On_Null_App()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Awaiting(x => x.WatchPublic(app: null!, false, Filename, () => Task.CompletedTask))
               .Should().ThrowAsync<ArgumentNullException>().WithParameterName("app");
        }

        [Fact]
        public void Throws_On_Null_Filename()
        {
            // Arrange
            using var app = new CommandLineApplication();
            var sut = CreateSut();

            // Act & Assert
            sut.Awaiting(x => x.WatchPublic(app, false, filename: null!, () => Task.CompletedTask))
               .Should().ThrowAsync<ArgumentNullException>().WithParameterName("filename");
        }

        [Fact]
        public void Throws_On_Null_Action()
        {
            // Arrange
            using var app = new CommandLineApplication();
            var sut = CreateSut();

            // Act & Assert
            sut.Awaiting(x => x.WatchPublic(app, false, Filename, action: null!))
               .Should().ThrowAsync<ArgumentNullException>().WithParameterName("action");
        }

        [Fact]
        public async Task Executes_Action_Once_When_Watch_Argument_Is_False()
        {
            // Arrange
            using var app = new CommandLineApplication();
            var counter = 0;
            var sut = CreateSut();

            // Act
            await sut.WatchPublic(app, false, Filename, () => { counter++; return Task.CompletedTask; });

            // Assert
            counter.Should().Be(1);
        }

        [Fact]
        public async Task Writes_ErrorMessage_When_Watch_Argument_Is_True_And_File_Does_Not_Exist()
        {
            // Arrange
            fileSystem.FileExists(Filename).Returns(false);
            var sut = CreateSut();

            // Act
            var result = await CommandLineCommandHelper.ExecuteCommand(async app => await sut.WatchPublic(app, true, Filename, () => Task.CompletedTask).ConfigureAwait(false));

            // Assert
            result.Should().Be("Error: Could not find file [MyFile.txt]. Could not watch file for changes." + Environment.NewLine);
        }

        [Fact]
        public async Task Does_Not_Execute_Action_When_Watch_Argument_Is_True_And_File_Does_Not_Exist()
        {
            // Arrange
            using var app = new CommandLineApplication();
            var counter = 0;
            fileSystem.FileExists(Filename).Returns(false);
            var sut = CreateSut();
            var task = () => new Task(() => { counter++; });

            // Act
            await sut.WatchPublic(app, true, Filename, task);

            // Assert
            counter.Should().Be(0);
        }

        [Fact]
        public async Task Repeats_Action_When_File_Has_Changed()
        {
            // Arrange
            using var app = new CommandLineApplication();
            var sut = CreateSut();
            var fileSystemCallCounter = 0;
            var actionCallCounter = 0;
            fileSystem.FileExists(Filename).Returns(_ =>
            {
                if (fileSystemCallCounter > 2)
                {
                    sut.DoAbort();
                }
                return true;
            });
            fileSystem.GetFileLastWriteTime(Filename).Returns(_ =>
            {
                fileSystemCallCounter++;
                return DateTime.MinValue.AddDays(fileSystemCallCounter);
            });

            // Act
            await sut.WatchPublic(app, true, Filename, () => { actionCallCounter++; return Task.CompletedTask; });

            // Assert
            actionCallCounter.Should().Be(4);
        }

        [Fact]
        public async Task Breaks_Watch_Loop_When_File_Has_Been_Removed()
        {
            // Arrange
            var fileSystemCallCounter = 0;
            var actionCallCounter = 0;
            var sut = CreateSut();
            fileSystem.FileExists(Filename).Returns(_ =>
            {
                return fileSystemCallCounter < 3;
            });
            fileSystem.GetFileLastWriteTime(Filename).Returns(_ =>
            {
                fileSystemCallCounter++;
                return DateTime.MinValue.AddDays(fileSystemCallCounter);
            });

            // Act
            var result = await CommandLineCommandHelper.ExecuteCommand(app => sut.WatchPublic(app, true, Filename, () => { actionCallCounter++; return Task.CompletedTask; }));

            // Assert
            result.Should().Be(@"Watching file [MyFile.txt] for changes...
Error: Could not find file [MyFile.txt]. Could not watch file for changes.
");
        }
    }

    public class GetCurrentDirectory : CommandBaseTests
    {
        [Fact]
        public void Returns_Empty_String_On_Null_AssemblyName()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetCurrentDirectoryPublic(string.Empty, assemblyName: null!);

            // Assert
            result.Should().BeEmpty();
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
        private IMultipleContentBuilder multipleContentBuilder { get; }
        
        public GenerateSingleOutput()
        {
            multipleContentBuilder = Fixture.Freeze<IMultipleContentBuilder>();
        }

        private void SetupMultipleContentBuilder(string filenamePrefix)
        {
            var contentBuilderMock1 = Substitute.For<IContent>();
            contentBuilderMock1.Filename.Returns($"{filenamePrefix}File1.txt");
            contentBuilderMock1.Contents.Returns("Contents from file1");

            var contentBuilderMock2 = Substitute.For<IContent>();
            contentBuilderMock2.Filename.Returns($"{filenamePrefix}File2.txt");
            contentBuilderMock2.Contents.Returns("Contents from file2");

            var multipleContentMock = Substitute.For<IMultipleContent>();
            multipleContentMock.Contents.Returns(new[] { contentBuilderMock1, contentBuilderMock2 });

            multipleContentBuilder.Build().Returns(multipleContentMock);
        }

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
            sut.Invoking(x => x.GenerateSingleOutputPublic(multipleContentBuilder, basePath: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("basePath");
        }

        [Fact]
        public void Uses_Filename_From_Content_When_BasePath_Is_Empty()
        {
            // Arrange
            SetupMultipleContentBuilder(string.Empty);
            var sut = CreateSut();

            // Act
            var result = sut.GenerateSingleOutputPublic(multipleContentBuilder, string.Empty);

            // Assert
            result.Should().Be(@"File1.txt:
Contents from file1
File2.txt:
Contents from file2
");
        }

        [Fact]
        public void Uses_Combined_Filename_When_Content_Filename_Is_PathRooted_And_BasePath_Is_Filled()
        {
            // Arrange
            SetupMultipleContentBuilder(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar);
            var sut = CreateSut();

            // Act
            var result = sut.GenerateSingleOutputPublic(multipleContentBuilder, "BasePath" + Path.DirectorySeparatorChar);

            // Assert
            result.Should().Be($@"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}File1.txt:
Contents from file1
{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}File2.txt:
Contents from file2
");
        }

        [Fact]
        public void Uses_Combined_Filename_When_Content_Filename_Is_PathRooted_And_BasePath_Is_Not_Filled()
        {
            // Arrange
            var sut = CreateSut();
            SetupMultipleContentBuilder(string.Empty);

            // Act
            var result = sut.GenerateSingleOutputPublic(multipleContentBuilder, "BasePath" + Path.DirectorySeparatorChar);

            // Assert
            result.Should().Be($@"BasePath{Path.DirectorySeparatorChar}File1.txt:
Contents from file1
BasePath{Path.DirectorySeparatorChar}File2.txt:
Contents from file2
");
        }

        [Fact]
        public void Uses_Filename_From_Content_When_Content_Filename_Is_Not_PathRooted_And_BasePath_Is_Empty()
        {
            // Arrange
            var sut = CreateSut();
            SetupMultipleContentBuilder(string.Empty);

            // Act
            var result = sut.GenerateSingleOutputPublic(multipleContentBuilder, string.Empty);

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
        [Fact]
        public void Throws_On_Null_App()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Awaiting(x => CommandLineCommandHelper.ExecuteCommand(_ => x.WriteOutputToHostPublic(app: null!, "TemplateOutput", true)))
               .Should().ThrowAsync<ArgumentNullException>().WithParameterName("app");
        }

        [Fact]
        public void Throws_On_Null_TemplateOutput()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => CommandLineCommandHelper.ExecuteCommand(app => x.WriteOutputToHostPublic(app, templateOutput: null!, true)))
               .Should().ThrowAsync<ArgumentNullException>().WithParameterName("templateOutput");
        }

        [Fact]
        public async Task Writes_Output_To_Host_Correctly_When_Bare_Is_True()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await CommandLineCommandHelper.ExecuteCommand(app => sut.WriteOutputToHostPublic(app, "TemplateOutput", true));

            // Assert
            result.Should().Be("TemplateOutput" + Environment.NewLine);
        }

        [Fact]
        public async Task Writes_Output_To_Host_Correctly_When_Bare_Is_False()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await CommandLineCommandHelper.ExecuteCommand(app => sut.WriteOutputToHostPublic(app, "TemplateOutput", false));

            // Assert
            result.Should().Be(@"Code generation output:
TemplateOutput
");
        }
    }

    public class WriteOutputToClipboard : CommandBaseTests
    {
        private IClipboard clipboard { get; }

        public WriteOutputToClipboard()
        {
            clipboard = Fixture.Freeze<IClipboard>();
        }

        [Fact]
        public void Throws_On_Null_App()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Awaiting(x => CommandLineCommandHelper.ExecuteCommand(_ => x.WriteOutputToClipboardPublic(app: null!, "TemplateOutput", true)))
               .Should().ThrowAsync<ArgumentNullException>().WithParameterName("app");
        }

        [Fact]
        public void Throws_On_Null_TemplateOutput()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => CommandLineCommandHelper.ExecuteCommand(app => x.WriteOutputToClipboardPublic(app, templateOutput: null!, true)))
               .Should().ThrowAsync<ArgumentNullException>().WithParameterName("templateOutput");
        }

        [Fact]
        public async Task Copies_Output_To_Clipboard()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            _ = await CommandLineCommandHelper.ExecuteCommand(app => sut.WriteOutputToClipboardPublic(app, "TemplateOutput", true));

            // Assert
            await clipboard.Received().SetTextAsync("TemplateOutput");
        }

        [Fact]
        public async Task Writes_Message_To_Host_When_Base_Is_False()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await CommandLineCommandHelper.ExecuteCommand(app => sut.WriteOutputToClipboardPublic(app, "TemplateOutput", false));

            // Assert
            result.Should().Be("Copied code generation output to clipboard" + Environment.NewLine);
        }

        [Fact]
        public async Task Does_Not_Write_Message_To_Host_When_Base_Is_True()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await CommandLineCommandHelper.ExecuteCommand(app => sut.WriteOutputToClipboardPublic(app, "TemplateOutput", true));

            // Assert
            result.Should().BeEmpty();
        }
    }

    public class GetDryRun : CommandBaseTests
    {
        [Theory,
            InlineData(true, false, true),
            InlineData(true, true, true),
            InlineData(false, true, true),
            InlineData(false, false, false)]
        public void Returns_Correct_Result(bool dryRun, bool clipboard, bool expectedResult)
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetDryRunPublic(dryRun, clipboard);

            // Assert
            result.Should().Be(expectedResult);
        }
    }

    public class GetDefaultFilename : CommandBaseTests
    {
        [Theory,
            InlineData("Filled", "Filled"),
            InlineData("", ""),
            InlineData(null, "")]
        public void Returns_Correct_Result(string? defaultFilename, string expectedResult)
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetDefaultFilenamePublic(defaultFilename);

            // Assert
            result.Should().Be(expectedResult);
        }
    }

    public class GetBasePath : CommandBaseTests
    {
        [Theory,
            InlineData("Filled", "Filled"),
            InlineData("", ""),
            InlineData(null, "")]
        public void Returns_Correct_Result(string? basePath, string expectedResult)
        {
            // Arrange
            var sut = CreateSut();

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

        public Task WatchPublic(CommandLineApplication app, bool watch, string filename, Func<Task> action)
            => Watch(app, watch, filename, action);

        public string? GetCurrentDirectoryPublic(string? currentDirectory, string assemblyName)
            => GetCurrentDirectory(currentDirectory, assemblyName);

        public string GenerateSingleOutputPublic(IMultipleContentBuilder builder, string basePath)
            => GenerateSingleOutput(builder, basePath);

        public Task WriteOutputToHostPublic(CommandLineApplication app, string templateOutput, bool bare)
            => WriteOutputToHost(app, templateOutput, bare);

        public Task WriteOutputPublic(CommandLineApplication app, MultipleContentBuilderEnvironment generationEnvironment, string basePath, bool bare, bool clipboard, bool dryRun)
            => WriteOutput(app, generationEnvironment, basePath, bare, clipboard, dryRun);

        public Task WriteOutputToClipboardPublic(CommandLineApplication app, string templateOutput, bool bare)
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
