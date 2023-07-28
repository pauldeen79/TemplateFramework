using FluentAssertions.Equivalency.Tracing;

namespace TemplateFramework.Core.CodeGeneration.Tests;

public partial class CodeGenerationEngineTests
{
    public class Generate : CodeGenerationEngineTests
    {
        [Fact]
        public void Throws_On_Null_CodeGenerationProvider()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Invoking(x => x.Generate(provider: null!, GenerationEnvironmentMock.Object, CodeGenerationSettingsMock.Object))
               .Should().Throw<ArgumentNullException>().WithParameterName("provider");
        }

        [Fact]
        public void Throws_On_Null_GenerationEnvironment()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Invoking(x => x.Generate(CodeGenerationProviderMock.Object, generationEnvironment: null!, CodeGenerationSettingsMock.Object))
               .Should().Throw<ArgumentNullException>().WithParameterName("generationEnvironment");
        }

        [Fact]
        public void Throws_On_Null_CodeGenerationSettings()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Invoking(x => x.Generate(CodeGenerationProviderMock.Object, GenerationEnvironmentMock.Object, settings: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("settings");
        }

        [Fact]
        public void Saves_Generated_Content_When_BasePath_Is_Filled_And_DryRun_Is_False()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.Encoding).Returns(Encoding.Latin1);
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.Setup(x => x.CreateRequest(It.IsAny<IGenerationEnvironment>())).Returns<IGenerationEnvironment>(env => new RenderTemplateRequest(this, env, "Filename.txt", null, null));
            CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(false);
            CodeGenerationSettingsMock.SetupGet(x => x.BasePath).Returns(TestData.BasePath);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, GenerationEnvironmentMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            GenerationEnvironmentMock.Verify(x => x.SaveContents(CodeGenerationProviderMock.Object, TestData.BasePath, "Filename.txt"), Times.Once);
        }

        [Fact]
        public void Does_Not_Save_Generatd_Content_When_BasePath_Is_Filled_But_DryRun_Is_True()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.Encoding).Returns(Encoding.Latin1);
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.Setup(x => x.CreateRequest(It.IsAny<IGenerationEnvironment>())).Returns<IGenerationEnvironment>(env => new RenderTemplateRequest(this, env, "Filename.txt", null, null));
            CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(true);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, GenerationEnvironmentMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            GenerationEnvironmentMock.Verify(x => x.SaveContents(CodeGenerationProviderMock.Object, TestData.BasePath, "Filename.txt"), Times.Never);
        }
    }
}
