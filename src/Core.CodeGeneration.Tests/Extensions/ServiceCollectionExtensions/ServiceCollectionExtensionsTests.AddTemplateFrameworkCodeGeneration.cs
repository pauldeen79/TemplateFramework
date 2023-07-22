﻿namespace TemplateFramework.Core.CodeGeneration.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    public class AddTemplateFrameworkCodeGeneration
    {
        [Fact]
        public void All_Dependencies_Can_Be_Resolved()
        {
            // Act
            using var provider = new ServiceCollection()
                .AddTemplateFramework()
                .AddTemplateFrameworkCodeGeneration()
                .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

            // Assert
            provider.Should().NotBeNull();
        }
    }
}
