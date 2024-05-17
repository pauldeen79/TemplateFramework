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
        public void Throws_On_Null_Arguments()
        {
            typeof(FormattableStringTemplate).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(
                p => !new[] { "model", "iterationNumber", "iterationCount" }.Contains(p.Name));
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
                    return Result.Success<FormattableStringParserResult>(string.Empty);
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
            sut.Awaiting(x => x.Render(builder: null!, CancellationToken.None))
               .Should().ThrowAsync<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_NonSuccesful_Result_From_FormattableStringParser()
        {
            // Arrange
            FormattableStringParserMock
                .Parse(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<TemplateFrameworkStringContext>())
                .Returns(Result.Error<FormattableStringParserResult>("Kaboom!"));
            var sut = CreateSut();
            var builder = new StringBuilder();

            // Act & Assert
            sut.Awaiting(x => x.Render(builder, CancellationToken.None))
               .Should().ThrowAsync<InvalidOperationException>().WithMessage("Result: Error, ErrorMessage: Kaboom!");
        }

        [Fact]
        public async Task Appends_Result_From_FormattableStringParser_To_Builder_On_Succesful_Result()
        {
            // Arrange
            FormattableStringParserMock
                .Parse(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<TemplateFrameworkStringContext>())
                .Returns(Result.Success<FormattableStringParserResult>("Parse result"));
            var sut = CreateSut();
            var builder = new StringBuilder();

            // Act
            await sut.Render(builder, CancellationToken.None);

            // Assert
            builder.ToString().Should().Be("Parse result");
        }
    }

    public class SetParameter : FormattableStringTemplateTests
    {
        [Fact]
        public async Task Adds_Parameter_To_Context()
        {
            // Arrange
            var sut = CreateSut();
            IDictionary<string, object?>? dictionary = null;
            FormattableStringParserMock
                .Parse(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<TemplateFrameworkStringContext>())
                .Returns(x =>
                {
                    dictionary = x.ArgAt<TemplateFrameworkStringContext>(2).ParametersDictionary;

                    return Result.Success<FormattableStringParserResult>(string.Empty);
                });

            // Act
            sut.SetParameter("Name", "Value");

            // Assert
            await sut.Render(new StringBuilder(), CancellationToken.None);
            dictionary.Should().NotBeNull();
            dictionary!.First().Key.Should().Be("Name");
            dictionary!.First().Value.Should().Be("Value");
        }
    }
}
