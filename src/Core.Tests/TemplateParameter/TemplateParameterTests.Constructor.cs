namespace TemplateFramework.Core.Tests;

public class TemplateParameterTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Name()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateParameter(name: null!, GetType()))
                .Should().Throw<ArgumentNullException>().WithParameterName("name");
        }

        [Fact]
        public void Throws_On_Null_Type()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateParameter("Name", type: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("type");
        }

        [Fact]
        public void Creates_New_Instance_On_NonNull_Values()
        {
            // Act
            var instance = new TemplateParameter("Name", GetType());

            // Assert
            instance.Should().NotBeNull();
            instance.Name.Should().Be("Name");
            instance.Type.Should().Be(GetType());
        }
    }
}
