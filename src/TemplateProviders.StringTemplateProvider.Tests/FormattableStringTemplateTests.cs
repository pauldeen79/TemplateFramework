namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests;

public class FormattableStringTemplateTests
{
    protected const string Template = "Hello {Name}!";
    protected IFormattableStringParser FormattableStringParserMock { get; } = Substitute.For<IFormattableStringParser>();
    protected FormattableStringTemplateIdentifier Identifier { get; } = new FormattableStringTemplateIdentifier(Template, CultureInfo.CurrentCulture);
    protected ComponentRegistrationContext ComponentRegistrationContext { get; } = new();

    protected FormattableStringTemplate CreateSut() => new(Identifier, FormattableStringParserMock, ComponentRegistrationContext);

    public class Constructor : FormattableStringTemplateTests
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            typeof(FormattableStringTemplate).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(
                p => !new[] { "model", "iterationNumber", "iterationCount" }.Contains(p.Name),
                p => p.Name switch
                {
                    "formattableStringTemplateIdentifier" => new FormattableStringTemplateIdentifier("template", CultureInfo.InvariantCulture),
                    "componentRegistrationContext" => new ComponentRegistrationContext(),
                    _ => null
                });
        }
    }

    public class GetParameters : FormattableStringTemplateTests
    {
        [Fact]
        public void Returns_Parameters_From_Template()
        {
            // Arrange
            var sut = CreateSut();
            FormattableStringParserMock.Parse(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<TemplateFrameworkStringContext>())
                .Returns(x =>
                {
                    // Note that in this unit test, we have to mock the behavior of FormattableStringParser :)
                    // There is also an Integration test to prove it works in real life ;-)
                    x.ArgAt<TemplateFrameworkStringContext>(2).ParameterNamesList.Add("Name");
                    return Result<string>.Success(string.Empty);
                });

            // Act
            var result = sut.GetParameters();

            // Assert
            result.Select(x => x.Name).Should().BeEquivalentTo("Name");
        }
    }

    public class Render : FormattableStringTemplateTests
    {
        [Fact]
        public void Throws_On_Null_Builder()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Render(builder: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_NonSuccesful_Result_From_FormattableStringParser()
        {
            // Arrange
            FormattableStringParserMock.Parse(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<TemplateFrameworkStringContext>())
                .Returns(Result<string>.Error("Kaboom!"));
            var sut = CreateSut();
            var builder = new StringBuilder();

            // Act & Assert
            sut.Invoking(x => x.Render(builder))
               .Should().Throw<InvalidOperationException>().WithMessage("Result: Error, ErrorMessage: Kaboom!");
        }

        [Fact]
        public void Appends_Result_From_FormattableStringParser_To_Builder_On_Succesful_Result()
        {
            // Arrange
            FormattableStringParserMock.Parse(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<TemplateFrameworkStringContext>())
                .Returns(Result<string>.Success("Parse result"));
            var sut = CreateSut();
            var builder = new StringBuilder();

            // Act
            sut.Render(builder);

            // Assert
            builder.ToString().Should().Be("Parse result");
        }
    }

    public class SetParameter : FormattableStringTemplateTests
    {
        [Fact]
        public void Adds_Parameter_To_Context()
        {
            // Arrange
            var sut = CreateSut();
            IDictionary<string, object?>? dictionary = null;
            FormattableStringParserMock.Parse(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<TemplateFrameworkStringContext>())
                .Returns(x =>
                {
                    dictionary = x.ArgAt<TemplateFrameworkStringContext>(2).ParametersDictionary;

                    return Result<string>.Success(string.Empty);
                });

            // Act
            sut.SetParameter("Name", "Value");

            // Assert
            sut.Render(new StringBuilder());
            dictionary.Should().NotBeNull();
            dictionary!.First().Key.Should().Be("Name");
            dictionary!.First().Value.Should().Be("Value");
        }
    }
}
