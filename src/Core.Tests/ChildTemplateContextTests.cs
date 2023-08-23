namespace TemplateFramework.Core.Tests;

public class ChildTemplateContextTests
{
    protected const int IterationNumber = 13;
    protected const int IterationCount = 26;
    protected object Template { get; } = new();
    protected object Model { get; } = new();

    public class Constructor : ChildTemplateContextTests
    {
        [Fact]
        public void Throws_On_Null_Template_1()
        {
            this.Invoking(_ => new ChildTemplateContext(template: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("template");
        }

        [Fact]
        public void Throws_On_Null_Template_2()
        {
            this.Invoking(_ => new ChildTemplateContext(template: null!, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("template");
        }

        [Fact]
        public void Throws_On_Null_Template_3()
        {
            this.Invoking(_ => new ChildTemplateContext(template: null!, null, null, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("template");
        }

        [Fact]
        public void Initializes_Properties_Correctly_1()
        {
            // Act
            var sut = new ChildTemplateContext(Template);

            // Assert
            sut.Template.Should().BeSameAs(Template);
        }

        [Fact]
        public void Initializes_Properties_Correctly_2()
        {
            // Act
            var sut = new ChildTemplateContext(Template, Model);

            // Assert
            sut.Template.Should().BeSameAs(Template);
            sut.Model.Should().BeSameAs(Model);
        }

        [Fact]
        public void Initializes_Properties_Correctly_3()
        {
            // Act
            var sut = new ChildTemplateContext(Template, Model, IterationNumber, IterationCount);

            // Assert
            sut.Template.Should().BeSameAs(Template);
            sut.Model.Should().BeSameAs(Model);
            sut.IterationNumber.Should().Be(IterationNumber);
            sut.IterationCount.Should().Be(IterationCount);
        }
    }
}
