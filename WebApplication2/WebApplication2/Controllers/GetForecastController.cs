using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System.Xml.Serialization;

namespace WebApplication2.Controllers
{
    public class GetForecastController : ApiController
    {
        // GET api/GetForecastController
        public string Get()
        {
            return "Enter city";
        }

        // GET api/GetForecastController/city
        public IEnumerable<string> Get(string city)
        {
            const string URL = "http://api.openweathermap.org/data/2.5/";
            const string key = "69460fff466fc39c2db1e0e3f9633ff2";  // my key on openweathermap.org
            string urlParametersForForecast = String.Format("forecast?q={0}&units=metric&appid={1}", city, key);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);
                        
            HttpResponseMessage response = client.GetAsync(urlParametersForForecast).Result;    // get the response to the API call

            if (response.IsSuccessStatusCode)   // if everything is OK -> return the data
            {
                var dataObjects = response.Content.ReadAsStringAsync().Result;
                JObject forecast = JObject.Parse(dataObjects);

                string prev_date = "";
                DailyForecast[] days = new DailyForecast[6];    // array of params by day

                int i = 0;  // iterator

                // iteration over elements by date
                foreach (JObject day in forecast["list"])   
                {                  
                    DateTime date = Convert.ToDateTime(day["dt_txt"]);
                    
                    // Turning date into ISO
                    string current_date = date.ToString("dd-MM-yyyy");  

                    if (current_date != prev_date)  // if the forecast for a new day -> initialize new data
                    {
                        days[i] = new DailyForecast(date, 
                            
                            Convert.ToInt32(day["main"]["temp_min"]), 
                            Convert.ToInt32(day["main"]["temp_max"]), 
                            Convert.ToInt32(day["wind"]["speed"]), 
                            Convert.ToInt32(day["clouds"]["all"]));

                        i++;    // next day
                    }
                    // Correction of temperatures
                    else
                    {
                        // find minimum temp for the day
                        if (Convert.ToInt32(day["main"]["temp_min"]) < days[i - 1].MinTemperature)  
                        {
                            days[i - 1].MinTemperature = Convert.ToInt32(day["main"]["temp_min"]);
                        }

                        // find maximum temp for the day
                        if (Convert.ToInt32(day["main"]["temp_max"]) > days[i - 1].MaxTemperature)  
                        {
                            days[i - 1].MaxTemperature = Convert.ToInt32(day["main"]["temp_max"]);
                        }
                    }

                    prev_date = current_date;
                    
                }

                string[] output_string = new string[6];
                
                i = 0;  // reset the iterator

                // Create an output string which contains all of the parameters by day
                foreach (DailyForecast day in days)
                {
                    output_string[i] = day.Date.ToString("dd-MM-yyyy") + 
                        " Min temp: " + day.MinTemperature + "C " + 
                        " Max temp: " + day.MaxTemperature + "C " + 
                        " Wind speed: " + day.WindSpeed + " km/h " + 
                        " Cloudy: " + day.CloudyPercentage + "%";
                    
                    i++;
                }

                return Enumerable.Range(0, 5).Select(index => output_string[index]);
            }
            else    // if there is an error occured -> return the response status code and reason phrase
            {                
                return new string[] { response.StatusCode.ToString(), response.ReasonPhrase };
            }
        }

        //// POST api/GetForecastController
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/GetForecastController/city
        //public void Put(string city, [FromBody] string value)
        //{
        //}

        //// DELETE api/GetForecastController/city
        //public void Delete(string city)
        //{
        //}
    }
}
