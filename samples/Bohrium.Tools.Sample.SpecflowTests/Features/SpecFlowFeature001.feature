@featureTag
Feature: SpecFlowFeature001
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@uspl8080 @mytag
Scenario: Add two numbers
	Given I have entered 50 into the calculator
	And I have entered 70 into the calculator
	And I have the following records
	| Id | Test Description    |
	| 1  | My Description Test |
	When I press add
	And I Go to the next when step
	Then the result should be 120 on the screen
	And I check the value x
