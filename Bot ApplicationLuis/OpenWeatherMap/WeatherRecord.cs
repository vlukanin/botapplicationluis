﻿namespace Bot_ApplicationLuis.OpenWeatherMap
{
    using System;

    public class WeatherRecord
    {
        public double Temp { get; set; }
        public double Pressure { get; set; }
        public int Humidity { get; set; }
        public DateTime When { get; set; }
        public string Date
        {
            get { return $"{this.When.Day:D2}.{this.When.Month:D2}.{this.When.Year:D4}"; }
        }

        public string FullDate
        {
            get { return this.When.ToString("D"); }
        }

    }
}