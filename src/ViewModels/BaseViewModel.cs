using System;

namespace Dive.App.ViewModels
{
    public abstract class BaseViewModel
    {
        public TimeZoneInfo Timezone => TimeZoneInfo.FindSystemTimeZoneById("Europe/Amsterdam");

        public string GetTimestamp(DateTime date) => TimeZoneInfo
            .ConvertTime(date, Timezone)
            .ToString("dd-MM-yyyy HH:mm");
    }
}