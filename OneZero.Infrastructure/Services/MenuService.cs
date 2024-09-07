using OneZero.Application.Interfaces;
using OneZero.Domain.Entities;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;


namespace OneZero.Infrastructure.Services
{
    public class MenuService : IMenuService
    {
        public async Task<DataWrapper> ReadDataJson()
        {
            StringBuilder builder = new StringBuilder();
            string dir = Environment.CurrentDirectory;
            using (FileStream fs = new("wwwroot/data.json", FileMode.Open, FileAccess.Read))
            {
                MemoryStream ms = new();
                await fs.CopyToAsync(ms);
                byte[] arr = ms.ToArray();
                var result = Encoding.UTF8.GetString(arr, 0, arr.Length);
                builder.Append(result);
            }
            JsonSerializerSettings settings = new()
            {
                Converters = { new NullableBooleanConverter(), new WeekdaysConverter() },
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };
            DataWrapper menu = JsonConvert.DeserializeObject<DataWrapper>(builder.ToString(), settings);

            var categories = menu.Data.Categories.Where(x => x.MenuItems.Count > 0 && x.MenuItems.Where(x => x.IsArchived).Any());
            menu.Data.Categories = categories.ToList();
            return menu;
        }
        public async Task<DataWrapper> GetAllMenu(DateTime Date)
        {


            DataWrapper menu = await ReadDataJson();
            
            if (Date == DateTime.MinValue)
            {
                return menu;
            }
            foreach (var category in menu.Data.Categories)
            {
                foreach (var menuItem in category.MenuItems)
                {
                    if (menuItem.Rate.Schedule.From < Date && Date < menuItem.Rate.Schedule.To)
                    {
                        menuItem.Rate.Schedule.IsActive = true;
                    }
                    bool isWorkingDay = Date.DayOfWeek switch
                    {
                        DayOfWeek.Monday => menuItem.Rate.Schedule.Weekdays.Monday == true,
                        DayOfWeek.Tuesday => menuItem.Rate.Schedule.Weekdays.Tuesday == true,
                        DayOfWeek.Wednesday => menuItem.Rate.Schedule.Weekdays.Wednesday == true,
                        DayOfWeek.Thursday => menuItem.Rate.Schedule.Weekdays.Thursday == true,
                        DayOfWeek.Friday => menuItem.Rate.Schedule.Weekdays.Friday == true,
                        DayOfWeek.Saturday => menuItem.Rate.Schedule.Weekdays.Saturday == true,
                        DayOfWeek.Sunday => menuItem.Rate.Schedule.Weekdays.Sunday == true,
                        _ => false
                    };
                    if (menuItem.IsArchived && menuItem.Rate.IsEnabled && isWorkingDay) 
                    {
                        if (menuItem.Rate.IsFixed)
                        {
                            menuItem.PriceSell -= menuItem.Rate.Amount;

                        }
                        else
                        {
                            menuItem.PriceSell -= (menuItem.PriceSell * menuItem.Rate.Amount / 100);
                        }
                    }
                }
            }

            return menu;
        }

        public async Task<MenuItem> GetProductById(int Id, DateTime Date)
        {
            DataWrapper menu = await ReadDataJson();
            var result = menu.Data.Categories.SelectMany(x => x.MenuItems).Where(x => x.Id == Id).FirstOrDefault();
            if (Date == DateTime.MinValue)
            {
                return result;
            }
            var isValidWorkingDay = Date.DayOfWeek switch
            {
                DayOfWeek.Monday => result.Rate.Schedule.Weekdays.Monday == true,
                DayOfWeek.Tuesday => result.Rate.Schedule.Weekdays.Tuesday == true,
                DayOfWeek.Wednesday => result.Rate.Schedule.Weekdays.Wednesday == true,
                DayOfWeek.Thursday => result.Rate.Schedule.Weekdays.Thursday == true,
                DayOfWeek.Friday => result.Rate.Schedule.Weekdays.Friday == true,
                DayOfWeek.Saturday => result.Rate.Schedule.Weekdays.Saturday == true,
                DayOfWeek.Sunday => result.Rate.Schedule.Weekdays.Sunday == true,
                _ => false
            };


          
            if (result.Rate.IsEnabled)
            {
                if (!isValidWorkingDay)
                {
                    throw new Exception("It's not working day");
                }
                if (result.Rate.Schedule.From < Date && Date < result.Rate.Schedule.To)
                {
                    result.Rate.Schedule.IsActive = true;
                }
                if (result.Rate.IsFixed)
                {
                    result.PriceSell -= result.Rate.Amount;
                }
                else
                {
                    result.PriceSell -= result.Rate.Amount * result.PriceSell / 100;
                }
            }
            return result;
        }

        public class NullableBooleanConverter : JsonConverter<bool?>
        {

            public override bool? ReadJson(JsonReader reader, Type objectType, bool? existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null)
                    return null;

                if (reader.TokenType == JsonToken.Boolean)
                    return (bool)reader.Value;

                
                return null;
            }

            public override void WriteJson(JsonWriter writer, bool? value, JsonSerializer serializer)
            {
                if (value.HasValue)
                    writer.WriteValue(value.Value);
                else
                    writer.WriteNull();
            }
        }
        public class WeekdaysConverter : JsonConverter<Weekdays>
            {

                    public override Weekdays? ReadJson(JsonReader reader, Type objectType, Weekdays? existingValue, bool hasExistingValue, JsonSerializer serializer)
                    {
                        var jsonObject = JObject.Load(reader);
                        var weekdays = new Weekdays();

                        foreach (var property in jsonObject.Properties())
                        {
                            var day = property.Name;
                            var dayObject = property.Value.ToObject<Day>();

                            switch (day)
                            {
                                case "monday":
                                    weekdays.Monday = dayObject.IsWorking;
                                    break;
                                case "tuesday":
                                    weekdays.Tuesday = dayObject.IsWorking;
                                    break;
                                case "wednesday":
                                    weekdays.Wednesday = dayObject.IsWorking;
                                    break;
                                case "thursday":
                                    weekdays.Thursday = dayObject.IsWorking;
                                    break;
                                case "friday":
                                    weekdays.Friday = dayObject.IsWorking;
                                    break;
                                case "saturday":
                                    weekdays.Saturday = dayObject.IsWorking;
                                    break;
                                case "sunday":
                                    weekdays.Sunday = dayObject.IsWorking;
                                    break;
                            }
                        }

                        return weekdays;
                    }

                    public override void WriteJson(JsonWriter writer, Weekdays value, JsonSerializer serializer)
                {
                    throw new NotImplementedException();
                }
            }

        }
}
