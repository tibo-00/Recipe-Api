Feature: Recipe


Background: 
	Given the recipe API is located at https://localhost:7290/api/

  @Get
  Scenario: Getting all recipes
    Given I am an logged in user with username: TiboAdmin and Password: Tibo123!
    When I request a list of recipes
    Then I should receive a success response 
    And I should receive a list of all recipes

  @Get
  Scenario: Getting a recipe by id
    Given I am an logged in user with username: TiboAdmin and Password: Tibo123!
    When I request a recipe with id 1
    Then I should receive a success response 
    And I should receive the recipe details with id 1

  @Get
  Scenario: Attempting to get a recipe with a non-existent id
    Given I am an logged in user with username: TiboAdmin and Password: Tibo123!
    When I request a recipe with a non-existent id 8
    Then I should receive a not found response

  @Post
  Scenario: Creating a new recipe as an admin
    Given I am an logged in user with username: TiboAdmin and Password: Tibo123!
    When I create a new recipe 
    Then I should receive a created response 
    And I should receive the recipe details

  @Delete
  Scenario: Deleting a recipe by id
    Given I am an logged in user with username: TiboAdmin and Password: Tibo123!
    And I create a new recipe
    When I delete that recipe with it's id
    Then I should receive a no content response
