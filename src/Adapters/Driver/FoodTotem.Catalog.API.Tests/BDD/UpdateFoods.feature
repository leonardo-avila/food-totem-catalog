Feature: Update food
As a user
I want to update food
So that my catalog is up to date

Scenario: Update valid food
    Given I have a food
    When I update the food
    Then the food is updated

Scenario: Update invalid food
    Given I have a invalid food
    When I update the food with invalid food
    Then I receive a domain error for invalid food

Scenario: Update food with internal error
    Given I have a food
    And there is a internal error for update food
    When I update the food
    Then I receive a internal error for update food
