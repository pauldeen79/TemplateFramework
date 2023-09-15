namespace TemplateFramework.Console.Tests.Commands;

public class CodeGenerationAssemblyCommandTests : TestBase<CodeGenerationAssemblyCommand>
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(CodeGenerationAssemblyCommand).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }

    public class Initialize : CodeGenerationAssemblyCommandTests
    {
        [Fact]
        public void Initialize_Adds_Command_To_Application()
        {
            // Arrange
            using var app = new CommandLineApplication();
            var sut = CreateSut();

            // Act
            sut.Initialize(app);

            // Assert
            app.Commands.Should().ContainSingle();
        }

        [Fact]
        public void Initialize_Throws_On_Null_Argument()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Initialize(app: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("app");
        }
    }

    public class ExecuteComand : CodeGenerationAssemblyCommandTests
    {
        [Fact]
        public void Empty_AssemblyName_Results_In_Error()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(sut);

            // Assert
            output.Should().Be("Error: Assembly name is required." + Environment.NewLine);
        }

        [Fact]
        public void Uses_Current_Directory_As_CurrentDirectory_When_AssemblyName_Is_Not_A_Filename()
        {
            // Arrange
            var codeGenerationAssembly = Fixture.Freeze<ICodeGenerationAssembly>();
            var sut = CreateSut();

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}");

            // Assert
            codeGenerationAssembly.Received().Generate(Arg.Is<ICodeGenerationAssemblySettings>(x => x.CurrentDirectory == Directory.GetCurrentDirectory()), Arg.Any<IGenerationEnvironment>());
        }

        [Fact]
        public void Uses_Directory_Of_Assembly_As_CurrentDirectory_When_AssemblyName_Is_Not_A_Filename()
        {
            // Arrange
            var codeGenerationAssembly = Fixture.Freeze<ICodeGenerationAssembly>();
            var sut = CreateSut();

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {Path.Combine(TestData.BasePath, "myassembly.dll")}");

            // Assert
            codeGenerationAssembly.Received().Generate(Arg.Is<ICodeGenerationAssemblySettings>(x => x.CurrentDirectory == TestData.BasePath), Arg.Any<IGenerationEnvironment>());
        }

        [Fact]
        public void Uses_Specified_CurrentDirectory_When_Available()
        {
            // Arrange
            var codeGenerationAssembly = Fixture.Freeze<ICodeGenerationAssembly>();
            var sut = CreateSut();

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {Path.Combine(TestData.BasePath, "myassembly.dll")}", "--directory something");

            // Assert
            codeGenerationAssembly.Received().Generate(Arg.Is<ICodeGenerationAssemblySettings>(x => x.CurrentDirectory == "something"), Arg.Any<IGenerationEnvironment>());
        }

        [Fact]
        public void Uses_Specified_BasePath_From_Arguments_When_Present()
        {
            // Arrange
            var codeGenerationAssembly = Fixture.Freeze<ICodeGenerationAssembly>();
            var sut = CreateSut();

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}", $"--path {TestData.BasePath}");

            // Assert
            codeGenerationAssembly.Received().Generate(Arg.Is<ICodeGenerationAssemblySettings>(x => x.BasePath == TestData.BasePath), Arg.Any<IGenerationEnvironment>());
        }

        [Fact]
        public void Uses_Empty_BasePath_When_Not_Present_In_Arguments()
        {
            // Arrange
            var codeGenerationAssembly = Fixture.Freeze<ICodeGenerationAssembly>();
            var sut = CreateSut();

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}");

            // Assert
            codeGenerationAssembly.Received().Generate(Arg.Is<ICodeGenerationAssemblySettings>(x => x.BasePath == string.Empty), Arg.Any<IGenerationEnvironment>());
        }

        [Fact]
        public void Uses_Specified_DefaultFIlename_From_Arguments_When_Present()
        {
            // Arrange
            var codeGenerationAssembly = Fixture.Freeze<ICodeGenerationAssembly>();
            var sut = CreateSut();
            
            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}", "--default MyFile.txt");

            // Assert
            codeGenerationAssembly.Received().Generate(Arg.Is<ICodeGenerationAssemblySettings>(x => x.DefaultFilename == "MyFile.txt"), Arg.Any<IGenerationEnvironment>());
        }

        [Fact]
        public void Uses_Empty_DefaultFilename_When_Not_Present_In_Arguments()
        {
            // Arrange
            var codeGenerationAssembly = Fixture.Freeze<ICodeGenerationAssembly>();
            var sut = CreateSut();

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}");

            // Assert
            codeGenerationAssembly.Received().Generate(Arg.Is<ICodeGenerationAssemblySettings>(x => x.DefaultFilename == string.Empty), Arg.Any<IGenerationEnvironment>());
        }

        [Fact]
        public void Uses_DryRun_When_DryRun_Option_Is_Present_In_Arguments()
        {
            // Arrange
            var codeGenerationAssembly = Fixture.Freeze<ICodeGenerationAssembly>();
            var sut = CreateSut();

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}", "--dryrun");

            // Assert
            codeGenerationAssembly.Received().Generate(Arg.Is<ICodeGenerationAssemblySettings>(x => x.DryRun), Arg.Any<IGenerationEnvironment>());
        }

        [Fact]
        public void Uses_DryRun_When_Cipboard_Option_Is_Present_In_Arguments()
        {
            // Arrange
            var codeGenerationAssembly = Fixture.Freeze<ICodeGenerationAssembly>();
            var sut = CreateSut();

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}", "--clipboard");

            // Assert
            codeGenerationAssembly.Received().Generate(Arg.Is<ICodeGenerationAssemblySettings>(x => x.DryRun), Arg.Any<IGenerationEnvironment>());
        }

        [Fact]
        public void Uses_DryRun_When_Cipboard_And_DryRun_Options_Are_Present_In_Arguments()
        {
            // Arrange
            var codeGenerationAssembly = Fixture.Freeze<ICodeGenerationAssembly>();
            var sut = CreateSut();

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}", "--clipboard", "--dryrun");

            // Assert
            codeGenerationAssembly.Received().Generate(Arg.Is<ICodeGenerationAssemblySettings>(x => x.DryRun), Arg.Any<IGenerationEnvironment>());
        }

        [Fact]
        public void Reports_Output_Directory_When_DryRun_Is_False_And_BasePath_Is_Specified()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}", $"--path {TestData.BasePath}");

            // Assert
            output.Should().Be("Written code generation output to path: " + TestData.BasePath + Environment.NewLine);
        }

        [Fact]
        public void Reports_Output_Directory_When_DryRun_Is_Not_Specified_And_BasePath_Is_Not_Specified()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}");

            // Assert
            output.Should().Be("Written code generation output to path: " + Directory.GetCurrentDirectory() + Environment.NewLine);
        }

        [Fact]
        public void Does_Not_Report_Output_Directory_When_DryRun_Is_Not_Specified_And_BareOption_Is_Specified()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}", "--bare");

            // Assert
            output.Should().BeEmpty();
        }

        [Fact]
        public void Copies_Output_To_Clipboard_When_ClipboardOption_Is_Specified()
        {
            // Arrange
            var codeGenerationAssembly = Fixture.Freeze<ICodeGenerationAssembly>();
            var clipboardMock = Fixture.Freeze<IClipboard>();
            var sut = CreateSut();

            // Arrange
            codeGenerationAssembly.When(x => x.Generate(Arg.Any<ICodeGenerationAssemblySettings>(), Arg.Any<IGenerationEnvironment>()))
                                      .Do(args =>
                                      {
                                          var x = args.ArgAt<MultipleContentBuilderEnvironment>(1);
                                          x.Builder.AddContent("MyFile.txt").Builder.Append("Hello!");
                                      });
            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}", "--clipboard");

            // Assert
            clipboardMock.Received().SetText(@"MyFile.txt:
Hello!
");
        }

        [Fact]
        public void Reports_Output_Being_Copied_To_Clipboard_When_BareOption_Is_Not_Specified()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}", "--clipboard");

            // Assert
            output.Should().Be("Copied code generation output to clipboard" + Environment.NewLine);
        }

        [Fact]
        public void Does_Not_Report_Output_Being_Copied_To_Clipboard_When_BareOption_Is_Specified()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}", "--clipboard", "--bare");

            // Assert
            output.Should().BeEmpty();
        }

        [Fact]
        public void Reports_Output_To_Host_When_DryRun_Option_Is_Present_In_Arguments_And_Clipboard_And_Bare_Options_Are_Not_Present_Without_BasePath()
        {
            // Arrange
            var codeGenerationAssembly = Fixture.Freeze<ICodeGenerationAssembly>();
            var sut = CreateSut();

            // Arrange
            codeGenerationAssembly.When(x => x.Generate(Arg.Any<ICodeGenerationAssemblySettings>(), Arg.Any<IGenerationEnvironment>()))
                                      .Do(args =>
                                      {
                                          var x = args.ArgAt<MultipleContentBuilderEnvironment>(1);
                                          x.Builder.AddContent("MyFile.txt").Builder.Append("Hello!");
                                      });

            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}", "--dryrun");

            // Assert
            output.Should().Be("Code generation output:" + Environment.NewLine + @"MyFile.txt:
Hello!
" + Environment.NewLine);
        }

        [Fact]
        public void Reports_Output_To_Host_When_DryRun_Option_Is_Present_In_Arguments_And_Clipboard_And_Bare_Options_Are_Not_Present_With_BasePath()
        {
            // Arrange
            var codeGenerationAssembly = Fixture.Freeze<ICodeGenerationAssembly>();
            codeGenerationAssembly.When(x => x.Generate(Arg.Any<ICodeGenerationAssemblySettings>(), Arg.Any<IGenerationEnvironment>()))
                                      .Do(args =>
                                      {
                                          var x = args.ArgAt<MultipleContentBuilderEnvironment>(1);
                                          x.Builder.AddContent("MyFile.txt").Builder.Append("Hello!");
                                      });
            var sut = CreateSut();

            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}", "--dryrun", $"--path {TestData.BasePath}");

            // Assert
            output.Should().Be("Code generation output:" + Environment.NewLine + @$"{Path.Combine(TestData.BasePath, "MyFile.txt")}:
Hello!
" + Environment.NewLine);
        }
    }
}
