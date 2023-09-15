namespace TemplateFramework.Runtime.Tests;

public class TestBase
{
    protected IFixture Fixture { get; } = new Fixture().Customize(new AutoNSubstituteCustomization());
}
