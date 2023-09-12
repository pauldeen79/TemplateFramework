namespace TemplateFramework.Core.Tests;

public partial class TemplateContextTests
{
    public class IsLastIteration : TemplateContextTests
    {
        [Theory,
            InlineData(null, null, null),
            InlineData(0, null, null),
            InlineData(null, 1, null),
            InlineData(0, 2, false),
            InlineData(1, 2, true)]
        public void Returns_Correct_Result(int? iterationNumber, int? iterationCount, bool? expectedResult)
        {
            // Arrange
            var sut = new TemplateContext(EngineMock, ProviderMock, DefaultFilename, new TemplateInstanceIdentifier(this), this, model: "test", parentContext: new TemplateContext(EngineMock, ProviderMock, DefaultFilename, new TemplateInstanceIdentifier(this), this, model: "parent"), iterationNumber: iterationNumber, iterationCount: iterationCount);

            // Act
            var result = sut.IsLastIteration;

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}
