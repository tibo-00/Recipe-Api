Feature: Authentication

	As a user of the recipe API,
    I want to be able to register and log in,
    So that I can access personalized features of the application.

Background: 
	Given for authentication the recipe API is located at https://localhost:7290/api/Authentication/

@Post
Scenario: Register As Admin
	Given a user with username: TiboAdministator2 and password: Tibo123!
	And wants to be admin
    When the user attempts to register
    Then the response status is 200
    And the user should receive a success message

@Post
Scenario: Register
	Given a new user with username: Tibomenardo2 and password: Menardo123!
    When the user attempts to register
    Then the response status is 200
    And the user should receive a success message

@Post
Scenario: Login
	Given a user with username: Tibo and password: Tibo123!
	And the user already exists
	When the user attempts to login
	Then the response status is 200
    And the user should receive a key

@Post
Scenario: Wrong Login
	Given a user with username: Tibo and password: Tibo12345
	When the user attempts to login
	Then the response status is 401
