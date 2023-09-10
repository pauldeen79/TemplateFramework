namespace TemplateFramework.Core.Tests.GenerationEnvironments;

public partial class MultipleContentBuilderEnvironmentTests
{
    public class SaveAll : MultipleContentBuilderEnvironmentTests
    {
        [Fact]
        public void Uses_Content_Filename_When_BasePath_Is_Empty()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.SaveAll(FileSystemMock, string.Empty, Encoding.Latin1, CreateContents());

            // Assert
            FileSystemMock.Received().WriteAllText("File1.txt", "Test1" + Environment.NewLine, Encoding.Latin1);
            FileSystemMock.Received().WriteAllText("File2.txt", "Test2" + Environment.NewLine, Encoding.Latin1);
        }

        [Fact]
        public void Uses_Content_Filename_When_Content_Filename_Is_A_Full_Path()
        {
            // Arrange
            var sut = CreateSut();
            var builder = new MultipleContentBuilder();
            var c1 = builder.AddContent(Path.Combine(TestData.BasePath, "File1.txt"));
            c1.Builder.AppendLine("Test1");
            var contents = builder.Build().Contents;

            // Act
            sut.SaveAll(FileSystemMock, TestData.BasePath, Encoding.Latin1, contents);

            // Assert
            FileSystemMock.Received().WriteAllText(Path.Combine(TestData.BasePath, "File1.txt"), "Test1" + Environment.NewLine, Encoding.Latin1);
        }

        [Fact]
        public void Uses_Combined_Path_When_Content_Filename_Is_Not_A_Full_Path_And_BasePath_Is_Filled()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.SaveAll(FileSystemMock, TestData.BasePath, Encoding.Latin1, CreateContents());

            // Assert
            FileSystemMock.Received().WriteAllText(Path.Combine(TestData.BasePath, "File1.txt"), "Test1" + Environment.NewLine, Encoding.Latin1);
            FileSystemMock.Received().WriteAllText(Path.Combine(TestData.BasePath, "File2.txt"), "Test2" + Environment.NewLine, Encoding.Latin1);
        }

        [Fact]
        public void Throws_On_Null_Encoding()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.SaveAll(FileSystemMock, TestData.BasePath, encoding: null!, CreateContents()))
               .Should().Throw<ArgumentNullException>().WithParameterName("encoding");
        }

        [Fact]
        public void Skips_Writing_File_When_SkipWhenFileExists_Is_True_And_File_Already_Exists()
        {
            // Arrange
            var sut = CreateSut();
            var builder = new MultipleContentBuilder();
            var c1 = builder.AddContent(Path.Combine(TestData.BasePath, "File1.txt"), skipWhenFileExists: true);
            c1.Builder.AppendLine("Test1");
            var contents = builder.Build().Contents;

            FileSystemMock.FileExists(Arg.Any<string>()).Returns(true);

            // Act
            sut.SaveAll(FileSystemMock, string.Empty, Encoding.Latin1, contents);

            // Assert
            FileSystemMock.DidNotReceive().WriteAllText(Arg.Any<string>(), Arg.Any<string>(), Encoding.Latin1);
        }

        [Fact]
        public void Creates_Directory_When_It_Does_Not_Exist_Yet()
        {
            // Arrange
            var sut = CreateSut();
            int counter = 0;
            FileSystemMock.DirectoryExists(TestData.BasePath).Returns(_ =>
            {
                counter++;
                return counter == 1;
            });

            // Act
            sut.SaveAll(FileSystemMock, TestData.BasePath, Encoding.Latin1, CreateContents());

            // Assert
            FileSystemMock.Received().CreateDirectory(TestData.BasePath);
        }

        [Fact]
        public void Uses_Specified_Encoding()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.SaveAll(FileSystemMock, TestData.BasePath, Encoding.UTF32, CreateContents());

            // Assert
            FileSystemMock.Received().WriteAllText(Path.Combine(TestData.BasePath, "File1.txt"), "Test1" + Environment.NewLine, Encoding.UTF32);
            FileSystemMock.Received().WriteAllText(Path.Combine(TestData.BasePath, "File2.txt"), "Test2" + Environment.NewLine, Encoding.UTF32);
        }

        [Fact]
        public void Retries_When_IOException_Occurs()
        {
            // Arrange
            var sut = CreateSut();
            int attempt = 0;
            FileSystemMock.When(x => x.WriteAllText(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Encoding>()))
                          .Do(x =>
                          {
                              if (x.ArgAt<string>(0) == Path.Combine(TestData.BasePath, "File1.txt"))
                              {
                                  attempt++;
                                  if (attempt < 3)
                                  {
                                      throw new IOException("Can't write to file because it is being used by another process");
                                  }
                              }
                          });
            // Act
            sut.SaveAll(FileSystemMock, TestData.BasePath, Encoding.UTF32, CreateContents());

            // Assert
            FileSystemMock.Received(3).WriteAllText(Path.Combine(TestData.BasePath, "File1.txt"), "Test1" + Environment.NewLine, Encoding.UTF32);
            FileSystemMock.Received().WriteAllText(Path.Combine(TestData.BasePath, "File2.txt"), "Test2" + Environment.NewLine, Encoding.UTF32);
        }
    }
}
