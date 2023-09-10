namespace TemplateFramework.Core.Tests;

public class ValueConverterTests
{
    protected ValueConverter CreateSut() => new(new[] { TemplateParameterConverterMock });

    protected ITemplateParameterConverter TemplateParameterConverterMock { get; } = Substitute.For<ITemplateParameterConverter>();

    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Converters()
        {
            // Act & Assert
            this.Invoking(_ => new ValueConverter(converters: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("converters");
        }
    }

    public class Convert : ValueConverterTests
    {
        [Fact]
        public void Returns_Input_Value_When_No_TemplateParameterConverter_Supports_The_Type()
        {
            // Arrange
            var sut = CreateSut();
            var value = "Hello world!";

            // Act
            var result = sut.Convert(value, value.GetType());

            // Assert
            result.Should().BeSameAs(value);
        }

        [Fact]
        public void Returns_Converted_Value_When_TemplateParameterConverter_Supports_The_Type()
        {
            // Arrange
            var sut = CreateSut();
            var value = "Hello world!";
            TemplateParameterConverterMock.TryConvert(Arg.Any<object?>(), Arg.Any<Type>(), out Arg.Any<object?>()).Returns(x =>
            {
                x[2] = value.ToUpperInvariant();
                return true;
            });

            // Act
            var result = sut.Convert(value, value.GetType());

            // Assert
            result.Should().BeEquivalentTo(value.ToUpperInvariant());
            TemplateParameterConverterMock.Received().TryConvert(Arg.Any<object?>(), Arg.Any<Type>(), out Arg.Any<object?>());
        }
    }
}
