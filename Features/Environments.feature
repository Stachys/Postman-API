Feature: Environments
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario: 01 List of environments should be returned after GET request to postman environments api
	Given I have environment with name vsTest
	When I send GET request to postman environments api
	Then I get list of environments with vsTest name in it

Scenario: 02 New environment should be created after POST request to postman environments api
	When I send request to create vsTest environment
	Then Created environment appears in postman

Scenario: 03 Environment should be updated after PUT request to postman environments api
	Given I have environment with name vsTest
	When I send request to rename this environment to Updated vsTest
	Then Name of environment was updated to Updated vsTest

Scenario: 04 Environment should be deleted after DELETE request to postman environments api
	Given I have environment with name vsTest
	When I send request to delete this environment
	Then Environment was deleted

Scenario: 05 Response with environment content should be returned after GET request to specified environment
	Given I have environment vsTest with variables
	When I send GET request to postman environments api with uid of this environment 
	Then I get response with environment content
