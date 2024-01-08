Feature: Add foods
As a user
I want to add food
So that the catalog could become bigger

Scenario: Add valid food
    Given I have a food
    When I add the food
    Then the food should be added to the catalog

Scenario: Add invalid food
    Given I have a invalid food
    When I add the food
    Then I should receive a domain error for invalid food

Scenario: Add food with internal error
    Given I have a food
    And there is a internal error for add food
    When I add the food
    Then I should receive a internal error for add food