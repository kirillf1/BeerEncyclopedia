using ShopBeerService.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerEncyclopedia.Tests.ShopBeerService.BeerShopParserTests
{
    public class StartTimeServiceArgsTests
    {
        [Fact]
        public void GetScheduledDate_CurrentDayPlusHours_ReturnDifferenceHours()
        {
            var expectedHoursDifference = 2;
            var timeZoneId = "Russian Standard Time";
            var currentDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, timeZoneId);
            var serviceArgs = new StartTimeServiceArgs(currentDate.DayOfWeek,currentDate.Hour + expectedHoursDifference,timeZoneId);

            var timeDifference = serviceArgs.GetDelayTime();
            
            Assert.Equal(expectedHoursDifference, Math.Round( timeDifference.TotalHours, MidpointRounding.ToPositiveInfinity));
        }
    }
}
