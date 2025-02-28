namespace TemplateFramework.Core.Tests;

public partial class TemplateContextTests
{
    public class CreateChildContext : TemplateContextTests
    {
        [Fact]
        public void Throws_On_Null_ChildContext()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            Action a = () => sut.CreateChildContext(childContext: null!);
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("childContext");
        }

        [Fact]
        public void Creates_ChildContext_With_Correct_ParentContext()
        {
            // Arrange
            var sut = CreateSut();
            var template = new object();

            // Act
            var childContext = sut.CreateChildContext(new TemplateContext(EngineMock, ProviderMock, DefaultFilename, new TemplateInstanceIdentifier(template), template: template));

            // Assert
            childContext.ShouldNotBeNull();
            childContext.IsRootContext.ShouldBeFalse();
            childContext.Template.ShouldBeOfType<IgnoreThis>();
            childContext.ParentContext.ShouldBeSameAs(sut);
        }
    }
}
