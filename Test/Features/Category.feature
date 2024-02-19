Feature: Category Management

Background: 
	Given the recipe API is located at https://localhost:7290/api/

  @Get
  Scenario: Getting all categories
    Given I am an logged in user with username: TiboAdmin and Password: Tibo123!
    When I request a list of categories
    Then I should receive a success response 
    And I should receive a list of all categories including Soepen, Vegetarisch, Voorgerecht, Hoofdgerecht, and Dessert

  @Get
  Scenario: Getting a category by id
    Given I am an logged in user with username: TiboAdmin and Password: Tibo123!
    When I request a category with id 1
    Then I should receive a success response 
    And I should receive the category details

  @Get
  Scenario: Attempting to get a category with a non-existent id
    Given I am an logged in user with username: TiboAdmin and Password: Tibo123!
    When I request a category with a non-existent id 99
    Then I should receive a not found response

  @Post
  Scenario: Creating a new category as an admin
    Given I am an logged in user with username: TiboAdmin and Password: Tibo123!
    When I create a new category with name: Fruits
    Then I should receive a created response
