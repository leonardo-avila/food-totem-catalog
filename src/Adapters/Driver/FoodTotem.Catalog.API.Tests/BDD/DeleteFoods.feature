Feature: Delete foods
As a user
I want to delete foods
So that my catalog is up to date

Scenario: Delete a food
    Given I have a food
    When I delete the food
    Then the food is deleted

Scenario: Delete a food that does not exist
    Given I have a invalid food
    When I delete the invalid food
    Then I receive a domain error for delete invalid food

Scenario: Delete a food with internal error
    Given I have a food
    And there is a internal error for delete food
    When I delete the food
    Then I receive a internal error for delete food
