﻿namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests.TemplateIdentifiers;

public class TemplateByNameIdentifierTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(TemplateByNameIdentifier).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }
    }
}
