namespace TemplateFramework.Abstractions.Tests.Extensions.TemplateContextExtensions;

public class TemplateContextExtensionsTests
{
    protected object Template { get; } = new();
    protected object Model { get; } = new();
    protected int? IterationNumber { get; } = 0;
    protected int? IterationCount { get; } = 2;

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
