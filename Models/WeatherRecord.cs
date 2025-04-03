using System;
using System.ComponentModel.DataAnnotations;

namespace WeatherArchive.Models
{
    public class WeatherRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public double? Temperature { get; set; }
        public double? Humidity { get; set; }
        public double? DewPoint { get; set; }
        public int? Pressure { get; set; }
        public string? WindDirection { get; set; }
        public int? WindSpeed { get; set; }

        public int? CloudCover { get; set; }
        public int? CloudBase { get; set; }
        public int? HorizontalVisibility { get; set; }
        public string? WeatherPhenomena { get; set; }
    }
}
