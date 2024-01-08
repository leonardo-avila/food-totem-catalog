using FoodTotem.Catalog.API.Controllers;
using FoodTotem.Catalog.UseCase.OutputViewModels;
using FoodTotem.Catalog.UseCase.Ports;
using FoodTotem.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodTotem.Catalog.API.Tests.BDD;

[Binding]
public class GetFoodsSteps {
    private readonly IFoodUseCases _foodUseCases = Substitute.For<IFoodUseCases>();
    private ActionResult<IEnumerable<FoodOutputViewModel>> _getFoodsResult;

    private readonly FoodController _foodController;

    public GetFoodsSteps()
    {
        _foodController = new FoodController(_foodUseCases);
    }

    [Given(@"there is foods")]
    public void GivenThereIsFoods() {
        _foodUseCases.GetFoodsByCategory(Arg.Any<string>()).Returns(new List<FoodOutputViewModel>
        {
            new() {
                Id = Guid.NewGuid(),
                Name = "Test Food",
                Description = "Test Description",
                Category = "Meal",
                Price = 10.00
            }
        });
    }

    [Given(@"there is no foods")]
    public void GivenThereIsNoFoods() {
        _foodUseCases.GetFoodsByCategory(Arg.Any<string>()).Returns(new List<FoodOutputViewModel>());
    }

    [Given(@"there is a internal error for get foods")]
    public void GivenThereIsAInternalErrorForGetFoods() {
        _foodUseCases.GetFoodsByCategory(Arg.Any<string>()).Returns<IEnumerable<FoodOutputViewModel>>(x => throw new Exception("An error occurred while retrieving foods."));
    }

    [When(@"I get foods by category")]
    public async Task WhenIGetFoodsByCategory() {
        _getFoodsResult = await _foodController.GetFoodsByCategory("Meal");
    }

    [When(@"I get foods by invalid category")]
    public async Task WhenIGetFoodsByInvalidCategory() {
        _foodUseCases.GetFoodsByCategory(Arg.Any<string>()).Returns<IEnumerable<FoodOutputViewModel>>(x => throw new DomainException("Invalid category."));
        _getFoodsResult = await _foodController.GetFoodsByCategory("InvalidCategory");
    }

    [Then(@"I should get foods by this category")]
    public void ThenIShouldGetFoodsByThisCategory() {
        Assert.IsInstanceOfType(_getFoodsResult.Result, typeof(OkObjectResult));
    }

    [Then(@"I should get a domain error for category")]
    public void ThenIShouldGetADomainErrorForCategory() {
        Assert.IsInstanceOfType(_getFoodsResult.Result, typeof(BadRequestObjectResult), "Invalid category.");
    }

    [Then(@"I should receive a no content response")]
    public void ThenIShouldReceiveANoContentResponse() {
        Assert.IsInstanceOfType(_getFoodsResult.Result, typeof(NoContentResult));
    }

    [Then(@"I should get a internal error for get foods")]
    public void ThenIShoudGetAInternalErrorForGetFoods() {
        Assert.IsInstanceOfType(_getFoodsResult.Result, typeof(ObjectResult));
        Assert.AreEqual(StatusCodes.Status500InternalServerError, (_getFoodsResult.Result as ObjectResult)!.StatusCode);
    }
}