namespace TemplateFramework.Console.Tests.Commands;

public class RunTemplateCommandTests
{
    protected IClipboard ClipboardMock { get; } = Substitute.For<IClipboard>();
    protected IFileSystem FileSystemMock { get; } = Substitute.For<IFileSystem>();
    protected ITemplateProvider TemplateProviderMock { get; } = Substitute.For<ITemplateProvider>();
    protected ITemplateEngine TemplateEngineMock { get; } = Substitute.For<ITemplateEngine>();
    protected IUserInput UserInputMock { get; } = Substitute.For<IUserInput>();

    private RunTemplateCommand CreateSut() => new(ClipboardMock, FileSystemMock, UserInputMock, TemplateProviderMock, TemplateEngineMock);

    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            TestHelpers.ConstructorMustThrowArgumentNullException(typeof(RunTemplateCommand), t => Substitute.For(new[] { t }, Array.Empty<object>()));
        }
    }

    public class Initialize : RunTemplateCommandTests
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

    public class ExecuteCommand : RunTemplateCommandTests
    {
        [Fact]
        public void Empty_AssemblyName_Results_In_Error()
        {
            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(CreateSut, "--assembly ");

            // Assert
            output.Should().Be("Error: Assembly name is required." + Environment.NewLine);
        }

        [Fact]
        public void Empty_ClassName_Results_In_Error()
        {
            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(CreateSut, "--assembly MyAssembly", "--classname ");

            // Assert
            output.Should().Be("Error: Class name is required." + Environment.NewLine);
        }

        [Fact]
        public void Renders_Template_With_AssemblyName_And_ClassName()
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            TemplateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(CreateSut, "--assembly MyAssembly", "--classname MyClass", "MyArgumentName:MyArgumentValue");

            // Assert
            TemplateEngineMock.Received().Render(Arg.Is<IRenderTemplateRequest>(req => req.Context != null && req.Context.Identifier is CompiledTemplateIdentifier));
        }

        [Fact]
        public void Renders_Template_With_FormattableString()
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            TemplateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            FileSystemMock.FileExists("myfile.txt").Returns(true);
            FileSystemMock.ReadAllText("myfile.txt", Arg.Any<Encoding>()).Returns("template contents");

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(CreateSut, "--formattablestring myfile.txt");

            // Assert
            TemplateEngineMock.Received().Render(Arg.Is<IRenderTemplateRequest>(req => req.Context != null && req.Context.Identifier is FormattableStringTemplateIdentifier));
        }

        [Fact]
        public void Renders_Template_With_ExpressionString()
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            TemplateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            FileSystemMock.FileExists("myfile.txt").Returns(true);
            FileSystemMock.ReadAllText("myfile.txt", Arg.Any<Encoding>()).Returns("template contents");

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(CreateSut, "--expressionstring myfile.txt");

            // Assert
            TemplateEngineMock.Received().Render(Arg.Is<IRenderTemplateRequest>(req => req.Context != null && req.Context.Identifier is ExpressionStringTemplateIdentifier));
        }

        [Fact]
        public void Sets_Parameters_Correctly_On_Template_Instance()
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            TemplateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(CreateSut, "--assembly MyAssembly", "--classname MyClass", "MyArgumentName:MyArgumentValue");

            // Assert
            TemplateEngineMock.Received().Render(Arg.Is<IRenderTemplateRequest>(req =>
                req.AdditionalParameters.ToKeyValuePairs().Count() == 1
                && req.AdditionalParameters.ToKeyValuePairs().First().Key == "MyArgumentName"
                && req.AdditionalParameters.ToKeyValuePairs().First().Value != null
                && req.AdditionalParameters.ToKeyValuePairs().First().Value!.ToString() == "MyArgumentValue"));
        }

        [Fact]
        public void Gets_Parameter_Values_Interactively_When_Interactive_Argument_Is_Provided()
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            TemplateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            TemplateEngineMock.GetParameters(Arg.Any<object>()).Returns(new[] { new TemplateParameter(nameof(TestData.PlainTemplateWithModelAndAdditionalParameters<string>.AdditionalParameter), typeof(string)) });

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(CreateSut, "--assembly MyAssembly", "--classname MyClass", "--interactive");

            // Assert
            UserInputMock.Received().GetValue(Arg.Any<ITemplateParameter>());
        }

        [Fact]
        public void Lists_Parameters_When_ListParameters_Argument_Is_Provided()
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            TemplateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            TemplateEngineMock.GetParameters(Arg.Any<object>()).Returns(new[] { new TemplateParameter(nameof(TestData.PlainTemplateWithModelAndAdditionalParameters<string>.AdditionalParameter), typeof(string)) });

            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(CreateSut, "--assembly MyAssembly", "--classname MyClass", "--list-parameters", "--default parameters.txt", "--dryrun", "--bare");

            // Assert
            output.Should().Be(@"parameters.txt:
AdditionalParameter (System.String)


");
        }

        [Fact]
        public void Writes_ErrorMessage_On_ListParameters_When_DefaultFilename_Is_Empty()
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            TemplateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            TemplateEngineMock.GetParameters(Arg.Any<object>()).Returns(new[] { new TemplateParameter(nameof(TestData.PlainTemplateWithModelAndAdditionalParameters<string>.AdditionalParameter), typeof(string)) });

            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(CreateSut, "--assembly MyAssembly", "--classname MyClass", "--list-parameters");

            // Assert
            output.Should().Be(@"Error: Default filename is required if you want to list parameters
");
        }

        [Fact]
        public void Writes_ErrorMessage_When_FormattableString_File_Does_Not_Exist()
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            TemplateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            TemplateEngineMock.GetParameters(Arg.Any<object>()).Returns(new[] { new TemplateParameter(nameof(TestData.PlainTemplateWithModelAndAdditionalParameters<string>.AdditionalParameter), typeof(string)) });

            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(CreateSut, "--formattablestring myfile.txt", "--default parameters.txt", "--dryrun");

            // Assert
            output.Should().Be(@"Error: File 'myfile.txt' does not exist
");
        }

        [Fact]
        public void Writes_ErrorMessage_When_ExpressionString_File_Does_Not_Exist()
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            TemplateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            TemplateEngineMock.GetParameters(Arg.Any<object>()).Returns(new[] { new TemplateParameter(nameof(TestData.PlainTemplateWithModelAndAdditionalParameters<string>.AdditionalParameter), typeof(string)) });

            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(CreateSut, "--expressionstring myfile.txt", "--default parameters.txt", "--dryrun");

            // Assert
            output.Should().Be(@"Error: File 'myfile.txt' does not exist
");
        }

    }
}
