namespace TemplateFramework.TemplateProviders.ChildTemplateProvider.Tests;

public class TemplateCreatorTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Factory()
        {
            // Act & Assert
            Action a = () => _ = new TemplateCreator<TemplateCreatorTests>(factory: null!, null, null);
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("factory");
        }

        [Fact]
        public void Throws_When_Name_And_ModelType_Are_Both_Null()
        {
            // Act & Assert
            Action a = () => _ = new TemplateCreator<TemplateCreatorTests>(() => new(), null, null);
            a.ShouldThrow<InvalidOperationException>().Message.ShouldBe("Either modelType or name is required");
        }

        [Fact]
        public void Throws_On_Null_Name()
        {
            // Act & Assert
            Action a = () => _ = new TemplateCreator<TemplateCreatorTests>(name: null!);
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("name");
        }

        [Fact]
        public void Throws_On_Null_ModelType()
        {
            // Act & Assert
            Action a = () => _ = new TemplateCreator<TemplateCreatorTests>(modelType: null!);
            a.ShouldThrow<ArgumentNullException>().ParamName.ShouldBe("modelType");
        }
    }

    public class CreateByModel
    {
        [Fact]
        public void Returns_Instance_When_Model_Is_Of_Correct_Type()
        {
            // Arrange
            var sut = new TemplateCreator<TemplateCreatorTests>(() => new TemplateCreatorTests(), typeof(string), null);

            // Act
            var result = sut.CreateByModel("string model");

            // Assert
            result.ShouldNotBeNull();
        }

        [Fact]
        public void Throws_When_Model_Is_Not_Of_Correct_Type()
        {
            // Arrange
            var sut = new TemplateCreator<TemplateCreatorTests>(() => new TemplateCreatorTests(), typeof(string), null);

            // Act & Assert
            Action a = () => sut.CreateByModel(1);
            a.ShouldThrow<NotSupportedException>();
        }
    }

    public class CreateByName
    {
        [Fact]
        public void Returns_Instance_When_Name_Is_Equal()
        {
            // Arrange
            var sut = new TemplateCreator<TemplateCreatorTests>(() => new TemplateCreatorTests(), null, "Correct");

            // Act
            var result = sut.CreateByName("Correct");

            // Assert
            result.ShouldNotBeNull();
        }

        [Fact]
        public void Throws_When_Name_Is_Not_Equal()
        {
            // Arrange
            var sut = new TemplateCreator<TemplateCreatorTests>(() => new TemplateCreatorTests(), null, "Correct");

            // Act & Assert
            Action a = () => sut.CreateByName("Incorrect");
            a.ShouldThrow<NotSupportedException>();
        }
    }

    public class SupportsModel
    {
        [Fact]
        public void Returns_True_When_Model_Is_Of_Correct_Type()
        {
            // Arrange
            var sut = new TemplateCreator<TemplateCreatorTests>(() => new TemplateCreatorTests(), typeof(string), null);

            // Act
            var result = sut.SupportsModel("string model");

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void Returns_False_When_Model_Is_Not_Of_Correct_Type()
        {
            // Arrange
            var sut = new TemplateCreator<TemplateCreatorTests>(() => new TemplateCreatorTests(), typeof(string), null);

            // Act
            var result = sut.SupportsModel(1);

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void Returns_False_When_ModelType_Is_Not_Set()
        {
            // Arrange
            var sut = new TemplateCreator<TemplateCreatorTests>(() => new TemplateCreatorTests(), modelType: null, "something to pass unit test, value gets ignored by SupportsModel");

            // Act
            var result = sut.SupportsModel("some model");

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void Returns_False_When_Model_Is_Null()
        {
            // Arrange
            var sut = new TemplateCreator<TemplateCreatorTests>(() => new TemplateCreatorTests(), typeof(string), null);

            // Act
            var result = sut.SupportsModel(model: null);

            // Assert
            result.ShouldBeFalse();
        }
    }

    public class SupportsName
    {
        [Fact]
        public void Returns_True_When_Name_Is_Equal()
        {
            // Arrange
            var sut = new TemplateCreator<TemplateCreatorTests>(() => new TemplateCreatorTests(), null, "Correct");

            // Act
            var result = sut.SupportsName("Correct");

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void Returns_False_When_Name_Is_Not_Equal()
        {
            // Arrange
            var sut = new TemplateCreator<TemplateCreatorTests>(() => new TemplateCreatorTests(), null, "Correct");

            // Act
            var result = sut.SupportsName("Incorrect");

            // Assert
            result.ShouldBeFalse();
        }
    }
}
