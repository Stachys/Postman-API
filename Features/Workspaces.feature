Feature: Workspaces
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario: 01 List of workspace should be returned after GET request to postman workspaces api
	Given I have workspace with name vsTest
	When I send GET request to postman workspaces api
	Then I get list of workspaces with vsTest name in it

Scenario: 02 New workspace should be created after POST request to postman workspaces api
	When I send request to create vsTest workspace
	Then Created workspace appears in postman

Scenario: 03 Workspace should be updated after PUT request to postman wokrspaces api
	Given I have workspace with name vsTest
	When I send request to rename this workspace to Updated vsTest
	Then Name of workspace was updated to Updated vsTest

Scenario: 04 Workspace should be deleted after DELETE request to postman workspaces api
	Given I have workspace with name vsTest
	When I send request to delete this workspace
	Then Workspace was deleted

@Collection
Scenario: 05 Response with workspace content should be returned after GET request to specified workspace
	Given I have collection with name vsTest
	And I have workspace vsTest which contains this collection
	When I send GET request to postman workspaces api with id of this workspace 
	Then I get response with workspace content
