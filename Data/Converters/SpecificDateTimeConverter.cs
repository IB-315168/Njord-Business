using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcNjordClient.SpecificDateTime;

namespace Data.Converters
{
    public class SpecificDateTimeConverter
    {
        public static DateTime convertToDateTime(SpecificDateTime specificDateTime)
        {
            return new DateTime(specificDateTime.Year, specificDateTime.Month,
                specificDateTime.Day, specificDateTime.Hour, specificDateTime.Minute, 0);
        }

        public static SpecificDateTime convertToSpecificDateTime(DateTime dateTime)
        {
            return new SpecificDateTime()
            {
                Year = dateTime.Year,
                Month = dateTime.Month,
                Day = dateTime.Day,
                Hour = dateTime.Hour,
                Minute = dateTime.Minute
            };
        }

        public static DateTime ConvertToTime(SpecificTime specificTime)
        {
            return new DateTime(1970, 1, 1, specificTime.Hour, specificTime.Minute, specificTime.Second);
        }

        public static SpecificTime ConvertToSpecificTime(DateTime dateTime)
        {
            return new SpecificTime()
            {
                Hour = dateTime.Hour,
                Minute = dateTime.Minute,
                Second = dateTime.Second
            };
        }
    }
}
