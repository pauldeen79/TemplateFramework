namespace TemplateFramework.Core.Tests;

public partial class TemplateFileManagerTests
{
    public class Process : TemplateFileManagerTests
    {
        [Fact]
        public void Generates_Multiple_Content_When_Split_Is_Set_To_True()
        {
            // Arrange
            StringBuilder.Append("OriginalContent");
            var sut = CreateSutWithRealMultipleContentBuilder();
            sut.GenerationEnvironment.Append("Ignored");
            sut.StartNewFile("MyFile.txt").Append("Added");

            // Act
            sut.Process(split: true);

            // Assert
            sut.GenerationEnvironment.ToString().Should().Be(@"<?xml version=""1.0"" encoding=""utf-16""?>
<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TemplateFramework"">
  <BasePath></BasePath>
  <Contents>
    <Contents>
      <Filename>MyFile.txt</Filename>
      <Lines xmlns:d4p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
        <d4p1:string>Added</d4p1:string>
      </Lines>
      <SkipWhenFileExists>false</SkipWhenFileExists>
    </Contents>
  </Contents>
</MultipleContents>");
        }

        [Fact]
        public void Generates_Single_Content_When_Split_Is_Set_To_False_And_SilentOutput_Is_Set_To_False()
        {
            // Arrange
            StringBuilder.Append("OriginalContent");
            var sut = CreateSutWithRealMultipleContentBuilder();
            sut.GenerationEnvironment.Append("Ignored");
            sut.StartNewFile("MyFile.txt").Append("Added");

            // Act
            sut.Process(split: false, silentOutput: false);

            // Assert
            sut.GenerationEnvironment.ToString().Should().Be("OriginalContentAdded");
        }

        [Fact]
        public void Generates_Nothing_When_Split_Is_Set_To_False_And_SilentOutput_Is_Set_To_True()
        {
            // Arrange
            StringBuilder.Append("OriginalContent");
            var sut = CreateSutWithRealMultipleContentBuilder();
            sut.GenerationEnvironment.Append("Ignored");
            sut.StartNewFile("MyFile.txt").Append("Also ignored");

            // Act
            sut.Process(split: false, silentOutput: true);

            // Assert
            sut.GenerationEnvironment.ToString().Should().Be("OriginalContent");
        }
    }
}
