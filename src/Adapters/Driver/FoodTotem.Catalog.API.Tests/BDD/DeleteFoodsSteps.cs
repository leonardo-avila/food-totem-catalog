using FoodTotem.Catalog.API.Controllers;
using FoodTotem.Catalog.UseCase.Ports;
using FoodTotem.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodTotem.Catalog.API.Tests.BDD;

[Binding]
public class DeleteFoodsSteps {
    private readonly IFoodUseCases _foodUseCases = Substitute.For<IFoodUseCases>();

    private IActionResult _deleteFoodResult;

    private readonly FoodController _foodController;

    public DeleteFoodsSteps()
    {
        _foodController = new FoodController(_foodUseCases);
    }

    [Given(@"there is a internal error for delete food")]
    public void GivenThereIsAInternalErrorForDeleteFood() {
        _foodUseCases.DeleteFood(Arg.Any<Guid>()).Returns<bool>(x => throw new Exception("An error occurred while deleting food"));
    }

    [When(@"I delete the food")]
    public async Task WhenIDeleteTheFood() {
        _deleteFoodResult = await _foodController.DeleteFood(Guid.NewGuid());
    }

    [When(@"I delete the invalid food")]
    public async Task WhenIDeleteTheInvalidFood() {
        _foodUseCases.DeleteFood(Arg.Any<Guid>()).Returns<bool>(x => throw new DomainException("Invalid food."));
        _deleteFoodResult = await _foodController.DeleteFood(Guid.NewGuid());
    }

    [Then(@"the food is deleted")]
    public void ThenTheFoodIsDeleted() {
        Assert.IsInstanceOfType(_deleteFoodResult, typeof(OkObjectResult));
    }

    [Then(@"I receive a domain error for delete invalid food")]
    public void ThenIReceiveADomainErrorForDeleteInvalidFood() {
        Assert.IsInstanceOfType(_deleteFoodResult, typeof(BadRequestObjectResult));
    }

    [Then(@"I receive a internal error for delete food")]
    public void ThenIReceiveAInternalErrorForDeleteFood() {
        Assert.IsInstanceOfType(_deleteFoodResult, typeof(ObjectResult));
        Assert.AreEqual(StatusCodes.Status500InternalServerError, (_deleteFoodResult as ObjectResult)!.StatusCode);
    }
}