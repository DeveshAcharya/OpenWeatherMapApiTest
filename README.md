# OpenWeatherMapApiTest
Welcome to the Test Suite for OpenWeatherApi forecast endpoint.

## Getting Started

Its quite striaght forward to get started once you clone the repo.
- Open the solution in visual studio and build the solution.
- Place the apikey credential file "AppSettingsSecrets.config" provided in the folder "C:\OpenWeatheMapApiTest". 
  - in case you don't have this just get an api key from https://openweathermap.org/appid and create a file with given name and put below 
     ``` xml
     <appSettings>
          <add key="apiKey" value="your api key" />
      </appSettings>
     ```
      
- If you want to change the location just replace the file location in the app.config where you are placing the credential file.  (unfortunatly it doesn't allow relative paths.)
- Run the tests in visual studio test runner.
