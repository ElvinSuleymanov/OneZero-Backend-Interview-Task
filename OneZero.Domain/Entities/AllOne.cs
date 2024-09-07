
namespace OneZero.Domain.Entities
{
    public class Data
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Category> Categories { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public bool IsPublished { get; set; }
        public bool IsArchived { get; set; }
        public int PlaceInTheList { get; set; }
        public Schedule Schedule { get; set; }
        public List<Name> Name { get; set; }
        public List<MenuItem> MenuItems { get; set; }
    }

    public class Schedule
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public bool? IsActive { get; set; }
        public Weekdays? Weekdays { get; set; }
    }

    public class Weekdays
    {
        public bool? Monday { get; set; }
        public bool? Tuesday { get; set; }
        public bool? Wednesday { get; set; }
        public bool? Thursday { get; set; }
        public bool? Friday { get; set; }
        public bool? Saturday { get; set; }
        public bool? Sunday { get; set; }
    }

    public class Name
    {
        public string Value { get; set; }
        public string LanguageCode { get; set; }
    }

    public class MenuItem
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public List<Name> Name { get; set; }
        public List<Description> Description { get; set; }
        public string CoverImageSrc { get; set; }
        public List<string> OtherImagesSrc { get; set; }
        public double PriceSell { get; set; }
        public double? PriceCost { get; set; }
        public int PlaceInTheList { get; set; }
        public Unit Unit { get; set; } 
        public double Amount { get; set; }
        public double Calories { get; set; }
        public int TimeToMake { get; set; }
        public bool IsArchived { get; set; }
        public Rate Rate { get; set; }
    }

    public class Description
    {
        public string Value { get; set; }
        public string LanguageCode { get; set; }
    }

    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Day
    {
        public bool? IsWorking { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
    public class Rate
    {
        public bool IsFixed { get; set; }
        public double Amount { get; set; }
        public bool IsEnabled { get; set; }
        public Schedule Schedule { get; set; }
    }

    public class DataWrapper
    {
        public Data Data { get; set; }
    }

}
