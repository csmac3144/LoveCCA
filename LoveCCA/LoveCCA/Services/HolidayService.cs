using LoveCCA.Models;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoveCCA.Services
{
    public interface IHolidayService
    {
    }
    public class HolidayService : IHolidayService
    {
        public static DateTime GetStartOfCurrentSchoolYear()
        {
            if (DateTime.Now.Month > 7)
            {
                return new DateTime(DateTime.Now.Year,9,1);
            }
            else
            {
                return new DateTime(DateTime.Now.Year - 1, 9, 1);
            }

        }

    }
}
