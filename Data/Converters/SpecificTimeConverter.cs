using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcNjordClient;

namespace Data.Converters
{
    public class SpecificTimeConverter
    {
        public static DateTime convertToDateTime(SpecificTime specificTime)
        {
            return new DateTime(specificTime.Year, specificTime.Month, 
                specificTime.Day, specificTime.Hour, specificTime.Minute, 0);
        }

        public static SpecificTime convertToSpecificTime(DateTime dateTime)
        {
            return new SpecificTime()
            {
                Year = dateTime.Year,
                Month = dateTime.Month,
                Day = dateTime.Day,
                Hour = dateTime.Hour,
                Minute = dateTime.Minute
            };
        }
    }
}
