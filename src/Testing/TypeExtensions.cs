using CrossCutting.Common.Testing;

namespace TemplateFramework.Testing;

[ExcludeFromCodeCoverage]
public static class TypeExtensions
{
    /// <summary>
    /// Asserts that the specified type performs argument null checks on all arguments in all (public) constructors, using NSubstitute to create mocks for reference types.
    /// </summary>
    /// <remarks>Note that this method throws an exeption when there is no public constructor.</remarks>
    /// <param name="type">The type to assert null argument checks for.</param>
    /// <param name="classFactory">Factory delegate to create instances from types (likely mocks)</param>
    /// <param name="parameterPredicate">Optional predicate to apply to each parameter info. When the predicate returns false, then the parameter will be skipped.</param>
    /// <param name="parameterReplaceDelegate">Optional function to apply to a parameter info. When the predicate is not defined, then we will create a mock or value type.</param>
    /// <param name="constructorPredicate">Optional constructor predicate. If not providerd, all public constructors will be used</param>
    public static void ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(
        this Type type,
        Func<ParameterInfo, bool>? parameterPredicate = null,
        Func<ParameterInfo, object?>? parameterReplaceDelegate = null,
        Func<ConstructorInfo, bool>? constructorPredicate = null)
        => type.ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(
            t => t.CreateInstance(t2 => Substitute.For([t2], Array.Empty<object>()), parameterReplaceDelegate, constructorPredicate),
            parameterPredicate,
            parameterReplaceDelegate,
            constructorPredicate);
}
