Feature: Read To Do Items

As a user, 
I want to read my To Do Items
So that I can manage my tasks for the day

@tag1
Scenario: Get To Do Items -> Lists all To Do Items
	Given a list of To Do Items
	| Id | Name     | Completed |
	| 1  | Grocery  | true      |
	| 2  | Exercise | false     |
	When the list of To Do Items are requested
	Then the list of To Do Items
	| Id | Name     | Completed |
	| 1  | Grocery  | true      |
	| 2  | Exercise | false     |
