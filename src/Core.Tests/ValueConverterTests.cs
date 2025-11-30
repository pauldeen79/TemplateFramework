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
            templateParameterConverter.Convert(Arg.Any<object?>(), Arg.Any<Type>(), Arg.Any<ITemplateEngineContext>()).Returns(Result.Continue<object?>());

            // Act
            var result = sut.Convert(value, value.GetType(), context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeSameAs(value);
        }

        [Theory, AutoMockData]
        public void Returns_Converted_Value_When_TemplateParameterConverter_Supports_The_Type(
            [Frozen] ITemplateParameterConverter templateParameterConverter,
            [Frozen] ITemplateEngineContext context,
            ValueConverter sut)
        {
            // Arrange
            var value = "Hello world!";
            templateParameterConverter
                .Convert(Arg.Any<object?>(), Arg.Any<Type>(), Arg.Any<ITemplateEngineContext>())
                .Returns(x => Result.Success<object?>(value.ToUpperInvariant()));

            // Act
            var result = sut.Convert(value, value.GetType(), context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(value.ToUpperInvariant());
        }
    }
}
