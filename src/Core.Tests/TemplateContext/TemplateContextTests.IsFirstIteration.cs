namespace TemplateFramework.Core.Tests;

public partial class TemplateContextTests
{
    public class IsFirstIteration : TemplateContextTests
    {
        [Theory,
            InlineData(null, null, null),
            InlineData(0, null, null),
            InlineData(null, 1, null),
            InlineData(0, 2, true),
            InlineData(1, 2, false)]
        public void Returns_Correct_Result(int? iterationNumber, int? iterationCount, bool? expectedResult)
        {
            // Arrange
            var sut = new TemplateContext(EngineMock.Object, ProviderMock.Object, DefaultFilename, new TemplateInstanceIdentifier(this), this, model: "test", parentContext: new TemplateContext(EngineMock.Object, ProviderMock.Object, DefaultFilename, new TemplateInstanceIdentifier(this), this, model: "parent"), iterationNumber: iterationNumber, iterationCount: iterationCount);

            // Act
            var result = sut.IsFirstIteration;

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}
