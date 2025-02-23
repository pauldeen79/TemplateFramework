namespace TemplateFramework.Core.Tests;

public class TemplateParameterTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(TemplateParameter).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }

        [Fact]
        public void Creates_New_Instance_On_NonNull_Values()
        {
            // Act
            var instance = new TemplateParameter("Name", GetType());

            // Assert
            instance.ShouldNotBeNull();
            instance.Name.ShouldBe("Name");
            instance.Type.ShouldBe(GetType());
        }
    }
}
