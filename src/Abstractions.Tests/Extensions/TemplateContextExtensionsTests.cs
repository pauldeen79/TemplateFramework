namespace TemplateFramework.Abstractions.Tests.Extensions.TemplateContextExtensions;

public class TemplateContextExtensionsTests : TestBase
{
    public class GetContextByTemplateType : TemplateContextExtensionsTests
    {
        [Fact]
        public void Without_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = Fixture.Freeze<ITemplateContext>();

            // Act
            sut.GetContextByTemplateType<string>();

            // Assert
            sut.Received().GetContextByTemplateType<string>(null);
        }
    }

    public class GetModelFromContextByType : TemplateContextExtensionsTests
    {
        [Fact]
        public void Without_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = Fixture.Freeze<ITemplateContext>();

            // Act
            sut.GetModelFromContextByType<string>();

            // Assert
            sut.Received().GetModelFromContextByType<string>(null);
        }
    }
}
