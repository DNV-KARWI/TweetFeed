using System;
using System.Globalization;

namespace TwitterFeed.Helpers
{
    public class ConvertDates
    {
        public DateTime ConvertToTweeterCustomFormat(string date)
        {
            return DateTime.ParseExact(date, "ddd MMM d HH:mm:ss +0000 yyyy", new CultureInfo("US-us"));
        }
    }
}
