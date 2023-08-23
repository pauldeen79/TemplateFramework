namespace TemplateFramework.TemplateProviders.CompiledTemplateProvider.Tests;

public class TemplateParameterWrapperTests
{
    public class Constructor
    {
        [Fact]
        public void Should_Throw_On_Null_Instance()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateParameterWrapper(instance: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("instance");
        }
    }

    public class Name
    {
        [Fact]
        public void Wraps_Name_Property_Correctly()
        {
            // Arrange
            var wrappedInstance = new TemplateParameter("Name", typeof(string));
            var sut = new TemplateParameterWrapper(wrappedInstance);

            // Act
            var result = sut.Name;

            // Assert
            result.Should().Be(wrappedInstance.Name);
        }

        [Fact]
        public void Returns_Empty_String_When_Name_Property_Returns_Null()
        {
            // Arrange
            var wrappedInstance = new BogusTemplateParameter { Name = null };
            var sut = new TemplateParameterWrapper(wrappedInstance);

            // Act
            var result = sut.Name;

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Throws_When_Wrapped_Instance_Does_Not_Have_Name_Property()
        {
            // Arrange
            var wrappedInstance = new object();
            var sut = new TemplateParameterWrapper(wrappedInstance);

            // Act
            sut.Invoking(x => _ = x.Name)
               .Should().Throw<NotSupportedException>();
        }
    }

    public class Type
    {
        [Fact]
        public void Wraps_Type_Property_Correctly()
        {
            // Arrange
            var wrappedInstance = new TemplateParameter("Name", typeof(string));
            var sut = new TemplateParameterWrapper(wrappedInstance);

            // Act
            var result = sut.Type;

            // Assert
            result.Should().Be(wrappedInstance.Type);
        }

        [Fact]
        public void Throws_When_Wrapped_Instance_Does_Not_Have_Type_Property()
        {
            // Arrange
            var wrappedInstance = new object();
            var sut = new TemplateParameterWrapper(wrappedInstance);

            // Act
            sut.Invoking(x => _ = x.Type)
               .Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void Throws_When_Wrapped_Instance_Returns_Null()
        {
            // Arrange
            var wrappedInstance = new BogusTemplateParameter { Type = null };
            var sut = new TemplateParameterWrapper(wrappedInstance);

            // Act
            sut.Invoking(x => _ = x.Type)
               .Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void Throws_When_Wrapped_Instance_Returns_Wrong_Type()
        {
            // Arrange
            var wrappedInstance = new BogusTemplateParameter { Type = 13 };
            var sut = new TemplateParameterWrapper(wrappedInstance);

            // Act
            sut.Invoking(x => _ = x.Type)
               .Should().Throw<NotSupportedException>();
        }
    }

    private sealed class BogusTemplateParameter
    {
        public string? Name { get; set; }
        public int? Type { get; set; }
    }
}
