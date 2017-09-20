﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;
using RestSharp;

namespace openweathemap.api.test
{
	[Binding]
	public class HolidayDayMakerSteps
	{
		private UriBuilder OpenWeatherMapUri { get; } = new UriBuilder("http://api.openweathermap.org/data/2.5/forecast");
		private DayOfWeek DayOfHoliday { get; set; }
		private IRestResponse Response { get; set; }
		private IList<double> MinimumTemp { get; } = new List<double>();

		[Given(@"I like to holiday in (.*)")]
		public void GivenILikeToHolidayIn(string city)
		{
			string apiKey = ConfigurationManager.AppSettings["apiKey"];
			OpenWeatherMapUri.Query = "q=" + city + "&units=metric&appid="+ apiKey;
		}

		[Given(@"I only like to holiday on (.*)")]
		public void GivenIOnlyLikeToHolidayOn(string day)
		{
			DayOfWeek holidaydayOfWeek;
			if (Enum.TryParse(day, true, out holidaydayOfWeek))
				DayOfHoliday = holidaydayOfWeek;
		}

		[When(@"I look up the weather forecast")]
		public void WhenILookUpTheWeatherForecast()
		{
			var client = new RestClient(OpenWeatherMapUri.Uri);
			var request = new RestRequest(Method.GET);
			Response = client.Execute(request);
		}

		[Then(@"I receive the weather forecast")]
		public void ThenIReceiveTheWeatherForecast()
		{
			Response.StatusCode.Should().Be(HttpStatusCode.OK,"We need a valid response to know the forecast.");
		}

		[Then(@"the temperature is warmer than (.*) degrees")]
		public void ThenTheTemperatureIsWarmerThanDegrees(string minimumTemperature)
		{
			double expectedMinumumTemp = Convert.ToDouble(minimumTemperature);
			var responseBody = JObject.Parse(Response.Content);
			var listOfForecasts = JArray.Parse(responseBody["list"].ToString()); 
			foreach (var forecast in listOfForecasts)
			{
				if (IsForecastFortheDayOfTheHoliday(forecast))
				{
					GetMinimumTemperature(forecast);
				}
			}
			MinimumTemp.Count.Should().BeGreaterThan(0,"We need the forecast for the day of the holiday");
			MinimumTemp.Min().Should().BeGreaterThan(expectedMinumumTemp, "It should be warmer");
		}

		private bool IsForecastFortheDayOfTheHoliday(JToken forecast)
		{
			var dateOfForcast = forecast["dt_txt"].Value<DateTime>();
			return dateOfForcast.DayOfWeek.Equals(DayOfHoliday);
		}

		private void GetMinimumTemperature(JToken forecast)
		{
			var minimumTempForQuarter = forecast["main"]["temp_min"].Value<double>();
			MinimumTemp.Add(minimumTempForQuarter);
		}
	}
}
