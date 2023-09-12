namespace TemplateFramework.Abstractions.Tests.Extensions;

public class MultipleContentBuilderExtensionsTests
{
    public class AddContent
    {
        [Theory, AutoMockData]
        public void Without_Arguments_Works_Correctly([Frozen] IMultipleContentBuilder sut)
        {
            // Act
            sut.AddContent();

            // Assert
            sut.Received().AddContent(string.Empty, false, null);
        }

        [Theory, AutoMockData]
        public void With_Filename_Argument_Works_Correctly([Frozen] IMultipleContentBuilder sut)
        {
            // Act
            sut.AddContent("MyFilename.txt");

            // Assert
            sut.Received().AddContent("MyFilename.txt", false, null);
        }

        [Theory, AutoMockData]
        public void With_Filename_And_SkipWhenFileExists_Arguments_Works_Correctly([Frozen] IMultipleContentBuilder sut)
        {
            // Act
            sut.AddContent("MyFilename.txt", true);

            // Assert
            sut.Received().AddContent("MyFilename.txt", true, null);
        }
    }
}
