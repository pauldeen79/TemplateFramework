namespace TemplateFramework.Core.Tests;

public class ChildTemplateContextTests
{
    public class Constructor
    {
        private const int IterationNumber = 13;
        private const int IterationCount = 26;
        private object Model { get; } = new();

        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(ChildTemplateContext).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(p => !new[] { "model", "iterationNumber", "iterationCount" }.Contains(p.Name));
        }

        [Theory, AutoMockData]
        public void Initializes_Properties_Correctly_1([Frozen] ITemplateIdentifier templateIdentifier)
        {
            // Act
            var sut = new ChildTemplateContext(templateIdentifier);

            // Assert
            sut.Identifier.Should().BeSameAs(templateIdentifier);
        }

        [Theory, AutoMockData]
        public void Initializes_Properties_Correctly_2([Frozen] ITemplateIdentifier templateIdentifier)
        {
            // Act
            var sut = new ChildTemplateContext(templateIdentifier, Model);

            // Assert
            sut.Identifier.Should().BeSameAs(templateIdentifier);
            sut.Model.Should().BeSameAs(Model);
        }

        [Theory, AutoMockData]
        public void Initializes_Properties_Correctly_3([Frozen] ITemplateIdentifier templateIdentifier)
        {
            // Act
            var sut = new ChildTemplateContext(templateIdentifier, Model, IterationNumber, IterationCount);

            // Assert
            sut.Identifier.Should().BeSameAs(templateIdentifier);
            sut.Model.Should().BeSameAs(Model);
            sut.IterationNumber.Should().Be(IterationNumber);
            sut.IterationCount.Should().Be(IterationCount);
        }
    }
}
