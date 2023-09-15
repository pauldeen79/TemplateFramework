namespace TemplateFramework.TemplateProviders.StringTemplateProvider.Tests;

public class TestBase
{
    protected IFixture Fixture { get; } = new Fixture().Customize(new AutoNSubstituteCustomization());
}

public class TestBase<TSut> : TestBase
{
    protected TSut CreateSut() => Fixture.Create<TSut>();
}
