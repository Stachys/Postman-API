Feature: Collections
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario: 01 List of collections should be returned after GET request to postman collections api
	Given I have collection with name vsTest
	When I send GET request to postman collections api
	Then I get list of collections with vsTest name in it

Scenario: 02 New collection should be created after POST request to postman collections api
	When I send request to create vsTest collection
	Then Created collection appears in postman

Scenario: 03 Collection should be updated after PUT request to postman collections api
	Given I have collection with name vsTest
	When I send request to rename this collection to Updated vsTest
	Then Name of collection was updated to Updated vsTest

Scenario: 04 Collection should be deleted after DELETE request to postman collections api
	Given I have collection with name vsTest
	When I send request to delete this collection
	Then Collection was deleted
