﻿namespace TemplateFramework.Core.Abstractions;

public interface ITemplateInitializerComponent : ITemplateInitializer
{
    int Order { get; }
}
