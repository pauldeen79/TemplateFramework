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
            result.ShouldBeEmpty();
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
            result.ToArray().ShouldBeEquivalentTo(input);
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
            result.ShouldNotBeNull();
            result.Length.ShouldBe(3);
            result![0].Key.ShouldBe("Item1");
            result[0].Value.ShouldBe(1);
            result[1].Key.ShouldBe("Item2");
            result[1].Value.ShouldBe("some value");
            result[2].Key.ShouldBe("Item3");
            result[2].Value.ShouldBeNull();
        }
    }
}
