﻿namespace TemplateFramework.Console.Tests.Commands;

public class RunTemplateCommandTests : TestBase<RunTemplateCommand>
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
        public async Task Empty_AssemblyName_Results_In_Error()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var output = await CommandLineCommandHelper.ExecuteCommand(sut, "--assembly ");

            // Assert
            output.Should().Be("Error: Assembly name is required." + Environment.NewLine);
        }

        [Fact]
        public async Task Empty_ClassName_Results_In_Error()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var output = await CommandLineCommandHelper.ExecuteCommand(sut, "--assembly MyAssembly", "--classname ");

            // Assert
            output.Should().Be("Error: Class name is required." + Environment.NewLine);
        }

        [Fact]
        public async Task Renders_Template_With_AssemblyName_And_ClassName()
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            var templateProviderMock = Fixture.Freeze<ITemplateProvider>();
            var templateEngineMock = Fixture.Freeze<ITemplateEngine>();
            templateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            var sut = CreateSut();

            // Act
            _ = await CommandLineCommandHelper.ExecuteCommand(sut, "--assembly MyAssembly", "--classname MyClass", "MyArgumentName:MyArgumentValue");

            // Assert
            templateEngineMock.Received().Render(Arg.Is<IRenderTemplateRequest>(req => req.Context != null && req.Context.Identifier is CompiledTemplateIdentifier));
        }

        [Fact]
        public async Task Renders_Template_With_FormattableString()
        {
            // Arrange
            var templateProviderMock = Fixture.Freeze<ITemplateProvider>();
            var templateEngineMock = Fixture.Freeze<ITemplateEngine>();
            var fileSystemMock = Fixture.Freeze<IFileSystem>();
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            templateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            fileSystemMock.FileExists("myfile.txt").Returns(true);
            fileSystemMock.ReadAllText("myfile.txt", Arg.Any<Encoding>()).Returns("template contents");
            var sut = CreateSut();

            // Act
            _ = await CommandLineCommandHelper.ExecuteCommand(sut, "--formattablestring myfile.txt");

            // Assert
            templateEngineMock.Received().Render(Arg.Is<IRenderTemplateRequest>(req => req.Context != null && req.Context.Identifier is FormattableStringTemplateIdentifier));
        }

        [Fact]
        public async Task Renders_Template_With_ExpressionString()
        {
            // Arrange
            var templateProviderMock = Fixture.Freeze<ITemplateProvider>();
            var templateEngineMock = Fixture.Freeze<ITemplateEngine>();
            var fileSystemMock = Fixture.Freeze<IFileSystem>();
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            templateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            fileSystemMock.FileExists("myfile.txt").Returns(true);
            fileSystemMock.ReadAllText("myfile.txt", Arg.Any<Encoding>()).Returns("template contents");
            var sut = CreateSut();

            // Act
            _ = await CommandLineCommandHelper.ExecuteCommand(sut, "--expressionstring myfile.txt");

            // Assert
            templateEngineMock.Received().Render(Arg.Is<IRenderTemplateRequest>(req => req.Context != null && req.Context.Identifier is ExpressionStringTemplateIdentifier));
        }

        [Fact]
        public async Task Sets_Parameters_Correctly_On_Template_Instance()
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            var templateProviderMock = Fixture.Freeze<ITemplateProvider>();
            var templateEngineMock = Fixture.Freeze<ITemplateEngine>();
            templateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            var sut = CreateSut();

            // Act
            _ = await CommandLineCommandHelper.ExecuteCommand(sut, "--assembly MyAssembly", "--classname MyClass", "MyArgumentName:MyArgumentValue");

            // Assert
            templateEngineMock.Received().Render(Arg.Is<IRenderTemplateRequest>(req =>
                req.AdditionalParameters.ToKeyValuePairs().Count() == 1
                && req.AdditionalParameters.ToKeyValuePairs().First().Key == "MyArgumentName"
                && req.AdditionalParameters.ToKeyValuePairs().First().Value != null
                && req.AdditionalParameters.ToKeyValuePairs().First().Value!.ToString() == "MyArgumentValue"));
        }

        [Fact]
        public async Task Gets_Parameter_Values_Interactively_When_Interactive_Argument_Is_Provided()
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            var templateProviderMock = Fixture.Freeze<ITemplateProvider>();
            var templateEngineMock = Fixture.Freeze<ITemplateEngine>();
            var userInputMock = Fixture.Freeze<IUserInput>();
            templateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            templateEngineMock.GetParameters(Arg.Any<object>()).Returns(new[] { new TemplateParameter(nameof(TestData.PlainTemplateWithModelAndAdditionalParameters<string>.AdditionalParameter), typeof(string)) });
            var sut = CreateSut();

            // Act
            _ = await CommandLineCommandHelper.ExecuteCommand(sut, "--assembly MyAssembly", "--classname MyClass", "--interactive");

            // Assert
            userInputMock.Received().GetValue(Arg.Any<ITemplateParameter>());
        }

        [Fact]
        public async Task Lists_Parameters_When_ListParameters_Argument_Is_Provided()
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            var templateProviderMock = Fixture.Freeze<ITemplateProvider>();
            var templateEngineMock = Fixture.Freeze<ITemplateEngine>();
            templateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            templateEngineMock.GetParameters(Arg.Any<object>()).Returns(new[] { new TemplateParameter(nameof(TestData.PlainTemplateWithModelAndAdditionalParameters<string>.AdditionalParameter), typeof(string)) });
            var sut = CreateSut();

            // Act
            var output = await CommandLineCommandHelper.ExecuteCommand(sut, "--assembly MyAssembly", "--classname MyClass", "--list-parameters", "--default parameters.txt", "--dryrun", "--bare");

            // Assert
            output.Should().Be(@"parameters.txt:
AdditionalParameter (System.String)


");
        }

        [Fact]
        public async Task Writes_ErrorMessage_On_ListParameters_When_DefaultFilename_Is_Empty()
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            var templateProviderMock = Fixture.Freeze<ITemplateProvider>();
            var templateEngineMock = Fixture.Freeze<ITemplateEngine>();
            templateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            templateEngineMock.GetParameters(Arg.Any<object>()).Returns(new[] { new TemplateParameter(nameof(TestData.PlainTemplateWithModelAndAdditionalParameters<string>.AdditionalParameter), typeof(string)) });
            var sut = CreateSut();

            // Act
            var output = await CommandLineCommandHelper.ExecuteCommand(sut, "--assembly MyAssembly", "--classname MyClass", "--list-parameters");

            // Assert
            output.Should().Be(@"Error: Default filename is required if you want to list parameters
");
        }

        [Fact]
        public async Task Writes_ErrorMessage_When_FormattableString_File_Does_Not_Exist()
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            var templateProviderMock = Fixture.Freeze<ITemplateProvider>();
            var templateEngineMock = Fixture.Freeze<ITemplateEngine>();
            var fileSystemMock = Fixture.Freeze<IFileSystem>();
            templateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            templateEngineMock.GetParameters(Arg.Any<object>()).Returns(new[] { new TemplateParameter(nameof(TestData.PlainTemplateWithModelAndAdditionalParameters<string>.AdditionalParameter), typeof(string)) });
            fileSystemMock.FileExists("myfile.txt").Returns(false);
            var sut = CreateSut();

            // Act
            var output = await CommandLineCommandHelper.ExecuteCommand(sut, "--formattablestring myfile.txt", "--default parameters.txt", "--dryrun");

            // Assert
            output.Should().Be(@"Error: File 'myfile.txt' does not exist
");
        }

        [Fact]
        public async Task Writes_ErrorMessage_When_ExpressionString_File_Does_Not_Exist()
        {
            // Arrange
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            var templateProviderMock = Fixture.Freeze<ITemplateProvider>();
            var templateEngineMock = Fixture.Freeze<ITemplateEngine>();
            var fileSystemMock = Fixture.Freeze<IFileSystem>();
            templateProviderMock.Create(Arg.Any<ITemplateIdentifier>()).Returns(templateInstance);
            templateEngineMock.GetParameters(Arg.Any<object>()).Returns(new[] { new TemplateParameter(nameof(TestData.PlainTemplateWithModelAndAdditionalParameters<string>.AdditionalParameter), typeof(string)) });
            fileSystemMock.FileExists("myfile.txt").Returns(false);
            var sut = CreateSut();

            // Act
            var output = await CommandLineCommandHelper.ExecuteCommand(sut, "--expressionstring myfile.txt", "--default parameters.txt", "--dryrun");

            // Assert
            output.Should().Be(@"Error: File 'myfile.txt' does not exist
");
        }
    }
}
