namespace TemplateFramework.Core.Tests;

public class ChildTemplateContextTests
{
    protected const int IterationNumber = 13;
    protected const int IterationCount = 26;
    protected ITemplateIdentifier Identifier { get; } = Substitute.For<ITemplateIdentifier>();
    protected object Model { get; } = new();

    public class Constructor : ChildTemplateContextTests
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(ChildTemplateContext).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(p => !new[] { "model", "iterationNumber", "iterationCount" }.Contains(p.Name));
        }

        [Fact]
        public void Initializes_Properties_Correctly_1()
        {
            // Act
            var sut = new ChildTemplateContext(Identifier);

            // Assert
            sut.Identifier.Should().BeSameAs(Identifier);
        }

        [Fact]
        public void Initializes_Properties_Correctly_2()
        {
            // Act
            var sut = new ChildTemplateContext(Identifier, Model);

            // Assert
            sut.Identifier.Should().BeSameAs(Identifier);
            sut.Model.Should().BeSameAs(Model);
        }

        [Fact]
        public void Initializes_Properties_Correctly_3()
        {
            // Act
            var sut = new ChildTemplateContext(Identifier, Model, IterationNumber, IterationCount);

            // Assert
            sut.Identifier.Should().BeSameAs(Identifier);
            sut.Model.Should().BeSameAs(Model);
            sut.IterationNumber.Should().Be(IterationNumber);
            sut.IterationCount.Should().Be(IterationCount);
        }
    }
}
