@featureTag
Feature: SpecFlowFeature001
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: 
	Given I am logged as SA

@uspl8080 @mytag
Scenario: Add two numbers
	When I check the current user
	Then I see the user SA
