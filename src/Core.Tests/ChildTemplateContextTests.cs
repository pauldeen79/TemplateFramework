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
            sut.Identifier.ShouldBeSameAs(templateIdentifier);
        }

        [Theory, AutoMockData]
        public void Initializes_Properties_Correctly_2([Frozen] ITemplateIdentifier templateIdentifier)
        {
            // Act
            var sut = new ChildTemplateContext(templateIdentifier, Model);

            // Assert
            sut.Identifier.ShouldBeSameAs(templateIdentifier);
            sut.Model.ShouldBeSameAs(Model);
        }

        [Theory, AutoMockData]
        public void Initializes_Properties_Correctly_3([Frozen] ITemplateIdentifier templateIdentifier)
        {
            // Act
            var sut = new ChildTemplateContext(templateIdentifier, Model, IterationNumber, IterationCount);

            // Assert
            sut.Identifier.ShouldBeSameAs(templateIdentifier);
            sut.Model.ShouldBeSameAs(Model);
            sut.IterationNumber.ShouldBe(IterationNumber);
            sut.IterationCount.ShouldBe(IterationCount);
        }
    }
}
