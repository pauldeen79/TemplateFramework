﻿namespace TemplateFramework.Core.Abstractions;

public interface ITemplateParameterConverter
{
    bool TryConvert(object? value, Type type, ITemplateEngineContext context, out object? convertedValue);
}
