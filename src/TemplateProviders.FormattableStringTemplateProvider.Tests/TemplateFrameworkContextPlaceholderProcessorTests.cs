﻿namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests;

public class TemplateFrameworkContextPlaceholderProcessorTests
{
    protected ComponentRegistrationContext ComponentRegistrationContext { get; } = new();
    
    protected TemplateFrameworkContextPlaceholderProcessor CreateSut() => new();

    public class Process : TemplateFrameworkContextPlaceholderProcessorTests
    {
        [Fact]
        public void Throws_On_Null_Value()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Process(value: null!, CultureInfo.CurrentCulture, null))
               .Should().Throw<ArgumentNullException>().WithParameterName("value");
        }

        [Fact]
        public void Throws_On_Null_FormatProvider()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Process("some template", formatProvider: null!, null))
               .Should().Throw<ArgumentNullException>().WithParameterName("formatProvider");
        }

        [Fact]
        public void Returns_Continue_When_Context_Is_Not_TemplateFrameworkFormattableStringContext()
        {
            // Arrange
            var sut = CreateSut();
            var context = "some context that's not of type TemplateFrameworkFormattableStringContext";

            // Act
            var result = sut.Process("some template", CultureInfo.CurrentCulture, context);

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Success_With_Parameter_Value_When_Context_Is_TemplateFrameworkFormattableStringContext_And_Parameter_Is_Known_And_Not_Null()
        {
            // Arrange
            var sut = CreateSut();
            var parametersDictionary = new Dictionary<string, object?>
            {
                { "Name", "Value" }
            };
            var context = new TemplateFrameworkStringContext(parametersDictionary, ComponentRegistrationContext.Processors, false);

            // Act
            var result = sut.Process("Name", CultureInfo.CurrentCulture, context);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be("Value");
        }

        [Fact]
        public void Returns_Success_With_StringEmpty_When_Context_Is_TemplateFrameworkFormattableStringContext_And_Parameter_Is_Known_But_Null()
        {
            // Arrange
            var sut = CreateSut();
            var parametersDictionary = new Dictionary<string, object?>
            {
                { "Name", null }
            };
            var context = new TemplateFrameworkStringContext(parametersDictionary, ComponentRegistrationContext.Processors, false);

            // Act
            var result = sut.Process("Name", CultureInfo.CurrentCulture, context);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEmpty();
        }

        [Fact]
        public void Returns_Continue_When_Context_Is_TemplateFrameworkFormattableStringContext_And_Parameter_Is_Unknown()
        {
            // Arrange
            var sut = CreateSut();
            var parametersDictionary = new Dictionary<string, object?>();
            var context = new TemplateFrameworkStringContext(parametersDictionary, ComponentRegistrationContext.Processors, false);

            // Act
            var result = sut.Process("Name", CultureInfo.CurrentCulture, context);

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
        }

        [Fact]
        public void Adds_Parameter_Name_To_List()
        {
            // Arrange
            var sut = CreateSut();
            var parametersDictionary = new Dictionary<string, object?>();
            var context = new TemplateFrameworkStringContext(parametersDictionary, ComponentRegistrationContext.Processors, false);

            // Act
            _ = sut.Process("Name", CultureInfo.CurrentCulture, context);

            // Assert
            context.ParameterNamesList.Should().BeEquivalentTo("Name");
        }

        [Fact]
        public void Does_Not_Add_Parameter_Name_To_List_When_Name_Starts_With_Double_Underscore()
        {
            // Arrange
            var sut = CreateSut();
            var parametersDictionary = new Dictionary<string, object?>();
            var context = new TemplateFrameworkStringContext(parametersDictionary, ComponentRegistrationContext.Processors, false);

            // Act
            _ = sut.Process("__Name", CultureInfo.CurrentCulture, context);

            // Assert
            context.ParameterNamesList.Should().BeEmpty();
        }
    }
}
