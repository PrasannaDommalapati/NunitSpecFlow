Feature: GoogleDemo
	As a Google user
	I want to search by keyword
	So that i can see the results

@Smoke @Regression @Sanity
Scenario: Search by text positive
	Given I am in "https://www.google.co.uk"
	When I enter a keyword "Selenium"
	Then I should see the page title contains "Selenium"

@Smoke @Regression @Sanity
Scenario: Search by text negative
	Given I am in "https://www.google.co.uk"
	When I enter a keyword "Selenium"
	Then I should see the page title contains "banana"