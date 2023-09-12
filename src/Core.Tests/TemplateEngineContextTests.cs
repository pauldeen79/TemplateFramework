namespace TemplateFramework.Core.Tests;

public class TemplateEngineContextTests
{
    protected IRenderTemplateRequest RequestMock { get; } = Substitute.For<IRenderTemplateRequest>();
    protected ITemplateEngine EngineMock { get; } = Substitute.For<ITemplateEngine>();
    protected ITemplateComponentRegistry ComponentRegistryMock { get; } = Substitute.For<ITemplateComponentRegistry>();
    protected object Template { get; } = new();

    public class Constructor : TemplateEngineContextTests
    {
        [Fact]
        public void Throws_On_Null_Arguments()
        {
            typeof(TemplateEngineContext).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments();
        }

        [Fact]
        public void Sets_Properties_Correctly()
        {
            // Arrange
            var additionalParameters = new object();
            var templateContextMock = Substitute.For<ITemplateContext>();
            var generationEnvironmentMock = Substitute.For<IGenerationEnvironment>();
            var identifierMock = Substitute.For<ITemplateIdentifier>();
            var model = new object();
            RequestMock.AdditionalParameters.Returns(additionalParameters);
            RequestMock.Context.Returns(templateContextMock);
            RequestMock.DefaultFilename.Returns("Filename.txt");
            RequestMock.GenerationEnvironment.Returns(generationEnvironmentMock);
            RequestMock.Identifier.Returns(identifierMock);
            RequestMock.Model.Returns(model);

            // Act
            var instance = new TemplateEngineContext(RequestMock, EngineMock, ComponentRegistryMock, Template);

            // Assert
            instance.AdditionalParameters.Should().BeEquivalentTo(RequestMock.AdditionalParameters);
            instance.ComponentRegistry.Should().BeSameAs(ComponentRegistryMock);
            instance.Context.Should().BeSameAs(RequestMock.Context);
            instance.DefaultFilename.Should().BeSameAs(RequestMock.DefaultFilename);
            instance.Engine.Should().BeSameAs(EngineMock);
            instance.GenerationEnvironment.Should().BeSameAs(RequestMock.GenerationEnvironment);
            instance.Identifier.Should().BeSameAs(RequestMock.Identifier);
            instance.Model.Should().BeSameAs(RequestMock.Model);
            instance.Template.Should().BeSameAs(Template);
        }
    }
}
