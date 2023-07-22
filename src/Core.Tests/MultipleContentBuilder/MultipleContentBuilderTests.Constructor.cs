namespace TemplateFramework.Core.Tests;

public partial class MultipleContentBuilderTests
{
    public class Constructor : MultipleContentBuilderTests
    {
        [Fact]
        public void Creates_Instance_With_Empty_BasePath()
        {
            // Act
            var sut = new MultipleContentBuilder();

            // Assert
            sut.BasePath.Should().BeEmpty();
        }

        [Fact]
        public void Creates_Instance_With_Filled_BasePath()
        {
            // Act
            var sut = new MultipleContentBuilder(TestData.BasePath);

            // Assert
            sut.BasePath.Should().Be(TestData.BasePath);
        }

        [Fact]
        public void Creates_Instance_With_Filled_Encoding()
        {
            // Act
            var sut = new MultipleContentBuilder(Encoding.Latin1);

            // Assert
            sut.Encoding.Should().BeEquivalentTo(Encoding.Latin1);
        }

        [Fact]
        public void Creates_Instance_With_Filled_BasePath_And_Encoding()
        {
            // Act
            var sut = new MultipleContentBuilder(Encoding.UTF32, TestData.BasePath);

            // Assert
            sut.Encoding.Should().Be(Encoding.UTF32);
            sut.BasePath.Should().Be(TestData.BasePath);
        }

        [Fact]
        public void Throws_On_Null_FileSystem()
        {
            // Act & Assert
            this.Invoking(_ => new MultipleContentBuilder(fileSystem: null!, Encoding.UTF8, TestData.BasePath))
                .Should().Throw<ArgumentNullException>().WithParameterName("fileSystem");
        }

        [Fact]
        public void Throws_On_Null_Encoding()
        {
            // Act & Assert
            this.Invoking(_ => new MultipleContentBuilder(FileSystemMock.Object, encoding: null!, TestData.BasePath))
                .Should().Throw<ArgumentNullException>().WithParameterName("encoding");
        }

        [Fact]
        public void Throws_On_Null_BasePath()
        {
            // Act & Assert
            this.Invoking(_ => new MultipleContentBuilder(FileSystemMock.Object, Encoding.UTF8, basePath: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("basePath");
        }
    }
}
