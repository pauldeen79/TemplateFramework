﻿namespace TemplateFramework.Core.Tests;

public class IntegrationTests
{
    [Fact]
    public void Can_Render_Template()
    {
        // Arrange
        using var provider = new ServiceCollection()
            .AddTemplateFramework()
            .AddSingleton(new Mock<ITemplateComponentRegistryPluginFactory>().Object)
            .BuildServiceProvider();
        var sut = provider.GetRequiredService<ITemplateEngine>();

        var template = new TestData.Template(builder => builder.Append("Hello world!"));
        var builder = new MultipleContentBuilder();

        // Act
        sut.Render(new RenderTemplateRequest(new TemplateInstanceIdentifier(template), builder));

        // Assert
        builder.Contents.Should().ContainSingle();
        builder.Contents.Single().Builder.ToString().Should().Be("Hello world!");
    }
}
