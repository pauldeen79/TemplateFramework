﻿namespace TemplateFramework.Core.Tests.MultipleContentBuilderTemplateCreators;

public partial class TypedMultipleCreatorTests
{
    public class TryCreate : TypedMultipleCreatorTests
    {
        [Fact]
        public void Returns_Instance_When_Instance_Is_Assignable_To_IMultipleContentBuilderTemplate()
        {
            // Arrange
            var templateMock = new Mock<IMultipleContentBuilderTemplate>();
            var sut = CreateSut();

            // Act
            var result = sut.TryCreate(templateMock.Object);

            // Assert
            result.Should().BeSameAs(templateMock.Object);
        }

        [Fact]
        public void Returns_Null_When_Instance_Is_Not_Assignable_To_IMultipleContentBuilderTemplate()
        {
            // Arrange
            var template = new object();
            var sut = CreateSut();

            // Act
            var result = sut.TryCreate(template);

            // Assert
            result.Should().BeNull();
        }
    }
}
