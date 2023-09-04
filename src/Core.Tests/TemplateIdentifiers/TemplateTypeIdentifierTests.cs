﻿namespace TemplateFramework.Core.Tests.TemplateIdentifiers;

public class TemplateTypeIdentifierTests
{
    protected Mock<ITemplateFactory> TemplateFactoryMock { get; } = new();

    public class Constructor : TemplateTypeIdentifierTests
    {
        [Fact]
        public void Throws_On_Null_Type()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateTypeIdentifier(type: null!, TemplateFactoryMock.Object))
                .Should().Throw<ArgumentNullException>().WithParameterName("type");
        }

        [Fact]
        public void Throws_On_Null_TemplateFactory()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateTypeIdentifier(GetType(), templateFactory: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("templateFactory");
        }

        [Fact]
        public void Sets_Type_Correctly()
        {
            // Act
            var identifier = new TemplateTypeIdentifier(GetType(), TemplateFactoryMock.Object);

            // Assert
            identifier.Type.Should().BeSameAs(GetType());
        }

        [Fact]
        public void Sets_TemplateFactory_Correctly()
        {
            // Act
            var identifier = new TemplateTypeIdentifier(GetType(), TemplateFactoryMock.Object);

            // Assert
            identifier.TemplateFactory.Should().BeSameAs(TemplateFactoryMock.Object);
        }
    }
}