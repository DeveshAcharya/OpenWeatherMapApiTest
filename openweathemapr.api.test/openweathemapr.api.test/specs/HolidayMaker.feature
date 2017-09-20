Feature: HolidayDayMaker
	In order to plan the holidays
	As a holidaymaker
	I want to know the weather forecast.

Scenario Outline: A happy holiday maker
	Given I like to holiday in <city>
	And I only like to holiday on <day>
	When I look up the weather forecast
	Then I receive the weather forecast
	And the temperature is warmer than <minimum temperature> degrees
	
	Examples:
    | city   | day       | minimum temperature |
    | Sydney | Thursday  | 10                  |
    | Moscow | Thursday  | 10                  |
    