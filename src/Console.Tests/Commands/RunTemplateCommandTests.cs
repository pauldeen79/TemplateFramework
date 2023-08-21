namespace TemplateFramework.Console.Tests.Commands;

public class RunTemplateCommandTests
{
    protected Mock<IClipboard> ClipboardMock { get; } = new();
    protected Mock<ITemplateProvider> TemplateProviderMock { get; } = new();
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();

    private RunTemplateCommand CreateSut() => new(ClipboardMock.Object, TemplateProviderMock.Object, TemplateEngineMock.Object);

    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            TestHelpers.ConstructorMustThrowArgumentNullException(typeof(RunTemplateCommand));
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
            var result = CommandLineCommandHelper.ExecuteCommand(CreateSut, "--assembly ");

            // Assert
            result.Should().Be("Error: Assembly name is required." + Environment.NewLine);
        }

        [Fact]
        public void Empty_ClassName_Results_In_Error()
        {
            // Act
            var result = CommandLineCommandHelper.ExecuteCommand(CreateSut, "--assembly MyAssembly", "--classname ");

            // Assert
            result.Should().Be("Error: Class name is required." + Environment.NewLine);
        }

        [Fact]
        public void Sets_Parameters_Correctly_On_Template_Instance()
        {
            // Arrange
            var sut = CreateSut();
            var templateInstance = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            TemplateProviderMock.Setup(x => x.Create(It.IsAny<ICreateTemplateRequest>())).Returns(templateInstance);

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(CreateSut, "--assembly MyAssembly", "--classname MyClass", "MyArgumentName:MyArgumentValue");

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.Is<IRenderTemplateRequest>(req =>
                req.AdditionalParameters.ToKeyValuePairs().Count() == 1
                && req.AdditionalParameters.ToKeyValuePairs().First().Key == "MyArgumentName"
                && req.AdditionalParameters.ToKeyValuePairs().First().Value != null
                && req.AdditionalParameters.ToKeyValuePairs().First().Value!.ToString() == "MyArgumentValue")), Times.Once);
        }
    }
}
