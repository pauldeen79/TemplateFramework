﻿namespace TemplateFramework.Core.Tests;

public class TemplateProviderTests
{
    protected TemplateProvider CreateSut() => new(new[] { TemplateProviderComponentMock.Object });

    protected Mock<ITemplateProviderComponent> TemplateProviderComponentMock { get; } = new();

    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Components()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateProvider(components: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("components");
        }
    }

    public class Create : TemplateProviderTests
    {
        [Fact]
        public void Throws_On_Null_Identifier()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(identifier: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("identifier");
        }

        [Fact]
        public void Throws_On_Unsupported_Type()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Create(identifier: new Mock<ITemplateIdentifier>().Object))
               .Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void Returns_Template_Instance_From_Component_On_Supported_Type()
        {
            // Arrange
            var sut = CreateSut();
            var identifier = new Mock<ITemplateIdentifier>().Object;
            var expectedTemplate = new object();
            TemplateProviderComponentMock.Setup(x => x.Supports(identifier)).Returns(true);
            TemplateProviderComponentMock.Setup(x => x.Create(identifier)).Returns(expectedTemplate);

            // Act
            var template = sut.Create(identifier);

            // Assert
            template.Should().BeSameAs(expectedTemplate);
        }
    }

    public class RegisterComponent : TemplateProviderTests
    {
        [Fact]
        public void Throws_On_Null_Component()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.RegisterComponent(component: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("component");
        }

        [Fact]
        public void Adds_Component_Registration()
        {
            // Arrange
            var sut = CreateSut();
            var customTemplateProviderComponentMock = new Mock<ITemplateProviderComponent>();
            var identifier = new Mock<ITemplateIdentifier>().Object;
            var expectedTemplate = new object();
            customTemplateProviderComponentMock.Setup(x => x.Supports(identifier)).Returns(true);
            customTemplateProviderComponentMock.Setup(x => x.Create(identifier)).Returns(expectedTemplate);

            // Act
            sut.RegisterComponent(customTemplateProviderComponentMock.Object);

            // Assert
            sut.Create(identifier);
            customTemplateProviderComponentMock.Verify(x => x.Create(identifier), Times.Once);
        }
    }
}
