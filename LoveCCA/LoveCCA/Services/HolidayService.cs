using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoveCCA.Services
{
    public interface IHolidayService
    {
        Task<SchoolYearSettings> LoadSchoolSettings();
    }
    public class HolidayService : IHolidayService
    {
        public bool IsCurrentSchoolYear(int schoolStartYear)
        {
            if (DateTime.Now.Month > 7)
            {
                return schoolStartYear == DateTime.Now.Year;
            } 
            else
            {
                return (DateTime.Now.Year == schoolStartYear + 1);
            }
        }

        public virtual async Task<SchoolYearSettings> LoadSchoolSettings()
        {
            try
            {
                var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .GetCollection("school_year_settings")
                                     .GetDocument("current_year")
                                     .GetDocumentAsync();

                var document = query.ToObject<SchoolYearSettings>();

                if (IsCurrentSchoolYear(document.YearStart.Year))
                {
                    return document;
                } 
                else
                {
                    return GetDefault();
                }

            }
            catch (Exception)
            {
                return GetDefault();
            }

        }

        protected SchoolYearSettings GetDefault()
        {
            return new SchoolYearSettings
            {
                YearStart = new DateTime(DateTime.Now.Year, 9, 3),
                YearEnd = new DateTime(DateTime.Now.Year + 1, 6, 30),
                Holidays = new List<Holiday>()
            };

        }
    }
}
