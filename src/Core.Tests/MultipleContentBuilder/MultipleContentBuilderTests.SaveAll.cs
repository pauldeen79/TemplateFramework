//namespace TemplateFramework.Core.Tests;

//public partial class MultipleContentBuilderTests
//{
//    public class SaveAll : MultipleContentBuilderTests
//    {
//        [Fact]
//        public void Uses_Content_Filename_When_BasePath_Is_Empty()
//        {
//            // Arrange
//            var sut = CreateSut(string.Empty);

//            // Act
//            sut.SaveAll(Encoding.Latin1);

//            // Assert
//            FileSystemMock.Verify(x => x.WriteAllText("File1.txt", "Test1" + Environment.NewLine, Encoding.Latin1), Times.Once);
//            FileSystemMock.Verify(x => x.WriteAllText("File2.txt", "Test2" + Environment.NewLine, Encoding.Latin1), Times.Once);
//        }

//        [Fact]
//        public void Uses_Content_Filename_When_Content_Filename_Is_A_Full_Path()
//        {
//            // Arrange
//            var sut = CreateSut();
//            var c1 = sut.AddContent(Path.Combine(TestData.BasePath, "File1.txt"));
//            c1.Builder.AppendLine("Test1");

//            // Act
//            sut.SaveAll(Encoding.Latin1);

//            // Assert
//            FileSystemMock.Verify(x => x.WriteAllText(Path.Combine(TestData.BasePath, "File1.txt"), "Test1" + Environment.NewLine, Encoding.Latin1), Times.Once);
//        }

//        [Fact]
//        public void Uses_Combined_Path_When_Content_Filename_Is_Not_A_Full_Path_And_BasePath_Is_Filled()
//        {
//            // Arrange
//            var sut = CreateSut(TestData.BasePath);

//            // Act
//            sut.SaveAll(Encoding.Latin1);

//            // Assert
//            FileSystemMock.Verify(x => x.WriteAllText(Path.Combine(TestData.BasePath, "File1.txt"), "Test1" + Environment.NewLine, Encoding.Latin1), Times.Once);
//            FileSystemMock.Verify(x => x.WriteAllText(Path.Combine(TestData.BasePath, "File2.txt"), "Test2" + Environment.NewLine, Encoding.Latin1), Times.Once);
//        }

//        [Fact]
//        public void Throws_When_Filename_Is_Empty()
//        {
//            // Arrange
//            var sut = CreateSut();
//            var c1 = sut.AddContent(filename: string.Empty);
//            c1.Builder.AppendLine("Test1");

//            // Act & Assert
//            sut.Invoking(x => x.SaveAll(Encoding.Latin1))
//               .Should().Throw<ArgumentException>().WithParameterName("filename");
//        }

//        [Fact]
//        public void Throws_On_Null_Encoding()
//        {
//            // Arrange
//            var sut = CreateSut();

//            // Act & Assert
//            sut.Invoking(x => x.SaveAll(encoding: null!))
//               .Should().Throw<ArgumentNullException>().WithParameterName("encoding");
//        }

//        [Fact]
//        public void Skips_Writing_File_When_SkipWhenFileExists_Is_True_And_File_Already_Exists()
//        {
//            // Arrange
//            var sut = CreateSut(string.Empty, skipWhenFileExists: true);
//            FileSystemMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

//            // Act
//            sut.SaveAll(Encoding.Latin1);

//            // Assert
//            FileSystemMock.Verify(x => x.WriteAllText(It.IsAny<string>(), It.IsAny<string>(), Encoding.Latin1), Times.Never);
//        }

//        [Fact]
//        public void Creates_Directory_When_It_Does_Not_Exist_Yet()
//        {
//            // Arrange
//            var sut = CreateSut(TestData.BasePath);
//            int counter = 0;
//            FileSystemMock.Setup(x => x.DirectoryExists(TestData.BasePath)).Returns(() =>
//            {
//                counter++;
//                return counter == 1;
//            });

//            // Act
//            sut.SaveAll(Encoding.Latin1);

//            // Assert
//            FileSystemMock.Verify(x => x.CreateDirectory(TestData.BasePath), Times.Once);
//        }

//        [Fact]
//        public void Uses_Specified_Encoding()
//        {
//            // Arrange
//            var sut = CreateSut(TestData.BasePath);

//            // Act
//            sut.SaveAll(Encoding.UTF32);

//            // Assert
//            FileSystemMock.Verify(x => x.WriteAllText(Path.Combine(TestData.BasePath, "File1.txt"), "Test1" + Environment.NewLine, Encoding.UTF32), Times.Once);
//            FileSystemMock.Verify(x => x.WriteAllText(Path.Combine(TestData.BasePath, "File2.txt"), "Test2" + Environment.NewLine, Encoding.UTF32), Times.Once);
//        }

//        [Fact]
//        public void Retries_When_IOException_Occurs()
//        {
//            // Arrange
//            var sut = CreateSut(TestData.BasePath);
//            int attempt = 0;
//            FileSystemMock.Setup(x => x.WriteAllText(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Encoding>()))
//                          .Callback<string, string, Encoding>((path, contents, encoding) =>
//                          {
//                              if (path == Path.Combine(TestData.BasePath, "File1.txt"))
//                              {
//                                  attempt++;
//                                  if (attempt < 3)
//                                  {
//                                      throw new IOException("Can't write to file because it is being used by another process");
//                                  }
//                              }
//                          });
//            // Act
//            sut.SaveAll(Encoding.UTF32);

//            // Assert
//            FileSystemMock.Verify(x => x.WriteAllText(Path.Combine(TestData.BasePath, "File1.txt"), "Test1" + Environment.NewLine, Encoding.UTF32), Times.Exactly(3));
//            FileSystemMock.Verify(x => x.WriteAllText(Path.Combine(TestData.BasePath, "File2.txt"), "Test2" + Environment.NewLine, Encoding.UTF32), Times.Once);
//        }
//    }
//}
