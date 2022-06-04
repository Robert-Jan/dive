using System;

namespace Dive.App.ViewModels
{
    public abstract class BaseViewModel
    {
        public TimeZoneInfo Timezone => TimeZoneInfo.FindSystemTimeZoneById("Europe/Amsterdam");
    }
}