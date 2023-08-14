﻿namespace TemplateFramework.Core.Tests;

public partial class TemplateInitializerTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Converters()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateInitializer(converters: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("converters");
        }
    }
}