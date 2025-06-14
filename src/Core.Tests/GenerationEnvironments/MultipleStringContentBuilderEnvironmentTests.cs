﻿namespace TemplateFramework.Core.Tests.GenerationEnvironments;

public class MultipleStringContentBuilderEnvironmentTests
{
    protected IFileSystem FileSystemMock { get; } = Substitute.For<IFileSystem>();
    protected ICodeGenerationProvider CodeGenerationProviderMock { get; } = Substitute.For<ICodeGenerationProvider>();
    protected IMultipleContentBuilder<StringBuilder> MultipleContentBuilderMock { get; } = Substitute.For<IMultipleContentBuilder<StringBuilder>>();
    protected IMultipleContent MultipleContentMock { get; } = Substitute.For<IMultipleContent>();
    protected IContent ContentMock { get; } = Substitute.For<IContent>();
    protected IRetryMechanism RetryMechanism { get; } = new FastRetryMechanism();

    protected MultipleContentBuilderEnvironment<StringBuilder> CreateSut() => new(FileSystemMock, RetryMechanism, MultipleContentBuilderMock);

    protected static IEnumerable<IContent> CreateContents(bool skipWhenFileExists = false)
    {
        var builder = new MultipleContentBuilder();
        var c1 = builder.AddContent("File1.txt", skipWhenFileExists: skipWhenFileExists);
        c1.Builder.AppendLine("Test1");
        var c2 = builder.AddContent("File2.txt", skipWhenFileExists: skipWhenFileExists);
        c2.Builder.AppendLine("Test2");
        return builder.Build().Contents;
    }

    private sealed class FastRetryMechanism : RetryMechanism
    {
        protected override int WaitTimeInMs => 1;
    }

    public class Constructor : MultipleStringContentBuilderEnvironmentTests
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(MultipleContentBuilderEnvironment<StringBuilder>).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }

        [Fact]
        public void Creates_Instance_Correctly_With_Builder_Argument()
        {
            // Act
            var instance = CreateSut();

            // Assert
            instance.Builder.ShouldBeSameAs(MultipleContentBuilderMock);
        }

        [Fact]
        public void Creates_Instance_Correctly_Without_Arguments()
        {
            // Act
            var instance = new MultipleStringContentBuilderEnvironment();

            // Assert
            instance.Builder.ShouldNotBeNull();
        }
    }

    public class DeleteLastGeneratedFiles : MultipleStringContentBuilderEnvironmentTests
    {
        [Fact]
        public void Throws_On_Null_LastGeneratedFilesPath()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Action a = () => sut.DeleteLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, lastGeneratedFilesPath: null!, false);
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("lastGeneratedFilesPath");
        }

        [Fact]
        public void Throws_On_Empty_LastGeneratedFilesPath()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Action a = () => sut.DeleteLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, lastGeneratedFilesPath: string.Empty, false);
            a.ShouldThrow<ArgumentException>().ParamName.ShouldBe("lastGeneratedFilesPath");
        }

        [Fact]
        public void Throws_On_WhiteSpace_LastGeneratedFilesPath()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Action a = () => sut.DeleteLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, lastGeneratedFilesPath: " ", false);
            a.ShouldThrow<ArgumentException>().ParamName.ShouldBe("lastGeneratedFilesPath");
        }

        [Fact]
        public void Throws_On_Null_Encoding()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Action a = () => sut.DeleteLastGeneratedFiles(FileSystemMock, TestData.BasePath, encoding: null!, "LastGeneratedFiles.txt", true);
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("encoding");
        }

        [Fact]
        public void Appends_Directory_To_BasePath_When_LastGeneratedFilesPath_Contains_PathSeparator()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.DeleteLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, Path.Combine("MyDirectory", "LastGeneratedFiles.txt"), false);

            // Assert
            FileSystemMock.Received().FileExists(Path.Combine(TestData.BasePath, "MyDirectory", "LastGeneratedFiles.txt"));
        }

        [Fact]
        public void Does_Not_Append_Anything_To_BasePath_When_LastGeneratedFilesPath_Does_Not_Contain_PathSeparator_And_BasePath_Is_Not_Empty()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.DeleteLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, "LastGeneratedFiles.txt", false);

            // Assert
            FileSystemMock.Received().FileExists(Path.Combine(TestData.BasePath, "LastGeneratedFiles.txt"));
        }

        [Fact]
        public void Does_Not_Append_Anything_To_BasePath_When_LastGeneratedFilesPath_Does_Not_Contain_PathSeparator_And_BasePath_Is_Empty()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.DeleteLastGeneratedFiles(FileSystemMock, string.Empty, Encoding.Latin1, "LastGeneratedFiles.txt", false);

            // Assert
            FileSystemMock.Received().FileExists("LastGeneratedFiles.txt");
        }

        [Fact]
        public void Deletes_No_Files_From_Pattern_When_LastGeneratedFilesPath_Contains_Asterisk_But_Directory_Does_Not_Exist()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.DirectoryExists(TestData.BasePath).Returns(false);

            // Act
            sut.DeleteLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, "*.generated.cs", false);

            // Assert
            FileSystemMock.DidNotReceive().FileExists(Arg.Any<string>());
            FileSystemMock.DidNotReceive().FileDelete(Arg.Any<string>());
        }

        [Fact]
        public void Deletes_No_Files_From_Pattern_When_LastGeneratedFilesPath_Contains_Asterisk_But_BasePath_Is_Empty()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.DirectoryExists(TestData.BasePath).Returns(true);

            // Act
            sut.DeleteLastGeneratedFiles(FileSystemMock, string.Empty, Encoding.Latin1, "*.generated.cs", false);

            // Assert
            FileSystemMock.DidNotReceive().FileExists(Arg.Any<string>());
            FileSystemMock.DidNotReceive().FileDelete(Arg.Any<string>());
        }

        [Fact]
        public void Deletes_All_Files_From_Pattern_When_LastGeneratedFilesPath_Contains_Asterisk_Without_Recursion()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.DirectoryExists(TestData.BasePath).Returns(true);
            FileSystemMock.GetFiles(TestData.BasePath, "*.generated.cs", false).Returns([Path.Combine(TestData.BasePath, "File1.txt")]);

            // Act
            sut.DeleteLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, "*.generated.cs", false);

            // Assert
            FileSystemMock.DidNotReceive().FileExists(Arg.Any<string>()); // not called because the files are read from Directory.GetFiles
            FileSystemMock.Received().FileDelete(Arg.Any<string>()); // called once because Directory.GetFiles returns one file
        }

        [Fact]
        public void Deletes_All_Files_From_Pattern_When_LastGeneratedFilesPath_Contains_Asterisk_With_Recursion()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.DirectoryExists(TestData.BasePath).Returns(true);
            FileSystemMock.GetFiles(TestData.BasePath, "*.generated.cs", true).Returns(
            [
                Path.Combine(TestData.BasePath, "File1.txt"),
                Path.Combine(TestData.BasePath, "Subdirectory", "File2.txt")
            ]);

            // Act
            sut.DeleteLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, "*.generated.cs", true);

            // Assert
            FileSystemMock.DidNotReceive().FileExists(Arg.Any<string>()); // not called because the files are read from Directory.GetFiles
            FileSystemMock.Received(2).FileDelete(Arg.Any<string>()); // called once because Directory.GetFiles returns two files
        }

        [Fact]
        public void Deletes_No_Files_When_LastGeneratedFilesPath_Contains_No_Asterisk_But_File_Does_Not_Exist()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.FileExists("LastGeneratedFiles.txt").Returns(false);

            // Act
            sut.DeleteLastGeneratedFiles(FileSystemMock, string.Empty, Encoding.Latin1, "LastGeneratedFiles.txt", false);

            // Assert
            FileSystemMock.Received().FileExists(Arg.Any<string>()); // once because last generated files is checked
            FileSystemMock.DidNotReceive().FileDelete(Arg.Any<string>());
        }

        [Fact]
        public void Deletes_Files_When_LastGeneratedFilesPath_Contains_No_Asterisk_And_File_Exists_Empty_BasePath()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.FileExists(Arg.Any<string>())
                          .Returns(x => x.ArgAt<string>(0) == "LastGeneratedFiles.txt" || x.ArgAt<string>(0) == "File1.txt");
            FileSystemMock.ReadAllLines("LastGeneratedFiles.txt", Arg.Any<Encoding>()).Returns(
            [
                "File1.txt",
                "File2.txt"
            ]);

            // Act
            sut.DeleteLastGeneratedFiles(FileSystemMock, string.Empty, Encoding.Latin1, "LastGeneratedFiles.txt", false);

            // Assert
            FileSystemMock.Received(3).FileExists(Arg.Any<string>()); // once because last generated files is checked, two times because of File1.txt and File2.txt
            FileSystemMock.Received(1).FileDelete(Arg.Any<string>()); // File2.txt does not exist, so only File1.txt is deleted
        }

        [Fact]
        public void Deletes_Files_When_LastGeneratedFilesPath_Contains_No_Asterisk_And_File_Exists_Non_Empty_BasePath()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.FileExists(Arg.Any<string>()).Returns(x => x.ArgAt<string>(0) == Path.Combine(TestData.BasePath, "LastGeneratedFiles.txt") || x.ArgAt<string>(0) == Path.Combine(TestData.BasePath, "File1.txt"));
            FileSystemMock.ReadAllLines(Path.Combine(TestData.BasePath, "LastGeneratedFiles.txt"), Arg.Any<Encoding>()).Returns(
            [
                "File1.txt",
                "File2.txt"
            ]);

            // Act
            sut.DeleteLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, "LastGeneratedFiles.txt", false);

            // Assert
            FileSystemMock.Received(3).FileExists(Arg.Any<string>()); // once because last generated files is checked, two times because of File1.txt and File2.txt
            FileSystemMock.Received(1).FileDelete(Arg.Any<string>()); // File2.txt does not exist, so only File1.txt is deleted
        }
    }

    public class SaveAll : MultipleStringContentBuilderEnvironmentTests
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
            Action a = () => sut.SaveAll(FileSystemMock, TestData.BasePath, encoding: null!, CreateContents());
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("encoding");
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

    public class SaveContentsAsync : MultipleStringContentBuilderEnvironmentTests
    {
        public SaveContentsAsync()
        {
            CodeGenerationProviderMock.LastGeneratedFilesFilename.Returns("LastGeneratedFiles.txt");
            CodeGenerationProviderMock.Encoding.Returns(Encoding.Latin1);
            CodeGenerationProviderMock.Path.Returns("Subdirectory");
            ContentMock.Filename.Returns(Path.Combine("Subdirectory", "Filename.txt"));
            ContentMock.Contents.Returns("Content");
            MultipleContentMock.Contents.Returns(new[] { ContentMock }.ToList().AsReadOnly());
            MultipleContentBuilderMock.Build().Returns(MultipleContentMock);
        }

        [Fact]
        public async Task Deletes_LastGeneratedFiles_When_LastGeneratedFilesFilename_Is_Not_Empty()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.FileExists(Path.Combine(TestData.BasePath, "Subdirectory", "LastGeneratedFiles.txt")).Returns(true);

            // Act
            await sut.SaveContentsAsync(CodeGenerationProviderMock, TestData.BasePath, string.Empty, CancellationToken.None);

            // Assert
            FileSystemMock.Received().ReadAllLines(Path.Combine(TestData.BasePath, "Subdirectory", "LastGeneratedFiles.txt"), Encoding.Latin1);
        }

        [Fact]
        public async Task Does_Not_Delete_LastGeneratedFiles_When_LastGeneratedFilesFilename_Is_Null()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.LastGeneratedFilesFilename.Returns(default(string)!);

            // Act
            await sut.SaveContentsAsync(CodeGenerationProviderMock, TestData.BasePath, string.Empty, CancellationToken.None);

            // Assert
            FileSystemMock.DidNotReceive().ReadAllLines(Arg.Any<string>(), Arg.Any<Encoding>());
        }

        [Fact]
        public async Task Does_Not_Delete_LastGeneratedFiles_When_LastGeneratedFilesFilename_Is_Empty()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.LastGeneratedFilesFilename.Returns(string.Empty);

            // Act
            await sut.SaveContentsAsync(CodeGenerationProviderMock, TestData.BasePath, string.Empty, CancellationToken.None);

            // Assert
            FileSystemMock.DidNotReceive().ReadAllLines(Arg.Any<string>(), Arg.Any<Encoding>());
        }

        [Fact]
        public async Task Saves_All_Contents()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            await sut.SaveContentsAsync(CodeGenerationProviderMock, TestData.BasePath, string.Empty, CancellationToken.None);

            // Assert
            FileSystemMock.Received().WriteAllText(Path.Combine(TestData.BasePath, "Subdirectory", "Filename.txt"), "Content", Encoding.Latin1);
        }
    }

    public class SaveLastGeneratedFiles : MultipleStringContentBuilderEnvironmentTests
    {
        [Fact]
        public void Throws_On_Null_LastGeneratedFilesPath()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Action a = () => sut.SaveLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, lastGeneratedFilesPath: null!, CreateContents());
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("lastGeneratedFilesPath");
        }

        [Fact]
        public void Throws_On_Empty_LastGeneratedFilesPath()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Action a = () => sut.SaveLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, lastGeneratedFilesPath: string.Empty, CreateContents());
            a.ShouldThrow<ArgumentException>().ParamName.ShouldBe("lastGeneratedFilesPath");
        }

        [Fact]
        public void Throws_On_WhiteSpace_LastGeneratedFilesPath()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Action a = () => sut.SaveLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, lastGeneratedFilesPath: " ", CreateContents());
            a.ShouldThrow<ArgumentException>().ParamName.ShouldBe("lastGeneratedFilesPath");
        }

        [Fact]
        public void Throws_On_Null_Encoding()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Action a = () => sut.SaveLastGeneratedFiles(FileSystemMock, TestData.BasePath, encoding: null!, "LastGeneratedFiles.txt", CreateContents());
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("encoding");
        }

        [Fact]
        public void Throws_On_Null_Contents()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            Action a = () => sut.SaveLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, "LastGeneratedFiles.txt", contents: null!);
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("contents");
        }

        [Fact]
        public void Creates_Directory_When_It_Does_Not_Exist_Yet()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.DirectoryExists(TestData.BasePath).Returns(false);

            // Act
            sut.SaveLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, "LastGeneratedFiles.txt", CreateContents());

            // Assert
            FileSystemMock.Received().CreateDirectory(TestData.BasePath);
        }

        [Fact]
        public void Creates_File_When_Filename_Does_Not_Contain_Asterisk_Using_NonEmpty_BasePath()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.DirectoryExists(TestData.BasePath).Returns(true);

            // Act
            sut.SaveLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, "LastGeneratedFiles.txt", CreateContents());

            // Assert
            FileSystemMock.Received().WriteAllLines(Path.Combine(TestData.BasePath, "LastGeneratedFiles.txt"), Arg.Any<IEnumerable<string>>(), Encoding.Latin1);
        }

        [Fact]
        public void Creates_File_When_Filename_Does_Not_Contain_Asterisk_Using_Empty_BasePath()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.DirectoryExists("MyDirectory").Returns(true);

            // Act
            sut.SaveLastGeneratedFiles(FileSystemMock, string.Empty, Encoding.Latin1, "LastGeneratedFiles.txt", CreateContents());

            // Assert
            FileSystemMock.Received().WriteAllLines("LastGeneratedFiles.txt", Arg.Any<IEnumerable<string>>(), Encoding.Latin1);
        }

        [Fact]
        public void Does_Not_Create_File_When_Filename_Contains_Asterisk()
        {
            // Arrange
            var sut = CreateSut();
            FileSystemMock.DirectoryExists(TestData.BasePath).Returns(true);

            // Act
            sut.SaveLastGeneratedFiles(FileSystemMock, TestData.BasePath, Encoding.Latin1, "*.template.generated.cs", CreateContents());

            // Assert
            FileSystemMock.DidNotReceive().WriteAllLines(Arg.Any<string>(), Arg.Any<IEnumerable<string>>(), Encoding.Latin1);
        }
    }
}
