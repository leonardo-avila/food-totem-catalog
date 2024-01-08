using FoodTotem.Catalog.API.Controllers;
using FoodTotem.Catalog.UseCase.InputViewModels;
using FoodTotem.Catalog.UseCase.OutputViewModels;
using FoodTotem.Catalog.UseCase.Ports;
using FoodTotem.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodTotem.Catalog.API.Tests.BDD;

[Binding]
public class AddFoodsSteps {
    private readonly IFoodUseCases _foodUseCases = Substitute.For<IFoodUseCases>();
    private FoodInputViewModel _foodInputViewModel;

    private ActionResult<FoodOutputViewModel> _addFoodResult;
    private readonly FoodController _foodController;

    public AddFoodsSteps()
    {
        _foodController = new FoodController(_foodUseCases);
    }

    [Given(@"I have a food")]
    public void GivenIHaveAFood()
    {
        var food = new FoodInputViewModel
        {
            Name = "Test Food",
            Description = "Test Description",
            Category = "Meal",
            Price = 10.00
        };
        _foodUseCases.AddFood(food).Returns(new FoodOutputViewModel
        {
            Id = Guid.NewGuid(),
            Name = food.Name,
            Description = food.Description,
            Category = food.Category,
            Price = food.Price
        });
    }

    [Given(@"I have a invalid food")]
    public void GivenIHaveAInvalidFood()
    {
        _foodInputViewModel = new FoodInputViewModel
        {
            Name = "Test Food",
            Description = "Test Description",
            Category = "Meal",
            Price = 0
        };
        _foodUseCases.AddFood(_foodInputViewModel).Returns<FoodOutputViewModel>(x => throw new DomainException("Food should have a positive price."));
    }

    [Given(@"there is a internal error for add food")]
    public void GivenThereIsAInternalErrorForAddFood()
    {
        _foodInputViewModel = new FoodInputViewModel
        {
            Name = "Test Food",
            Description = "Test Description",
            Category = "Meal",
            Price = 10.00
        };
        _foodUseCases.AddFood(_foodInputViewModel).Returns<FoodOutputViewModel>(x => throw new Exception("An error occurred while adding food."));
    }

    [When(@"I add the food")]
    public async Task WhenIAddTheFood()
    {
        _addFoodResult = await _foodController.AddNewFood(_foodInputViewModel);
    }

    [Then(@"the food should be added to the catalog")]
    public void ThenTheFoodShouldBeAddedToTheCatalog()
    {
        Assert.IsInstanceOfType(_addFoodResult.Result, typeof(OkObjectResult));
    }

    [Then(@"I should receive a domain error for invalid food")]
    public void ThenIShouldReceiveADomainErrorForInvalidFood()
    {
        Assert.IsInstanceOfType(_addFoodResult.Result, typeof(BadRequestObjectResult), "Food should have a positive price.");
    }

    [Then(@"I should receive a internal error for add food")]
    public void ThenIShouldReceiveAInternalErrorForAddFood()
    {
        Assert.IsInstanceOfType(_addFoodResult.Result, typeof(ObjectResult));
        Assert.AreEqual(StatusCodes.Status500InternalServerError, (_addFoodResult.Result as ObjectResult)!.StatusCode);
    }
}