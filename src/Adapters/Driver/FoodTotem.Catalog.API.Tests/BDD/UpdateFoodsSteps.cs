using FoodTotem.Catalog.API.Controllers;
using FoodTotem.Catalog.UseCase.InputViewModels;
using FoodTotem.Catalog.UseCase.OutputViewModels;
using FoodTotem.Catalog.UseCase.Ports;
using FoodTotem.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodTotem.Catalog.API.Tests.BDD;

[Binding]
public class UpdateFoodsSteps {
    private readonly IFoodUseCases _foodUseCases = Substitute.For<IFoodUseCases>();

    private ActionResult<FoodOutputViewModel> _updateFoodResult;
    private readonly FoodController _foodController;

    public UpdateFoodsSteps()
    {
        _foodController = new FoodController(_foodUseCases);
    }

    [Given(@"there is a internal error for update food")]
    public void GivenThereIsAInternalErrorForUpdateFood() {
        _foodUseCases.UpdateFood(Arg.Any<Guid>(), Arg.Any<FoodInputViewModel>()).Returns<FoodOutputViewModel>(x => throw new Exception("Something wrong happened when updating food."));
    }

    [When(@"I update the food")]
    public async Task WhenIUpdateTheFood() {
        _updateFoodResult = await _foodController.UpdateFood(Guid.NewGuid(), MockFoodInputViewModel());
    }

    [When(@"I update the food with invalid food")]
    public async Task WhenIUpdateTheFoodWithInvalidFood() {
        _foodUseCases.UpdateFood(Arg.Any<Guid>(), Arg.Any<FoodInputViewModel>()).Returns<FoodOutputViewModel>(x => throw new DomainException("Food should have a positive price."));
        _updateFoodResult = await _foodController.UpdateFood(Guid.NewGuid(), MockFoodInputViewModel());
    }

    [Then(@"the food is updated")]
    public void ThenTheFoodIsUpdated() {
        Assert.IsInstanceOfType(_updateFoodResult.Result, typeof(OkObjectResult));
    }

    [Then(@"I receive a domain error for invalid food")]
    public void ThenIReceiveADomainErrorForInvalidFood() {
        Assert.IsInstanceOfType(_updateFoodResult.Result, typeof(BadRequestObjectResult));
    }

    [Then(@"I receive a internal error for update food")]
    public void ThenIReceiveAInternalErrorForUpdateFood() {
        Assert.IsInstanceOfType(_updateFoodResult.Result, typeof(ObjectResult));
        Assert.AreEqual(StatusCodes.Status500InternalServerError, (_updateFoodResult.Result as ObjectResult)!.StatusCode);
    }
    private static FoodInputViewModel MockFoodInputViewModel() {
        return new FoodInputViewModel
        {
            Name = "Test Food",
            Description = "Test Description",
            Category = "Meal",
            Price = 10.00
        };
    }
}