# WeatherCompare
A messy proof-of-concept C# WPF program to request temperature station data from the Netatmo Weather API. Uses [JSON.Net](http://www.newtonsoft.com/json).
# Important Notes
This tool is a proof-of-concept, meaning it uses quite a bunch of hardcoded strings/values and also stores account data in the source code itself in the current state. The account data is required to request the initial accesstoken for the API. After the initial request, the refresh token gets used instead of the user name and password.

The interface is currently in German, however the code uses English comments.
# How to Setup
* Add your clientID and clientSecret from the Netatmo Connect site in Properties->Settings
* Add your Netatmo Connect username and password into NetatmoSession.cs
* If you want to change the requested stations, look for the "HARDCODED" comments in MainWindow.xaml.cs.
# TODO:
* Add a proper security concept (don't store credentials in source code, duh).
* Use .NET naming conventions (current naming conventions is mixed-up with the API responses and improperly used camel type)
* Better code structure
* Store the weather stations to request in a database or config file instead of hardcoding them.
