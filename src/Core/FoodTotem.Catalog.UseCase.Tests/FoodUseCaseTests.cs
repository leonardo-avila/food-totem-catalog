using FoodTotem.Catalog.Domain.Models;
using FoodTotem.Catalog.Domain.Models.Enums;
using FoodTotem.Catalog.Domain.Ports;
using FoodTotem.Catalog.Domain.Repositories;
using FoodTotem.Catalog.UseCase.InputViewModels;
using FoodTotem.Catalog.UseCase.Ports;
using FoodTotem.Catalog.UseCase.UseCases;
using FoodTotem.Domain.Core;

namespace FoodTotem.Catalog.UseCase.Tests;

[TestClass]
public class FoodUseCaseTests
{
    private IFoodUseCases _foodUseCases;

    private readonly IEnumerable<Food> _foods = MockFoods();

    private readonly IFoodRepository _foodRepository = Substitute.For<IFoodRepository>();
    private readonly IFoodService _foodService = Substitute.For<IFoodService>();

    [TestInitialize]
    public void Initialize()
    {
        _foodUseCases = new FoodUseCases(_foodRepository, _foodService);
    }

    [TestMethod, TestCategory("Catalog - Use Cases - Food")]
    public async Task GetFoods_WithFoods_ShouldSucceed()
    {
        // Arrange
        _foodRepository.GetAll().Returns(_foods);

        // Act
        var foods = await _foodUseCases.GetFoods();

        // Assert
        Assert.IsNotNull(foods);
        Assert.AreEqual(5, foods.Count());
    }

    [TestMethod, TestCategory("Catalog - Use Cases - Food")]
    public async Task AddFood_WithValidFood_ShouldSucceed()
    {
        // Arrange
        var food = _foods.First();
        var foodInputViewModel = new FoodInputViewModel
        {
            Name = food.Name,
            Description = food.Description,
            ImageUrl = food.ImageUrl,
            Price = food.Price,
            Category = food.Category.ToString()
        };
        MockValidateFoodCategory();
        MockCreateFoodSuccess();

        // Act
        var foodOutputViewModel = await _foodUseCases.AddFood(foodInputViewModel);

        // Assert
        Assert.IsNotNull(foodOutputViewModel);
        Assert.AreEqual(food.Name, foodOutputViewModel.Name);
        Assert.AreEqual(food.Description, foodOutputViewModel.Description);
        Assert.AreEqual(food.ImageUrl, foodOutputViewModel.ImageUrl);
        Assert.AreEqual(food.Price, foodOutputViewModel.Price);
        Assert.AreEqual(food.Category.ToString(), foodOutputViewModel.Category);
    }

    [TestMethod, TestCategory("Catalog - Use Cases - Food")]
    public async Task AddFood_WithInvalidFoodCategory_ShouldFail()
    {
        // Arrange
        var food = _foods.First();
        var foodInputViewModel = new FoodInputViewModel
        {
            Name = food.Name,
            Description = food.Description,
            ImageUrl = food.ImageUrl,
            Price = food.Price,
            Category = nameof(food.Category)
        };
        MockInvalidateFoodCategory();

        // Act and Assert
        await Assert.ThrowsExceptionAsync<DomainException>(() => _foodUseCases.AddFood(foodInputViewModel), "Invalid food category.");
    }

    [TestMethod, TestCategory("Catalog - Use Cases - Food")]
    public async Task GetFood_WithValidId_ShouldSucceed()
    {
        // Arrange
        var food = _foods.First();
        MockGetFood();

        // Act
        var foodOutputViewModel = await _foodUseCases.GetFood(food.Id);

        // Assert
        Assert.IsNotNull(foodOutputViewModel);
        Assert.AreEqual(food.Name, foodOutputViewModel.Name);
        Assert.AreEqual(food.Description, foodOutputViewModel.Description);
        Assert.AreEqual(food.ImageUrl, foodOutputViewModel.ImageUrl);
        Assert.AreEqual(food.Price, foodOutputViewModel.Price);
        Assert.AreEqual(food.Category.ToString(), foodOutputViewModel.Category);
    }

    [TestMethod, TestCategory("Catalog - Use Cases - Food")]
    public async Task GetFood_WithInvalidId_ShouldFail()
    {
        // Arrange
        var food = _foods.Last();
        MockGetFood();

        // Act and Assert
        await Assert.ThrowsExceptionAsync<DomainException>(() => _foodUseCases.GetFood(food.Id), "There is no food with this id.");
    }

    [TestMethod, TestCategory("Catalog - Use Cases - Food")]
    public async Task GetFoodsByCategory_WithValidCategory_ShouldSucceed()
    {
        // Arrange
        var food = _foods.First();
        MockValidateFoodCategory();
        MockGetFoodsByCategory(food.Category);

        // Act
        var foods = await _foodUseCases.GetFoodsByCategory(food.Category.ToString());

        // Assert
        Assert.IsNotNull(foods);
        Assert.AreEqual(4, foods.Count());
    }

    [TestMethod, TestCategory("Catalog - Use Cases - Food")]
    public async Task GetFoodsByCategory_WithInvalidCategory_ShouldFail()
    {
        // Arrange
        var food = _foods.First();
        MockGetFoodsByCategory(food.Category);
        MockInvalidateFoodCategory();

        // Act and Assert
        await Assert.ThrowsExceptionAsync<DomainException>(() => _foodUseCases.GetFoodsByCategory("Invalid category"), "Invalid food category.");
    }

    [TestMethod, TestCategory("Catalog - Use Cases - Food")]
    public async Task UpdateFood_WithValidIdAndFood_ShouldSucceed()
    {
        // Arrange
        var food = _foods.First();
        var foodInputViewModel = new FoodInputViewModel
        {
            Name = food.Name,
            Description = food.Description,
            ImageUrl = food.ImageUrl,
            Price = food.Price,
            Category = food.Category.ToString()
        };
        MockGetFood();
        MockValidateFoodCategory();
        MockUpdateFoodSuccess();

        // Act
        var foodOutputViewModel = await _foodUseCases.UpdateFood(food.Id, foodInputViewModel);

        // Assert
        Assert.IsNotNull(foodOutputViewModel);
        Assert.AreEqual(food.Name, foodOutputViewModel.Name);
        Assert.AreEqual(food.Description, foodOutputViewModel.Description);
        Assert.AreEqual(food.ImageUrl, foodOutputViewModel.ImageUrl);
        Assert.AreEqual(food.Price, foodOutputViewModel.Price);
        Assert.AreEqual(food.Category.ToString(), foodOutputViewModel.Category);
    }

    [TestMethod, TestCategory("Catalog - Use Cases - Food")]
    public async Task UpdateFood_WithInvalidId_ShouldFail()
    {
        // Arrange
        var food = _foods.Last();
        var foodInputViewModel = new FoodInputViewModel
        {
            Name = food.Name,
            Description = food.Description,
            ImageUrl = food.ImageUrl,
            Price = food.Price,
            Category = food.Category.ToString()
        };
        MockGetFood();
        MockValidateFoodCategory();

        // Act and Assert
        await Assert.ThrowsExceptionAsync<DomainException>(() => _foodUseCases.UpdateFood(food.Id, foodInputViewModel), "There is no food with this id.");
    }

    [TestMethod, TestCategory("Catalog - Use Cases - Food")]
    public async Task UpdateFood_WithInvalidFoodCategory_ShouldFail()
    {
        // Arrange
        var food = _foods.First();
        var foodInputViewModel = new FoodInputViewModel
        {
            Name = food.Name,
            Description = food.Description,
            ImageUrl = food.ImageUrl,
            Price = food.Price,
            Category = food.Category.ToString()
        };
        MockGetFood();
        MockInvalidateFoodCategory();

        // Act and Assert
        await Assert.ThrowsExceptionAsync<DomainException>(() => _foodUseCases.UpdateFood(food.Id, foodInputViewModel), "Invalid food category.");
    }

    [TestMethod, TestCategory("Catalog - Use Cases - Food")]
    public async Task UpdateFood_WithValidIdAndInvalidFood_ShouldFail()
    {
        // Arrange
        var food = _foods.First();
        var foodInputViewModel = new FoodInputViewModel
        {
            Name = food.Name,
            Description = food.Description,
            ImageUrl = food.ImageUrl,
            Price = 0,
            Category = food.Category.ToString()
        };
        MockGetFood();
        MockValidateFoodCategory();
        MockInvalidateFood();

        // Act and Assert
        await Assert.ThrowsExceptionAsync<DomainException>(() => _foodUseCases.UpdateFood(food.Id, foodInputViewModel), "Food should have a positive price.");
    }

    [TestMethod, TestCategory("Catalog - Use Cases - Food")]
    public async Task DeleteFood_WithValidId_ShouldSucceed()
    {
        // Arrange
        var food = _foods.First();
        MockGetFood();
        MockDeleteFoodSuccess();

        // Act
        var result = await _foodUseCases.DeleteFood(food.Id);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod, TestCategory("Catalog - Use Cases - Food")]
    public async Task DeleteFood_WithInvalidId_ShouldFail()
    {
        // Arrange
        var food = _foods.Last();
        MockGetFood();

        // Act and Assert
        await Assert.ThrowsExceptionAsync<DomainException>(() => _foodUseCases.DeleteFood(food.Id), "No food found with this id.");
    }

    private static IEnumerable<Food> MockFoods() {
        return new List<Food> {
            new("Pizza", "Pizza de calabresa", "https://www.google.com", 10, FoodCategory.Meal),
            new("Hamburguer", "Hamburguer de calabresa", "https://www.google.com", 10, FoodCategory.Meal),
            new("Hot Dog", "Hot Dog de calabresa", "https://www.google.com", 10, FoodCategory.Meal),
            new("Pastel", "Pastel de calabresa", "https://www.google.com", 10, FoodCategory.Meal),
            new("Cerveja", "Cerveja de calabresa", "https://www.google.com", 10, FoodCategory.Drink),
        };
    }

    private void MockGetFood() {
        _foodRepository.Get(_foods.First().Id).Returns(_foods.First());
    }

    private void MockGetFoodsByCategory(FoodCategory category) {
        _foodRepository.GetFoodsByCategory(category).ReturnsForAnyArgs(_foods.Where(f => f.Category.Equals(category)));
    }

    private void MockInvalidateFoodCategory() {
        _foodService.IsValidCategory(Arg.Any<string>()).ReturnsForAnyArgs(false);
    }

    private void MockValidateFoodCategory() {
        _foodService.IsValidCategory(Arg.Any<string>()).ReturnsForAnyArgs(true);
    }

    private void MockCreateFoodSuccess() {
        _foodRepository.Create(Arg.Any<Food>()).ReturnsForAnyArgs(true);
    }

    private void MockInvalidateFood() {
        _foodService.When(x => x.ValidateFood(Arg.Any<Food>())).Do(x => throw new DomainException("Food should have a positive price."));
    }

    private void MockUpdateFoodSuccess() {
        _foodRepository.Update(Arg.Any<Food>()).ReturnsForAnyArgs(true);
    }

    private void MockDeleteFoodSuccess() {
        _foodRepository.Delete(Arg.Any<Food>()).ReturnsForAnyArgs(true);
    }
}