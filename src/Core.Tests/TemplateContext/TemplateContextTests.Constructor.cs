namespace TemplateFramework.Core.Tests;

public partial class TemplateContextTests
{
    public class Constructor : TemplateContextTests
    {
        [Fact]
        public void Throws_On_Null_Engine()
        {
            this.Invoking(_ => new TemplateContext(engine: null!, ProviderMock.Object, DefaultFilename, new TemplateInstanceIdentifier(this), this))
                .Should().Throw<ArgumentNullException>().WithParameterName("engine");
        }

        [Fact]
        public void Throws_On_Null_Provider()
        {
            this.Invoking(_ => new TemplateContext(EngineMock.Object, provider: null!, DefaultFilename, new TemplateInstanceIdentifier(this), this))
                .Should().Throw<ArgumentNullException>().WithParameterName("provider");
        }

        [Fact]
        public void Throws_On_Null_DefaultFilename()
        {
            this.Invoking(_ => new TemplateContext(EngineMock.Object, ProviderMock.Object, defaultFilename: null!, new TemplateInstanceIdentifier(this), this))
                .Should().Throw<ArgumentNullException>().WithParameterName("defaultFilename");
        }

        [Fact]
        public void Throws_On_Null_Identifier()
        {
            this.Invoking(_ => new TemplateContext(EngineMock.Object, ProviderMock.Object, DefaultFilename, identifier: null!, template: this))
                .Should().Throw<ArgumentNullException>().WithParameterName("identifier");
        }

        [Fact]
        public void Throws_On_Null_Template()
        {
            this.Invoking(_ => new TemplateContext(EngineMock.Object, ProviderMock.Object, DefaultFilename, new TemplateInstanceIdentifier(this), template: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("template");
        }

        [Fact]
        public void Creates_Instance_On_Non_Null_Template()
        {
            // Act
            var instance = new TemplateContext(EngineMock.Object, ProviderMock.Object, DefaultFilename, new TemplateInstanceIdentifier(this), this);

            // Assert
            instance.Template.Should().BeSameAs(this);
        }

        [Fact]
        public void Creates_Instance_With_Model_When_Supplied()
        {
            // Act
            var instance = new TemplateContext(EngineMock.Object, ProviderMock.Object, DefaultFilename, new TemplateInstanceIdentifier(this), this, model: "test");

            // Assert
            instance.Model.Should().BeEquivalentTo("test");
        }

        [Fact]
        public void Creates_Instance_With_ParentContext_When_Supplied()
        {
            // Act
            var instance = new TemplateContext(EngineMock.Object, ProviderMock.Object, DefaultFilename, new TemplateInstanceIdentifier(this), this, model: "current", parentContext: new TemplateContext(EngineMock.Object, ProviderMock.Object, DefaultFilename, new TemplateInstanceIdentifier(this), this, model: "parent"));

            // Assert
            instance.Model.Should().BeEquivalentTo("current");
            instance.ParentContext.Should().NotBeNull();
            instance.ParentContext!.Model.Should().BeEquivalentTo("parent");
        }

        [Fact]
        public void Creates_Instance_With_IterationNumber_And_IterationCount_When_Supplied()
        {
            // Act
            var instance = new TemplateContext(EngineMock.Object, ProviderMock.Object, DefaultFilename, new TemplateInstanceIdentifier(this), this, model: "test", parentContext: new TemplateContext(EngineMock.Object, ProviderMock.Object, DefaultFilename, new TemplateInstanceIdentifier(this), this, model: "parent"), iterationNumber: 1, iterationCount: 2);

            // Assert
            instance.IterationNumber.Should().Be(1);
            instance.IterationCount.Should().Be(2);
        }
    }
}
