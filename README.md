# Debatrix

#### A platform for online debate, Sept. 9, 2016

#### By Callan McNulty

## Description

This app will provide users with a purpose-built platform for debating online. It will feature intuitive display that clearly represents argument structure, a scoring system by which strong arguments can be voted for, user authentication, and built in citation making tools.

## Setup/Installation Requirements

* Clone repository from GitHub
* To get your API key, make the following http post request inserting your email address where indicated (this can be done in your browser): http://api.dp.la/v2/api_key/YOUR_EMAIL@example.com
* Check your email for your API key
* Add a file names EnvironmentVariables.cs to the Models folder in the src directory
* Add the following text to that file, inserting your API key where indicated: namespace DebatePlatform.Models { public class EnvironmentVariables { public static string DPLAKey = "YOUR_API_KEY"; } }
* Navigate to cloned directory and run the command: dotnet ef database update
* If Visual Studio is installed, open DebatePlatform.sln in Visual Studio
* Click the green 'Run in IIS' button
* If Visual Studio is not installed, IIS must be installed and configured according to your machine's needs

## Support and contact details

I can be contacted for support at jabberwocky222@gmail.com

## Technologies Used

* C#
* ASP.NET
* SQL
* Entity Core
* HTML
* CSS

Copyright (c) 2016 Callan McNulty
