namespace TemplateFramework.Core.Tests;

public partial class TemplateContextTests
{
    public class CreateChildContext : TemplateContextTests
    {
        [Fact]
        public void Throws_On_Null_Template()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Invoking(x => x.CreateChildContext(template: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("template");
        }

        [Fact]
        public void Creates_ChildContext_With_Correct_ParentContext()
        {
            // Arrange
            var sut = CreateSut();
            var template = new object();

            // Act
            var childContext = sut.CreateChildContext(template: template);

            // Assert
            childContext.Should().NotBeNull();
            childContext.IsRootContext.Should().BeFalse();
            childContext.Template.Should().BeSameAs(template);
            childContext.ParentContext.Should().BeSameAs(sut);
        }
    }
}
