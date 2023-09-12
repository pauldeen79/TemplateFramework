namespace TemplateFramework.Abstractions.Tests.Extensions.TemplateContextExtensions;

public class TemplateContextExtensionsTests
{
    public class GetContextByTemplateType : TemplateContextExtensionsTests
    {
        [Theory, AutoMockData]
        public void Without_Arguments_Works_Correctly([Frozen] ITemplateContext sut)
        {
            // Act
            sut.GetContextByTemplateType<string>();

            // Assert
            sut.Received().GetContextByTemplateType<string>(null);
        }
    }

    public class GetModelFromContextByType : TemplateContextExtensionsTests
    {
        [Theory, AutoMockData]
        public void Without_Arguments_Works_Correctly([Frozen] ITemplateContext sut)
        {
            // Act
            sut.GetModelFromContextByType<string>();

            // Assert
            sut.Received().GetModelFromContextByType<string>(null);
        }
    }
}
