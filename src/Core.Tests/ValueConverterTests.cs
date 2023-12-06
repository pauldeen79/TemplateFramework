namespace TemplateFramework.Core.Tests;

public class ValueConverterTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(ValueConverter).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }

    public class Convert
    {
        [Theory, AutoMockData]
        public void Returns_Input_Value_When_No_TemplateParameterConverter_Supports_The_Type(
            [Frozen] ITemplateParameterConverter templateParameterConverter,
            [Frozen] ITemplateEngineContext context,
            ValueConverter sut)
        {
            // Arrange
            var value = "Hello world!";
            templateParameterConverter.TryConvert(Arg.Any<object?>(), Arg.Any<Type>(), Arg.Any<ITemplateEngineContext>(), out Arg.Any<object?>()).Returns(false);

            // Act
            var result = sut.Convert(value, value.GetType(), context);

            // Assert
            result.Should().BeSameAs(value);
        }

        [Theory, AutoMockData]
        public void Returns_Converted_Value_When_TemplateParameterConverter_Supports_The_Type(
            [Frozen] ITemplateParameterConverter templateParameterConverter,
            [Frozen] ITemplateEngineContext context,
            ValueConverter sut)
        {
            // Arrange
            var value = "Hello world!";
            templateParameterConverter.TryConvert(Arg.Any<object?>(), Arg.Any<Type>(), Arg.Any<ITemplateEngineContext>(), out Arg.Any<object?>()).Returns(x =>
            {
                x[3] = value.ToUpperInvariant();
                return true;
            });

            // Act
            var result = sut.Convert(value, value.GetType(), context);

            // Assert
            result.Should().BeEquivalentTo(value.ToUpperInvariant());
        }
    }
}
