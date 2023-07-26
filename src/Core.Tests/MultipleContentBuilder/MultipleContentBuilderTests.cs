namespace TemplateFramework.Core.Tests;

public partial class MultipleContentBuilderTests
{
    protected MultipleContentBuilder CreateSut(string basePath = "", bool skipWhenFileExists = false)
    {
        var sut = new MultipleContentBuilder(basePath);
        var c1 = sut.AddContent("File1.txt", skipWhenFileExists: skipWhenFileExists);
        c1.Builder.AppendLine("Test1");
        var c2 = sut.AddContent("File2.txt", skipWhenFileExists: skipWhenFileExists);
        c2.Builder.AppendLine("Test2");
        return sut;
    }
}
