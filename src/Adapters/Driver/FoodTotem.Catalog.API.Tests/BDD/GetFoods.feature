Feature: Get foods
As a user
I want to get the catalog
So that I can choose my order

Scenario: Get foods by category
    Given there is foods
    When I get foods by category
    Then I should get foods by this category

Scenario: Get foods by invalid category
    Given there is foods
    When I get foods by invalid category
    Then I should get a domain error for category

Scenario: Get foods by category without foods
    Given there is no foods
    When I get foods by category
    Then I should receive a no content response

Scenario: Get foods with internal error
    Given there is foods
    And there is a internal error for get foods
    When I get foods by category
    Then I should get a internal error for get foods