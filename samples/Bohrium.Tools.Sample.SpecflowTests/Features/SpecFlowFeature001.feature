@featureTag
Feature: SpecFlowFeature001
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: 
	Given I am logged as SA

@uspl8080 @mytag
Scenario: Add two numbers
	Given I have entered 50 into the calculator
	And I have entered 70 into the calculator
	And I have the following records
	| Id | Test Description       |
	| 1  | My Description Test    |
	| 2  | Mysdfsdfsdfiption Test |
	When I press add
	And I Go to the next when step
	Then the result should be 120 on the screen
	And I check the value Z
	"""
	tESTE
	HKSJGSDF
	 GSD 
	 GFSD FGSDFG
	 SD FG
	 SDFGSDFG
		
	"""
@uspl8081 @mytag
Scenario: Add three numbers
	Given I have entered 50 into the calculator
	And I have entered 70 into the calculator
	And I have entered 120 into the calculator
	And I have the following records
	| Id | Test Description    |
	| 1  | My Description Test |
	When I press add
	And I Go to the next when step
	Then the result should be 120 on the screen
	And I check the value x
	Given I have entered 80 into the calculator
	When I am logged as SA
	Then the result should be 80 on the screen
	And I check the value Z
	"""
	tESTE
	HKSJGSDF
	 GSD 
	 GFSD FGSDFG
	 SD FG
	 SDFGSDFG
		
	"""
	Given I have the following records
	| Id | Test Description    |
	| 10  | My Description Test1 |
	Then the result should be 81 on the screen
	Given I have entered 80 into the calculator
	Then the result should be 80 on the screen
