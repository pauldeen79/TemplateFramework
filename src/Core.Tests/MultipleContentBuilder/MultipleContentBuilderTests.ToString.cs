namespace TemplateFramework.Core.Tests;

public partial class MultipleContentBuilderTests
{
    public new class ToString : MultipleContentBuilderTests
    {
        [Fact]
        public void Returns_Correct_Xml_Formatted_Content()
        {
            // Arrange
            var sut = CreateSut(string.Empty);

            // Act
            var result = sut.ToString();

            // Assert
            result.Should().Be(@"<?xml version=""1.0"" encoding=""utf-16""?>
<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TemplateFramework"">
  <BasePath></BasePath>
  <Contents>
    <Contents>
      <Filename>File1.txt</Filename>
      <Lines xmlns:d4p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
        <d4p1:string>Test1</d4p1:string>
        <d4p1:string></d4p1:string>
      </Lines>
      <SkipWhenFileExists>false</SkipWhenFileExists>
    </Contents>
    <Contents>
      <Filename>File2.txt</Filename>
      <Lines xmlns:d4p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
        <d4p1:string>Test2</d4p1:string>
        <d4p1:string></d4p1:string>
      </Lines>
      <SkipWhenFileExists>false</SkipWhenFileExists>
    </Contents>
  </Contents>
</MultipleContents>");
        }
    }
}
