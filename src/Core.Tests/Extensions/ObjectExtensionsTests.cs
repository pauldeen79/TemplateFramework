namespace TemplateFramework.Core.Tests.Extensions;

public class ObjectExtensionsTests
{
    public class ToKeyValuePairs
    {
        [Fact]
        public void Returns_Empty_Dictionary_On_Null_Instance()
        {
            // Arrange
            var input = (object?)null;

            // Act
            var result = input.ToKeyValuePairs();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Returns_Dictionary_With_Provided_KeyValuePairs()
        {
            // Arrange
            var input = new[]
            {
                new KeyValuePair<string, object?>("Item1", 1),
                new KeyValuePair<string, object?>("Item2", "some value"),
                new KeyValuePair<string, object?>("Item3", null),
            };

            // Act
            var result = input.ToKeyValuePairs();

            // Assert
            result.Should().BeEquivalentTo(input);
        }

        [Fact]
        public void Returns_Dictionary_With_Provided_Object_Instance()
        {
            // Arrange
            var input = new
            {
                Item1 = 1,
                Item2 = "some value",
                Item3 = (object?)null,
            };

            // Act
            var result = input.ToKeyValuePairs()?.ToArray();

            // Assert
            result.Should().HaveCount(3);
            result![0].Key.Should().Be("Item1");
            result[0].Value.Should().Be(1);
            result[1].Key.Should().Be("Item2");
            result[1].Value.Should().Be("some value");
            result[2].Key.Should().Be("Item3");
            result[2].Value.Should().BeNull();
        }
    }
}
