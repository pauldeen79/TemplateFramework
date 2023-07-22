namespace TemplateFramework.Core;

[ExcludeFromCodeCoverage] //HACK: for some reason, Sonar does not detect the code coverage, even though Visual Studio reports that it's fully covered. Let's just ignore this.
internal static class Constants
{
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
    internal const BindingFlags CustomBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
    internal const string ModelKey = "Model";
}
