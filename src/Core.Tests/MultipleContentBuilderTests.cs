namespace TemplateFramework.Core.Tests;

public class MultipleContentBuilderTests
{
    protected MultipleContentBuilder CreateSut(bool skipWhenFileExists = false)
    {
        var sut = new MultipleContentBuilder();
        var c1 = sut.AddContent("File1.txt", skipWhenFileExists: skipWhenFileExists);
        c1.Builder.AppendLine("Test1");
        var c2 = sut.AddContent("File2.txt", skipWhenFileExists: skipWhenFileExists);
        c2.Builder.AppendLine("Test2");
        return sut;
    }

    public class AddContent : MultipleContentBuilderTests
    {
        [Fact]
        public void Throws_On_Null_Filename()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Action a = () => sut.AddContent(filename: null!);
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("filename");
        }

        [Fact]
        public void Uses_StringBuilder_When_Supplied()
        {
            // Arrange
            var sut = CreateSut();
            var builder = new StringBuilder("ExistingContent");

            // Act
            var result = sut.AddContent("File.txt", skipWhenFileExists: false, builder: builder);

            // Assert
            result.Builder.ToString().ShouldBe("ExistingContent");
        }

        [Fact]
        public void Creates_New_StringBulder_When_Not_Supplied()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.AddContent("File.txt");

            // Assert
            result.Builder.ToString().ShouldBeEmpty();
        }

        [Fact]
        public void Sets_Filename_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.AddContent("File.txt");

            // Assert
            result.Filename.ShouldBe("File.txt");
        }

        [Fact]
        public void Sets_SkipWhenFileExists_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.AddContent("File.txt", skipWhenFileExists: true);

            // Assert
            result.SkipWhenFileExists.ShouldBeTrue();
        }

        [Fact]
        public void Adds_New_File_To_Content_List()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            _ = sut.AddContent("File.txt");

            // Assert
            sut.Contents.Count().ShouldBe(3); //two are added from initialization in CreateSut, one is added using AddContent here;
            sut.Contents.ShouldContain(x => x.Filename == "File.txt");
        }
    }

    public class Build : MultipleContentBuilderTests
    {
        [Fact]
        public void Generates_MultipleContent_Instance()
        {
            // Arrange
            var sut = CreateSut(skipWhenFileExists: true);

            // Act
            var instance = sut.Build();

            // Assert
            instance.ShouldNotBeNull();
            instance.Contents.Count.ShouldBe(2);
            instance.Contents.Select(x => x.SkipWhenFileExists).ShouldAllBe(x => x == true);
        }

        [Fact]
        public void Throws_When_Filename_Is_Empty()
        {
            // Arrange
            var sut = CreateSut();
            var c1 = sut.AddContent(filename: string.Empty);
            c1.Builder.AppendLine("Test1");

            // Act & Assert
            Action a = () => sut.Build();
            a.ShouldThrow<ArgumentException>().ParamName.ShouldBe("filename");
        }
    }
}
