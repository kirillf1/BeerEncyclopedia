namespace ShopBeerService.Workers
{
    public class StartTimeServiceArgs
    {
        public StartTimeServiceArgs(DayOfWeek DayOfWeek, int hour, string timeZoneId = "Russian Standard Time")
        {
            this.DayOfWeek = DayOfWeek;
            if (hour < 0 || hour > 24)
                throw new ArgumentException("Invalid hour value");
            Hour = hour;
            TimeZoneId = timeZoneId;
        }

        public DayOfWeek DayOfWeek { get; }
        public int Hour { get; }
        public string TimeZoneId { get; set; }

        public TimeSpan GetDelayTime()
        {
            return GetScheduledDate().Subtract(GetCurrentDate());
        }
        private DateTime GetScheduledDate()
        {
            var dateTime = GetCurrentDate();
            var offset = DayOfWeek - dateTime.DayOfWeek;
            if (offset < 0)
                offset = 7 + offset;
            return dateTime.AddDays(offset).
                Date.Add(new TimeSpan(Hour, 0, 0));      
        }
        private DateTime GetCurrentDate()
        {
           return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, TimeZoneId);
        }
    }
}
