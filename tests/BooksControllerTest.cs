
public class BooksControllerTest
{
    [Fact]
    public void indexUnitTest()
    {
        // Arrange
        var mockRepo = new Mock<IBookService>();
        mockRepo.Setup(n => n.GetAll()).Returns(MockData.GetTestBookItems() );
        var controller = new BooksController(mockRepo.Object);

        // Act
        var result = controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsAssignableFrom<List<Book>>(viewResult.ViewData.Model);
        var viewResultBooks = viewResult.ViewData.Model as List<Book>;
        Assert.Equal(5, viewResultBooks.Count);
    }

    [Theory]
    [InlineData("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200", "ab2bd817-98cd-4cf3-a80a-53ea0cd9cAAA")]
    public void DetailsUnitTest(string validGuid, string invalidGuid)
    {
        // Arrange
        var mockRepo = new Mock<IBookService>();
        var validItemGuid = new Guid(validGuid);
        mockRepo.Setup(n => n.GetById(validItemGuid)).Returns(MockData.GetTestBookItems().FirstOrDefault(x => x.Id == validItemGuid));
        var controller = new BooksController(mockRepo.Object);

        // Act
        var result = controller.Details(validItemGuid);
        // Assert            
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewResultValue = Assert.IsAssignableFrom<Book>(viewResult.ViewData.Model);
        Assert.Equal("Managing Oneself", viewResultValue.Title);
        Assert.Equal("Peter Drucker", viewResultValue.Author);
        Assert.Equal(validItemGuid, viewResultValue.Id);

        // Arrange
        var invalidItemGuid = new Guid(invalidGuid);
        mockRepo.Setup(n => n.GetById(invalidItemGuid)).Returns(MockData.GetTestBookItems().FirstOrDefault(x => x.Id == invalidItemGuid));

        // Act
        var notFoundResult = controller.Details(invalidItemGuid);
        // Assert
        Assert.IsType<NotFoundResult>(notFoundResult);
    }
    [Fact]
    public void CreateTest()
    {
        // Arrange
        var mockRepo = new Mock<IBookService>();
        var controller = new BooksController(mockRepo.Object);
        var newValidItem = new Book()
        {
            Author = "Author",
            Title = "Title",
            Description = "Description"
        };
        // Act
        var result = controller.Create(newValidItem);
        // Assert
        var rediretToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", rediretToActionResult.ActionName);
        Assert.Null(rediretToActionResult.ControllerName);

        // Arrange
        var newInvalidItem = new Book()
        {
            Title = "Title",
            Description = "Description"
        };
        controller.ModelState.AddModelError("Author", "Author field is required.");
        // Act
        var resultInvalid = controller.Create(newInvalidItem);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultInvalid);
        Assert.IsType<SerializableError>(badRequestResult.Value);
    }
    [Theory]
    [InlineData("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200")]
    public void DeleteTest(string validGuid) 
    {
        // Arrange
        var mockRepo = new Mock<IBookService>();
        mockRepo.Setup(n => n.GetAll()).Returns(MockData.GetTestBookItems());
        var controller = new BooksController(mockRepo.Object);
        var itemGuid = new Guid(validGuid);

        // Act
        var result = controller.Delete(itemGuid, null);
        // Assert
        var actionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", actionResult.ActionName);
        Assert.Null(actionResult.ControllerName);
    }
}
