namespace TemplateFramework.Console.Tests.Commands;

public class RunTemplateCommandTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(RunTemplateCommand).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }

    public class Initialize : RunTemplateCommandTests
    {
        [Theory, AutoMockData]
        public void Initialize_Adds_Command_To_Application(RunTemplateCommand sut)
        {
            // Arrange
            using var app = new CommandLineApplication();

            // Act
            sut.Initialize(app);

            // Assert
            app.Commands.Should().ContainSingle();
        }

        [Theory, AutoMockData]
        public void Initialize_Throws_On_Null_Argument(RunTemplateCommand sut)
        {
            // Act & Assert
            sut.Invoking(x => x.Initialize(app: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("app");
        }
    }

    public class ExecuteCommand : RunTemplateCommandTests
    {
        [Theory, AutoMockData]
        public void Empty_AssemblyName_Results_In_Error(RunTemplateCommand sut)
        {
            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(sut, "--assembly ");

            // Assert
            output.Should().Be("Error: Assembly name is required." + Environment.NewLine);
        }

        [Theory, AutoMockData]
        public void Empty_ClassName_Results_In_Error(RunTemplateCommand sut)
        {
            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(sut, "--assembly MyAssembly", "--classname ");

            // Assert
            output.Should().Be("Error: Class name is required." + Environment.NewLine);
        }

        [Theory, AutoMockData]
        public void Renders_Template_With_AssemblyName_And_ClassName(
            [Frozen] ITemplateProvider templateProviderMock,
            [Frozen] ITemplateEngine templateEngineMock,
            RunTemplateCommand sut)
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            templateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, "--assembly MyAssembly", "--classname MyClass", "MyArgumentName:MyArgumentValue");

            // Assert
            templateEngineMock.Received().Render(Arg.Is<IRenderTemplateRequest>(req => req.Context != null && req.Context.Identifier is CompiledTemplateIdentifier));
        }

        [Theory, AutoMockData]
        public void Renders_Template_With_FormattableString(
            [Frozen] ITemplateProvider templateProviderMock,
            [Frozen] ITemplateEngine templateEngineMock,
            [Frozen] IFileSystem fileSystemMock,
            RunTemplateCommand sut)
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            templateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            fileSystemMock.FileExists("myfile.txt").Returns(true);
            fileSystemMock.ReadAllText("myfile.txt", Arg.Any<Encoding>()).Returns("template contents");

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, "--formattablestring myfile.txt");

            // Assert
            templateEngineMock.Received().Render(Arg.Is<IRenderTemplateRequest>(req => req.Context != null && req.Context.Identifier is FormattableStringTemplateIdentifier));
        }

        [Theory, AutoMockData]
        public void Renders_Template_With_ExpressionString(
            [Frozen] ITemplateProvider templateProviderMock,
            [Frozen] ITemplateEngine templateEngineMock,
            [Frozen] IFileSystem fileSystemMock,
            RunTemplateCommand sut)
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            templateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            fileSystemMock.FileExists("myfile.txt").Returns(true);
            fileSystemMock.ReadAllText("myfile.txt", Arg.Any<Encoding>()).Returns("template contents");

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, "--expressionstring myfile.txt");

            // Assert
            templateEngineMock.Received().Render(Arg.Is<IRenderTemplateRequest>(req => req.Context != null && req.Context.Identifier is ExpressionStringTemplateIdentifier));
        }

        [Theory, AutoMockData]
        public void Sets_Parameters_Correctly_On_Template_Instance(
            [Frozen] ITemplateProvider templateProviderMock,
            [Frozen] ITemplateEngine templateEngineMock,
            RunTemplateCommand sut)
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            templateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, "--assembly MyAssembly", "--classname MyClass", "MyArgumentName:MyArgumentValue");

            // Assert
            templateEngineMock.Received().Render(Arg.Is<IRenderTemplateRequest>(req =>
                req.AdditionalParameters.ToKeyValuePairs().Count() == 1
                && req.AdditionalParameters.ToKeyValuePairs().First().Key == "MyArgumentName"
                && req.AdditionalParameters.ToKeyValuePairs().First().Value != null
                && req.AdditionalParameters.ToKeyValuePairs().First().Value!.ToString() == "MyArgumentValue"));
        }

        [Theory, AutoMockData]
        public void Gets_Parameter_Values_Interactively_When_Interactive_Argument_Is_Provided(
            [Frozen] ITemplateProvider templateProviderMock,
            [Frozen] ITemplateEngine templateEngineMock,
            [Frozen] IUserInput userInputMock,
            RunTemplateCommand sut)
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            templateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            templateEngineMock.GetParameters(Arg.Any<object>()).Returns(new[] { new TemplateParameter(nameof(TestData.PlainTemplateWithModelAndAdditionalParameters<string>.AdditionalParameter), typeof(string)) });

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(sut, "--assembly MyAssembly", "--classname MyClass", "--interactive");

            // Assert
            userInputMock.Received().GetValue(Arg.Any<ITemplateParameter>());
        }

        [Theory, AutoMockData]
        public void Lists_Parameters_When_ListParameters_Argument_Is_Provided(
            [Frozen] ITemplateProvider templateProviderMock,
            [Frozen] ITemplateEngine templateEngineMock,
            RunTemplateCommand sut)
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            templateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            templateEngineMock.GetParameters(Arg.Any<object>()).Returns(new[] { new TemplateParameter(nameof(TestData.PlainTemplateWithModelAndAdditionalParameters<string>.AdditionalParameter), typeof(string)) });

            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(sut, "--assembly MyAssembly", "--classname MyClass", "--list-parameters", "--default parameters.txt", "--dryrun", "--bare");

            // Assert
            output.Should().Be(@"parameters.txt:
AdditionalParameter (System.String)


");
        }

        [Theory, AutoMockData]
        public void Writes_ErrorMessage_On_ListParameters_When_DefaultFilename_Is_Empty(
            [Frozen] ITemplateProvider templateProviderMock,
            [Frozen] ITemplateEngine templateEngineMock,
            RunTemplateCommand sut)
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            templateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            templateEngineMock.GetParameters(Arg.Any<object>()).Returns(new[] { new TemplateParameter(nameof(TestData.PlainTemplateWithModelAndAdditionalParameters<string>.AdditionalParameter), typeof(string)) });

            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(sut, "--assembly MyAssembly", "--classname MyClass", "--list-parameters");

            // Assert
            output.Should().Be(@"Error: Default filename is required if you want to list parameters
");
        }

        [Theory, AutoMockData]
        public void Writes_ErrorMessage_When_FormattableString_File_Does_Not_Exist(
            [Frozen] ITemplateProvider templateProviderMock,
            [Frozen] ITemplateEngine templateEngineMock,
            [Frozen] IFileSystem fileSystemMock,
            RunTemplateCommand sut)
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            templateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            templateEngineMock.GetParameters(Arg.Any<object>()).Returns(new[] { new TemplateParameter(nameof(TestData.PlainTemplateWithModelAndAdditionalParameters<string>.AdditionalParameter), typeof(string)) });
            fileSystemMock.FileExists("myfile.txt").Returns(false);

            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(sut, "--formattablestring myfile.txt", "--default parameters.txt", "--dryrun");

            // Assert
            output.Should().Be(@"Error: File 'myfile.txt' does not exist
");
        }

        [Theory, AutoMockData]
        public void Writes_ErrorMessage_When_ExpressionString_File_Does_Not_Exist(
            [Frozen] ITemplateProvider templateProviderMock,
            [Frozen] ITemplateEngine templateEngineMock,
            [Frozen] IFileSystem fileSystemMock,
            RunTemplateCommand sut)
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            templateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            templateEngineMock.GetParameters(Arg.Any<object>()).Returns(new[] { new TemplateParameter(nameof(TestData.PlainTemplateWithModelAndAdditionalParameters<string>.AdditionalParameter), typeof(string)) });
            fileSystemMock.FileExists("myfile.txt").Returns(false);

            // Act
            var output = CommandLineCommandHelper.ExecuteCommand(sut, "--expressionstring myfile.txt", "--default parameters.txt", "--dryrun");

            // Assert
            output.Should().Be(@"Error: File 'myfile.txt' does not exist
");
        }
    }
}
