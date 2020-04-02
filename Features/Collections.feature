Feature: Collections
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario: 01 List of collections should be returned after GET request to postman collections api
	Given I have collection with name Test
	When I send GET request to postman collections api
	Then I get list of collections with Test name in it

Scenario: 02 New collection should be created after POST request to postman collections api
	When I send request to create Test collection
	Then Collection Test appears in postman

Scenario: 03 Collection should be updated after PUT request to postman collections api
	Given I have collection with name Test
	When I send request to rename Test collection to Updated Test
	Then Name of collection was updated to Updated Test

Scenario: 04 Collection should be deleted after DELETE request to postman collections api
	Given I have collection with name Test
	When I send request to delete collection Test
	Then Collection Test was deleted
