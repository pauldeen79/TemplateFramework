namespace TemplateFramework.Console.Tests.Commands;

public class CodeGenerationAssemblyCommandTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            typeof(CodeGenerationAssemblyCommand).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }

    public class Initialize : CodeGenerationAssemblyCommandTests
    {
        [Theory, AutoMockData]
        public void Initialize_Adds_Command_To_Application(CodeGenerationAssemblyCommand sut)
        {
            // Arrange
            using var app = new CommandLineApplication();

            // Act
            sut.Initialize(app);

            // Assert
            app.Commands.Should().ContainSingle();
        }

        [Theory, AutoMockData]
        public void Initialize_Throws_On_Null_Argument(CodeGenerationAssemblyCommand sut)
        {
            // Act & Assert
            sut.Invoking(x => x.Initialize(app: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("app");
        }
    }

    public class ExecuteComand : CodeGenerationAssemblyCommandTests
    {
        [Theory, AutoMockData]
        public void Empty_AssemblyName_Results_In_Error(CodeGenerationAssemblyCommand sut)
        {
            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(sut);

            // Assert
            output.Should().Be("Error: Assembly name is required." + Environment.NewLine);
        }

        [Theory, AutoMockData]
        public void Uses_Current_Directory_As_CurrentDirectory_When_AssemblyName_Is_Not_A_Filename(
            [Frozen] ICodeGenerationAssembly codeGenerationAssembly,
            CodeGenerationAssemblyCommand sut)
        {
            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}");

            // Assert
            codeGenerationAssembly.Received().Generate(Arg.Is<ICodeGenerationAssemblySettings>(x => x.CurrentDirectory == Directory.GetCurrentDirectory()), Arg.Any<IGenerationEnvironment>());
        }

        [Theory, AutoMockData]
        public void Uses_Directory_Of_Assembly_As_CurrentDirectory_When_AssemblyName_Is_Not_A_Filename(
            [Frozen] ICodeGenerationAssembly codeGenerationAssembly,
            CodeGenerationAssemblyCommand sut)
        {
            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {Path.Combine(TestData.BasePath, "myassembly.dll")}");

            // Assert
            codeGenerationAssembly.Received().Generate(Arg.Is<ICodeGenerationAssemblySettings>(x => x.CurrentDirectory == TestData.BasePath), Arg.Any<IGenerationEnvironment>());
        }

        [Theory, AutoMockData]
        public void Uses_Specified_CurrentDirectory_When_Available(
            [Frozen] ICodeGenerationAssembly codeGenerationAssembly,
            CodeGenerationAssemblyCommand sut)
        {
            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {Path.Combine(TestData.BasePath, "myassembly.dll")}", "--directory something");

            // Assert
            codeGenerationAssembly.Received().Generate(Arg.Is<ICodeGenerationAssemblySettings>(x => x.CurrentDirectory == "something"), Arg.Any<IGenerationEnvironment>());
        }

        [Theory, AutoMockData]
        public void Uses_Specified_BasePath_From_Arguments_When_Present(
            [Frozen] ICodeGenerationAssembly codeGenerationAssembly,
            CodeGenerationAssemblyCommand sut)
        {
            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}", $"--path {TestData.BasePath}");

            // Assert
            codeGenerationAssembly.Received().Generate(Arg.Is<ICodeGenerationAssemblySettings>(x => x.BasePath == TestData.BasePath), Arg.Any<IGenerationEnvironment>());
        }

        [Theory, AutoMockData]
        public void Uses_Empty_BasePath_When_Not_Present_In_Arguments(
            [Frozen] ICodeGenerationAssembly codeGenerationAssembly,
            CodeGenerationAssemblyCommand sut)
        {
            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}");

            // Assert
            codeGenerationAssembly.Received().Generate(Arg.Is<ICodeGenerationAssemblySettings>(x => x.BasePath == string.Empty), Arg.Any<IGenerationEnvironment>());
        }

        [Theory, AutoMockData]
        public void Uses_Specified_DefaultFIlename_From_Arguments_When_Present(
            [Frozen] ICodeGenerationAssembly codeGenerationAssembly,
            CodeGenerationAssemblyCommand sut)
        {
            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}", "--default MyFile.txt");

            // Assert
            codeGenerationAssembly.Received().Generate(Arg.Is<ICodeGenerationAssemblySettings>(x => x.DefaultFilename == "MyFile.txt"), Arg.Any<IGenerationEnvironment>());
        }

        [Theory, AutoMockData]
        public void Uses_Empty_DefaultFilename_When_Not_Present_In_Arguments(
            [Frozen] ICodeGenerationAssembly codeGenerationAssembly,
            CodeGenerationAssemblyCommand sut)
        {
            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}");

            // Assert
            codeGenerationAssembly.Received().Generate(Arg.Is<ICodeGenerationAssemblySettings>(x => x.DefaultFilename == string.Empty), Arg.Any<IGenerationEnvironment>());
        }

        [Theory, AutoMockData]
        public void Uses_DryRun_When_DryRun_Option_Is_Present_In_Arguments(
            [Frozen] ICodeGenerationAssembly codeGenerationAssembly,
            CodeGenerationAssemblyCommand sut)
        {
            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}", "--dryrun");

            // Assert
            codeGenerationAssembly.Received().Generate(Arg.Is<ICodeGenerationAssemblySettings>(x => x.DryRun), Arg.Any<IGenerationEnvironment>());
        }

        [Theory, AutoMockData]
        public void Uses_DryRun_When_Cipboard_Option_Is_Present_In_Arguments(
            [Frozen] ICodeGenerationAssembly codeGenerationAssembly,
            CodeGenerationAssemblyCommand sut)
        {
            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}", "--clipboard");

            // Assert
            codeGenerationAssembly.Received().Generate(Arg.Is<ICodeGenerationAssemblySettings>(x => x.DryRun), Arg.Any<IGenerationEnvironment>());
        }

        [Theory, AutoMockData]
        public void Uses_DryRun_When_Cipboard_And_DryRun_Options_Are_Present_In_Arguments(
            [Frozen] ICodeGenerationAssembly codeGenerationAssembly,
            CodeGenerationAssemblyCommand sut)
        {
            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}", "--clipboard", "--dryrun");

            // Assert
            codeGenerationAssembly.Received().Generate(Arg.Is<ICodeGenerationAssemblySettings>(x => x.DryRun), Arg.Any<IGenerationEnvironment>());
        }

        [Theory, AutoMockData]
        public void Reports_Output_Directory_When_DryRun_Is_False_And_BasePath_Is_Specified(CodeGenerationAssemblyCommand sut)
        {
            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}", $"--path {TestData.BasePath}");

            // Assert
            output.Should().Be("Written code generation output to path: " + TestData.BasePath + Environment.NewLine);
        }

        [Theory, AutoMockData]
        public void Reports_Output_Directory_When_DryRun_Is_Not_Specified_And_BasePath_Is_Not_Specified(CodeGenerationAssemblyCommand sut)
        {
            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}");

            // Assert
            output.Should().Be("Written code generation output to path: " + Directory.GetCurrentDirectory() + Environment.NewLine);
        }

        [Theory, AutoMockData]
        public void Does_Not_Report_Output_Directory_When_DryRun_Is_Not_Specified_And_BareOption_Is_Specified(CodeGenerationAssemblyCommand sut)
        {
            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}", "--bare");

            // Assert
            output.Should().BeEmpty();
        }

        [Theory, AutoMockData]
        public void Copies_Output_To_Clipboard_When_ClipboardOption_Is_Specified(
            [Frozen] ICodeGenerationAssembly codeGenerationAssembly,
            [Frozen] IClipboard clipboardMock,
            CodeGenerationAssemblyCommand sut)
        {
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

        [Theory, AutoMockData]
        public void Reports_Output_Being_Copied_To_Clipboard_When_BareOption_Is_Not_Specified(CodeGenerationAssemblyCommand sut)
        {
            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}", "--clipboard");

            // Assert
            output.Should().Be("Copied code generation output to clipboard" + Environment.NewLine);
        }

        [Theory, AutoMockData]
        public void Does_Not_Report_Output_Being_Copied_To_Clipboard_When_BareOption_Is_Specified(CodeGenerationAssemblyCommand sut)
        {
            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}", "--clipboard", "--bare");

            // Assert
            output.Should().BeEmpty();
        }

        [Theory, AutoMockData]
        public void Reports_Output_To_Host_When_DryRun_Option_Is_Present_In_Arguments_And_Clipboard_And_Bare_Options_Are_Not_Present_Without_BasePath(
            [Frozen] ICodeGenerationAssembly codeGenerationAssembly,
            CodeGenerationAssemblyCommand sut)
        {
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

        [Theory, AutoMockData]
        public void Reports_Output_To_Host_When_DryRun_Option_Is_Present_In_Arguments_And_Clipboard_And_Bare_Options_Are_Not_Present_With_BasePath(
            [Frozen] ICodeGenerationAssembly codeGenerationAssembly,
            CodeGenerationAssemblyCommand sut)
        {
            // Arrange
            codeGenerationAssembly.When(x => x.Generate(Arg.Any<ICodeGenerationAssemblySettings>(), Arg.Any<IGenerationEnvironment>()))
                                      .Do(args =>
                                      {
                                          var x = args.ArgAt<MultipleContentBuilderEnvironment>(1);
                                          x.Builder.AddContent("MyFile.txt").Builder.Append("Hello!");
                                      });

            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(sut, $"--name {GetType().Assembly.FullName}", "--dryrun", $"--path {TestData.BasePath}");

            // Assert
            output.Should().Be("Code generation output:" + Environment.NewLine + @$"{Path.Combine(TestData.BasePath, "MyFile.txt")}:
Hello!
" + Environment.NewLine);
        }
    }
}
