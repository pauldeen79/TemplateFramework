namespace TemplateFramework.Core.Tests;

public class ValueConverterTests
{
    protected ValueConverter CreateSut() => new(new[] { TemplateParameterConverterMock.Object });

    protected Mock<ITemplateParameterConverter> TemplateParameterConverterMock { get; } = new();

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
            object? convertedValue = value.ToUpperInvariant();
            TemplateParameterConverterMock.Setup(x => x.TryConvert(It.IsAny<object?>(), It.IsAny<Type>(), out convertedValue)).Returns(true);

            // Act
            var result = sut.Convert(value, value.GetType());

            // Assert
            result.Should().BeEquivalentTo(value.ToUpperInvariant());
            TemplateParameterConverterMock.Verify(x => x.TryConvert(It.IsAny<object?>(), It.IsAny<Type>(), out convertedValue), Times.Once);
        }
    }
}
