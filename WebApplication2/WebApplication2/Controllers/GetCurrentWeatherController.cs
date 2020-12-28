using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace WebApplication2.Controllers
{
    public class GetCurrentWeatherController : ApiController
    {
        // GET api/GetCurrentWeather
        public string Get()
        {
            return "Enter city";
        }

        // GET api/GetCurrentWeather/city
        public IEnumerable<string> Get(string city)
        {
            const string URL = "http://api.openweathermap.org/data/2.5/";
            const string key = "69460fff466fc39c2db1e0e3f9633ff2";      // my key on openweathermap.org   
            string urlParametersForCurrentWeather = String.Format("weather?q={0}&units=metric&appid={1}", city, key);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            HttpResponseMessage response = client.GetAsync(urlParametersForCurrentWeather).Result;  // get the response to the API call

            if (response.IsSuccessStatusCode)   // if everything is OK -> return the data
            {
                var dataObjects = response.Content.ReadAsStringAsync().Result;

                // Parse the string with forecast
                JObject weather = JObject.Parse(dataObjects);   

                // Initialize data
                string cur_weather = "Current weather: " + weather["weather"][0]["main"];
                string cur_temp = "Current temperature: " + weather["main"]["temp"] + "C";
                string max_temp = "Max temperature: " + weather["main"]["temp_max"] + "C";
                string min_temp = "Min temperature: " + weather["main"]["temp_min"] + "C";
                string wind_speed = "Wind speed: " + weather["wind"]["speed"] + "km/h";
                string cloudy = "Cloudy percentage: " + weather["clouds"]["all"] + "%";

                return new string[] 
                { 
                    city, 
                    DateTime.Now.ToString("dd-MM-yyyy"),
                    cur_weather,
                    cur_temp,
                    max_temp,
                    min_temp,
                    wind_speed,
                    cloudy
                };
            }
            else    // if there is an error occured -> return the response status code and reason phrase
            {                
                return new string[] { response.StatusCode.ToString(), response.ReasonPhrase };
            }
        }
    }
}
