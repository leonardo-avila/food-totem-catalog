using FluentValidation;
using FoodTotem.Catalog.Domain.Models;
using FoodTotem.Catalog.Domain.Models.Validators;
using FoodTotem.Catalog.Domain.Ports;
using FoodTotem.Catalog.Domain.Services;
using FoodTotem.Domain.Core;

namespace FoodTotem.Catalog.Domain.Tests;

[TestClass]
public class FoodServiceTests
{
    private IFoodService _foodService;

    private readonly IValidator<Food> _foodValidator = new FoodValidator();

    [TestInitialize]
    public void TestInitialize()
    {
        _foodService = new FoodService(_foodValidator);
    }

    [TestMethod, TestCategory("Catalog - Services - Food")]
    public void ValidateFoodCategory_WithValidFoodCategory_ShouldSucceed()
    {
        // Arrange
        var foodCategory = "Meal";

        // Act
        var result = _foodService.IsValidCategory(foodCategory);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod, TestCategory("Catalog - Services - Food")]
    public void ValidateFoodCategory_WithInvalidFoodCategory_ShouldFail()
    {
        // Arrange
        var foodCategory = "Invalid";

        // Act
        var result = _foodService.IsValidCategory(foodCategory);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod, TestCategory("Catalog - Services - Food")]
    public void ValidateFood_WithValidFood_ShouldSucceed()
    {
        // Arrange
        var food = new Food("Test", "Teste", "Img", 1.99, Models.Enums.FoodCategory.Meal);

        // Act
        _foodService.ValidateFood(food);

        // Assert
        Assert.IsTrue(true);
    }

    [TestMethod, TestCategory("Catalog - Services - Food")]
    public void ValidateFood_WithInvalidFood_ShouldFail()
    {
        // Arrange
        var food = new Food("Teste", "Teste", "Img", 0, Models.Enums.FoodCategory.Meal);

        // Act
        Assert.ThrowsException<DomainException>(() => _foodService.ValidateFood(food), "Food should have a positive price.");

        // Assert
        Assert.IsTrue(true);
    }
}