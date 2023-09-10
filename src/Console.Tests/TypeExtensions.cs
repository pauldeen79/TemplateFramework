namespace TemplateFramework.Console.Tests;

public static class TestHelpers
{
    /// <summary>
    /// Asserts that the specified type performs argument null checks on all arguments in all (public) constructors.
    /// </summary>
    /// <remarks>Note that this method throws an exeption when there is no public constructor.</remarks>
    /// <param name="type">The type to assert null argument checks for.</param>
    /// <param name="parameterPredicate">Optional predicate to apply to each parameter info. When the predicate returns false, then the parameter will be skipped.</param>
    /// <param name="parameterReplaceDelegate">Optional function to apply to a parameter info. When the predicate is not defined, then we will create a mock or value type.</param>
    public static void ShouldArgumentNullExceptionsInConstructorOnNullArguments(this Type type,
                                                                 Func<ParameterInfo, bool>? parameterPredicate = null,
                                                                 Func<ParameterInfo, object>? parameterReplaceDelegate = null)
        => ConstructorMustThrowArgumentNullException(type, t => Substitute.For(new[] { t }, Array.Empty<object>()), parameterPredicate, parameterReplaceDelegate);

    /// <summary>
    /// Asserts that the specified type performs argument null checks on all arguments in all (public) constructors.
    /// </summary>
    /// <remarks>Note that this method throws an exeption when there is no public constructor.</remarks>
    /// <param name="type">The type to assert null argument checks for.</param>
    /// <param name="parameterPredicate">Optional predicate to apply to each parameter info. When the predicate returns false, then the parameter will be skipped.</param>
    /// <param name="parameterReplaceDelegate">Optional function to apply to a parameter info. When the predicate is not defined, then we will create a mock or value type.</param>
    public static void ConstructorMustThrowArgumentNullException(this Type type,
                                                                 Func<Type, object?> classFactory,
                                                                 Func<ParameterInfo, bool>? parameterPredicate = null,
                                                                 Func<ParameterInfo, object>? parameterReplaceDelegate = null)
    {
        var ctors = type.GetConstructors();
        ctors.Should().Match<ConstructorInfo[]?>(x => x != null && x.Length > 0, $"Type {type.FullName} should have public constructors");

        foreach (var constructor in ctors)
        {
            var parameters = constructor.GetParameters().ToArray();
            var mocks = GetMocks(parameters, parameterReplaceDelegate, classFactory);

            for (int i = 0; i < parameters.Length; i++)
            {
                if (ShouldSkipParameter(parameterPredicate, parameters, i))
                {
                    continue;
                }
                var mocksCopy = mocks.ToArray();
                mocksCopy[i] = FillParameter(parameters, i);

                FixStringsAndArrays(parameters, i, mocksCopy);

                try
                {
                    constructor.Invoke(mocksCopy);
                    parameters[i].ParameterType.IsValueType.Should().BeFalse(string.Format(CultureInfo.InvariantCulture, "ArgumentNullException expected for parameter {0} of constructor, but no exception was thrown", parameters[i].Name));
                }
                catch (TargetInvocationException ex)
                {
                    ex.InnerException
                        .Should().BeOfType<ArgumentNullException>()
                        .And.Match<ArgumentNullException>(x => x.ParamName == parameters[i].Name);
                }
            }
        }
    }

    private static object? FillParameter(ParameterInfo[] parameters, int i)
        => parameters[i].ParameterType.IsValueType
            ? Activator.CreateInstance(parameters[i].ParameterType)
            : null;

    private static bool ShouldSkipParameter(Func<ParameterInfo, bool>? parameterPredicate, ParameterInfo[] parameters, int i)
        => parameterPredicate is not null
            && !parameterPredicate.Invoke(parameters[i]);

    private static object?[] GetMocks(ParameterInfo[] parameters, Func<ParameterInfo, object>? parameterReplaceDelegate, Func<Type, object?> classFactory)
        => parameters.Select
        (
            p =>
            {
                if (parameterReplaceDelegate is not null)
                {
                    var returnValue = parameterReplaceDelegate.Invoke(p);
                    if (returnValue is not null)
                    {
                        return returnValue;
                    }
                }

                if (p.ParameterType == typeof(string))
                {
                    return string.Empty; // use string.Empty for string arguments, in case they require a null check
                }
                if (p.ParameterType.IsValueType || p.ParameterType.IsArray)
                {
                    return null; //skip value types and arrays
                }

                return classFactory.Invoke(p.ParameterType);
            }
        ).ToArray();

    private static void FixStringsAndArrays(ParameterInfo[] parameters, int i, object?[] mocksCopy)
    {
        for (int j = 0; j < parameters.Length; j++)
        {
            if (j == i)
            {
                continue;
            }
            if (parameters[j].ParameterType.IsArray)
            {
                mocksCopy[j] = Activator.CreateInstance(parameters[j].ParameterType, 0);
            }
            else if (parameters[j].ParameterType.FullName?.StartsWith("System.Collections.Generic.IEnumerable", StringComparison.InvariantCulture) == true)
            {
                // note that for now, we only allow generic Enumerables to work.
                // this needs to be extended to generic collections and lists of more types.
                mocksCopy[j] = Activator.CreateInstance(typeof(List<>).MakeGenericType(parameters[j].ParameterType.GetGenericArguments()[0]));
            }
            else if (parameters[j].ParameterType == typeof(string))
            {
                mocksCopy[j] = string.Empty;
            }
        }
    }
}
