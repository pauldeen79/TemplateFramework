namespace TemplateFramework.Core.Tests;

public partial class MultipleContentBuilderTests
{
    public class FromString : MultipleContentBuilderTests
    {
        [Fact]
        public void Throws_On_Null_Xml()
        {
            // Act & Assert
            this.Invoking(_ => MultipleContentBuilder.FromString(xml: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("xml");
        }

        [Fact]
        public void Throws_On_Empty_Xml()
        {
            // Act & Assert
            this.Invoking(_ => MultipleContentBuilder.FromString(xml: string.Empty))
                .Should().Throw<ArgumentException>().WithParameterName("xml");
        }

        [Fact]
        public void Throws_On_WhiteSpace_Xml()
        {
            // Act & Assert
            this.Invoking(_ => MultipleContentBuilder.FromString(xml: " "))
                .Should().Throw<ArgumentException>().WithParameterName("xml");
        }

        [Fact]
        public void Returns_New_Instance_When_Xml_Is_Filled()
        {
            // Arrange
            const string xml = @"<?xml version=""1.0"" encoding=""utf-16""?>
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
</MultipleContents>";

            // Act
            var instance = MultipleContentBuilder.FromString(xml);

            // Assert
            instance.Should().NotBeNull();
            instance.BasePath.Should().BeEmpty();
            instance.Contents.Should().HaveCount(2);
            instance.Contents.First().Filename.Should().Be("File1.txt");
            instance.Contents.Last().Filename.Should().Be("File2.txt");
        }
    }
}
