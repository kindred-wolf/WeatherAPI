using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WebApplication2
{
    public class DailyForecast
    {
        public DailyForecast(DateTime date, int min_temp, int max_temp, int wind_speed, int cloudy)
        {
            this.Date = date;
            this.MinTemperature = min_temp;
            this.MaxTemperature = max_temp;
            this.WindSpeed = wind_speed;
            this.CloudyPercentage = cloudy;            
        }

        public DateTime Date { get; set; }

        public string Weather { get; set; }

        public int MinTemperature { get; set; }

        public int MaxTemperature { get; set; }

        public int WindSpeed { get; set; }

        public int CloudyPercentage { get; set; }
    }
}