using LoveCCA.Models;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveCCA.Services
{
    public class SchoolConfigurationService
    {
        private SchoolYearConfiguration _schoolYearConfiguration;

        static SchoolConfigurationService _schoolConfigurationService;
        public static SchoolConfigurationService Instance
        {
            get
            {
                if (_schoolConfigurationService == null)
                    _schoolConfigurationService = new SchoolConfigurationService();
                return _schoolConfigurationService;
            }
        }

        /// <summary>
        /// Only used for loading data during development
        /// </summary>
        /// <returns></returns>
        public async Task UpdateSchoolSettings(SchoolYearConfiguration settings)
        {
            try
            {
                var query = await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("school_year_settings")
                         .GetDocumentsAsync();

                var documents = (query.ToObjects<SchoolYearConfiguration>()).ToList();

                foreach (var doc in documents)
                {
                    await CrossCloudFirestore.Current
                             .Instance
                             .GetCollection("school_year_settings")
                             .GetDocument(doc.Id)
                             .DeleteDocumentAsync();
                }

                await CrossCloudFirestore.Current
                             .Instance
                             .GetCollection("school_year_settings")
                             .AddDocumentAsync(settings);


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<SchoolYearConfiguration> GetSchoolConfiguration()
        {
            if (_schoolYearConfiguration != null)
            {
                return _schoolYearConfiguration;
            }

            try
            {
                var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .GetCollection("school_year_settings")
                                     .GetDocumentsAsync();

                var document = (query.ToObjects<SchoolYearConfiguration>()).FirstOrDefault();

                if (IsCurrentSchoolYear(document.YearStart.Year))
                {
                    _schoolYearConfiguration = document;
                    return _schoolYearConfiguration;

                }
                else
                {
                    _schoolYearConfiguration = GetDefault();
                    return _schoolYearConfiguration;

                }

            }
            catch (Exception ex)
            {
                _schoolYearConfiguration = GetDefault();
                return _schoolYearConfiguration;
            }

        }

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

        protected SchoolYearConfiguration GetDefault()
        {
            return new SchoolYearConfiguration
            {
                YearStart = new DateTime(DateTime.Now.Year, 9, 3),
                YearEnd = new DateTime(DateTime.Now.Year + 1, 6, 30),
                SpecialDays = new List<SpecialDay>(),
                MealWeekMenuRotationSchedule = new List<Models.MealWeekRotation>(),
                HotLunchMenu = new List<HotLunchMenu>()
            };

        }

        public async Task GenerateConfig()
        {
            var slots = new List<MealWeekRotation>
            {
                new MealWeekRotation(DateTime.Parse("2020-09-14"),1),
                new MealWeekRotation(DateTime.Parse("2020-09-21"),2),
                new MealWeekRotation(DateTime.Parse("2020-09-28"),1),
                new MealWeekRotation(DateTime.Parse("2020-10-05"),2),
                new MealWeekRotation(DateTime.Parse("2020-10-12"),1),
                new MealWeekRotation(DateTime.Parse("2020-10-19"),2),
                new MealWeekRotation(DateTime.Parse("2020-10-26"),1),
                new MealWeekRotation(DateTime.Parse("2020-11-02"),2),
                new MealWeekRotation(DateTime.Parse("2020-11-09"),0),
                new MealWeekRotation(DateTime.Parse("2020-11-16"),1),
                new MealWeekRotation(DateTime.Parse("2020-11-23"),2),
                new MealWeekRotation(DateTime.Parse("2020-11-30"),1),
                new MealWeekRotation(DateTime.Parse("2020-12-07"),2),
                new MealWeekRotation(DateTime.Parse("2020-12-14"),0),
                new MealWeekRotation(DateTime.Parse("2020-12-21"),0),
                new MealWeekRotation(DateTime.Parse("2020-12-28"),0),
                new MealWeekRotation(DateTime.Parse("2021-01-04"),1),
                new MealWeekRotation(DateTime.Parse("2021-01-11"),2),
                new MealWeekRotation(DateTime.Parse("2021-01-18"),1),
                new MealWeekRotation(DateTime.Parse("2021-01-25"),2),
                new MealWeekRotation(DateTime.Parse("2021-02-01"),1),
                new MealWeekRotation(DateTime.Parse("2021-02-08"),2),
                new MealWeekRotation(DateTime.Parse("2021-02-15"),1),
                new MealWeekRotation(DateTime.Parse("2021-02-22"),2),
                new MealWeekRotation(DateTime.Parse("2021-03-01"),1),
                new MealWeekRotation(DateTime.Parse("2021-03-08"),2),
                new MealWeekRotation(DateTime.Parse("2021-03-15"),0),
                new MealWeekRotation(DateTime.Parse("2021-03-22"),1),
                new MealWeekRotation(DateTime.Parse("2021-03-29"),2),
                new MealWeekRotation(DateTime.Parse("2021-04-05"),1),
                new MealWeekRotation(DateTime.Parse("2021-04-12"),2),
                new MealWeekRotation(DateTime.Parse("2021-04-19"),1),
                new MealWeekRotation(DateTime.Parse("2021-04-26"),2),
                new MealWeekRotation(DateTime.Parse("2021-05-03"),1),
                new MealWeekRotation(DateTime.Parse("2021-05-10"),2),
                new MealWeekRotation(DateTime.Parse("2021-05-17"),1),
                new MealWeekRotation(DateTime.Parse("2021-05-24"),2),
                new MealWeekRotation(DateTime.Parse("2021-05-31"),1),
                new MealWeekRotation(DateTime.Parse("2021-06-07"),2)
            };

            var prods = new List<Product>
            {
                new Product { Id = Guid.NewGuid().ToString(), Name = "Hot Meal", Glyph = "🍱", MenuIndex = 1,
                ShortDescription = "Lunchbox (ham and cheese)",
                Description = "Lunchboxes - 1/2 Sub - Ham and cheese with mayo and lettuce, pkg of goldfish crackers, applesauce and juicebox.",
                Price = 5 },
                new Product { Id = Guid.NewGuid().ToString(), Name = "Hot Meal", Glyph = "🍱", MenuIndex = 2,
                ShortDescription = "Lunchbox (ham and salami)",
                Description = "Lunchboxes - 1/2 Sub - Cold cut (ham and salami) with mayo and lettuce, pkg of goldfish crackers, applesauce and juicebox.",
                Price = 5 },
                new Product { Id = Guid.NewGuid().ToString(), Name = "Hot Meal", Glyph = "🍱", MenuIndex = 3,
                ShortDescription = "Lunchbox (sliced turkey)",
                Description = "Lunchboxes - 1/2 Sub - Sliced turkey with mayo and lettuce, pkg of goldfish crackers, applesauce and juicebox.",
                Price = 5 },
                new Product { Id = Guid.NewGuid().ToString(), Name = "Hot Meal", Glyph = "🥗", MenuIndex = 4,
                ShortDescription = "Caesar Salad.",
                Description = "Caesar Salad.",
                Price = 3 },
                new Product { Id = Guid.NewGuid().ToString(), Name = "Hot Meal", Glyph = "🥖", MenuIndex = 5,
                ShortDescription = "Sub (ham and cheese)",
                Description = "Sub (approx. 9 inch) Ham and cheese with mayo and lettuce.",
                Price = 4 },
                new Product { Id = Guid.NewGuid().ToString(), Name = "Hot Meal", Glyph = "🥖", MenuIndex = 6,
                ShortDescription = "Sub (ham and salami)",
                Description = "Sub (approx. 9 inch) Cold cut (ham and salami) with mayo and lettuce.",
                Price = 4 },
                new Product { Id = Guid.NewGuid().ToString(), Name = "Hot Meal", Glyph = "🥖", MenuIndex = 7,
                ShortDescription = "Sub (sliced turkey)",
                Description = "Sub (approx. 9 inch) Sliced turkey with mayo and lettuce.",
                Price = 4 },
                new Product { Id = Guid.NewGuid().ToString(), Name = "Hot Meal", Glyph = "🍽️", MenuIndex = 9,
                ShortDescription = "Shepherd's Pie",
                Description = "Shepherd's Pie.",
                Price = 4 },
                new Product { Id = Guid.NewGuid().ToString(), Name = "Hot Meal", Glyph = "🥗", MenuIndex = 10,
                ShortDescription = "Garden Salad",
                Description = "Garden Salad.",
                Price = 3 },
                new Product { Id = Guid.NewGuid().ToString(), Name = "Hot Meal", Glyph = "🌯", MenuIndex = 11,
                ShortDescription = "Quesadilla (chicken and cheese)",
                Description = "Quesadillas - chicken and cheese",
                Price = 4.5M },
                new Product { Id = Guid.NewGuid().ToString(), Name = "Hot Meal", Glyph = "🌯", MenuIndex = 12,
                ShortDescription = "Quesadilla (cheese)",
                Description = "Quesadillas - cheese",
                Price = 4.5M },
                new Product { Id = Guid.NewGuid().ToString(), Name = "Hot Meal", Glyph = "🍕", MenuIndex = 14,
                ShortDescription = "Pepperoni Pizza",
                Description = "Pepperoni Pizza from Mario's Pizza.",
                Price = 2.5M },
                new Product { Id = Guid.NewGuid().ToString(), Name = "Hot Meal", Glyph = "🍕", MenuIndex = 15,
                ShortDescription = "Cheese Pizza",
                Description = "Cheese Pizza from Mario's Pizza.",
                Price = 2.5M },
                new Product { Id = Guid.NewGuid().ToString(), Name = "Hot Meal", Glyph = "🥙", MenuIndex = 16,
                ShortDescription = "Chicken Shawarma",
                Description = "Chicken Shawarma wrap with lettuce and mayo, from Mezza Lebanese",
                Price = 5M },
                new Product { Id = Guid.NewGuid().ToString(),
                ShortDescription = "Milk", Name = "Milk", Glyph = "🥛", MenuIndex = 0,
                Description = "Single Serving of Milk", 
                Price = 0.45M }
            };

            await ProductService.UpdateProducts(prods);


            var items = new List<HotLunchMenu>
            {
                new HotLunchMenu { DayOfWeek = (int)DayOfWeek.Wednesday, MenuNumber = 1, ProductOptionIndexes = new List<int> {1,2,3,4}},
                new HotLunchMenu { DayOfWeek = (int)DayOfWeek.Wednesday, MenuNumber = 2, ProductOptionIndexes = new List<int> {5,6,7,4}},
                new HotLunchMenu { DayOfWeek = (int)DayOfWeek.Thursday, MenuNumber = 1, ProductOptionIndexes = new List<int> {9,10}},
                new HotLunchMenu { DayOfWeek = (int)DayOfWeek.Thursday, MenuNumber = 2, ProductOptionIndexes = new List<int> {11,12,10}},
                new HotLunchMenu { DayOfWeek = (int)DayOfWeek.Friday, MenuNumber = 1, ProductOptionIndexes = new List<int> {14,15}},
                new HotLunchMenu { DayOfWeek = (int)DayOfWeek.Friday, MenuNumber = 2,ProductOptionIndexes = new List<int> {16}}
            };

            var specialDays = new List<SpecialDay>
            {
                new SpecialDay { Date = new DateTime(2020,09,9), Glyph="🏫", Description = "First Day of School", IsSchoolDay = true},
                new SpecialDay { Date = new DateTime(2020,09,16), Glyph="🏫", Description = "Picture Day", IsSchoolDay = true},
                new SpecialDay { Date = new DateTime(2020,10,12), Glyph="🦃", Description = "Thanksgiving"},
                new SpecialDay { Date = new DateTime(2020,11,11), Glyph="🎖", Description = "Remembrance Day"},
                new SpecialDay { Date = new DateTime(2020,11,12), Glyph="👩‍🏫", Description = "Professional Development Day"},
                new SpecialDay { Date = new DateTime(2020,11,13), Glyph="👩‍🏫", Description = "Professional Development Day"},
                new SpecialDay { Date = new DateTime(2020,11,23), Glyph="👫", Description = "Parent/Teacher", IsSchoolDay = true, IsEarlyDismissal = true},
                new SpecialDay { Date = new DateTime(2020,12,04), Glyph="🎶", Description = "Christmas Program (Location TBA)", IsSchoolDay = true},
                new SpecialDay { Date = new DateTime(2020,12,11), Glyph="🏫", Description = "Last Day Before Break", IsSchoolDay = true, IsEarlyDismissal = true},
                new SpecialDay { Date = new DateTime(2020,12,14), Glyph="🎄", EndDate = new DateTime(2021,1,3), Description = "Christmas Break"},
                new SpecialDay { Date = new DateTime(2021,1,4), Glyph="🏫", Description = "Classes Resume", IsSchoolDay = true},
                new SpecialDay { Date = new DateTime(2021,1,25), Glyph="🏫", EndDate = new DateTime(2021,1,27), Description = "High School Exams", IsSchoolDay = true},
                new SpecialDay { Date = new DateTime(2021,1,29), Glyph="👩‍🏫", Description = "Professional Development Day"},
                new SpecialDay { Date = new DateTime(2021,2,1), Glyph="🏫", Description = "First Semester Report Sent Home", IsSchoolDay = true},
                new SpecialDay { Date = new DateTime(2021,2,15), Glyph="📜", Description = "Heritage Day"},
                new SpecialDay { Date = new DateTime(2021,3,15), Glyph="🏠", EndDate = new DateTime(2021,3,19), Description = "March Break"},
                new SpecialDay { Date = new DateTime(2021,4,2), Glyph="✝️", Description = "Good Friday"},
                new SpecialDay { Date = new DateTime(2021,4,5), Glyph="✝️", Description = "Easter Monday"},
                new SpecialDay { Date = new DateTime(2021,4,16), Glyph="👩‍🏫", Description = "Professional Development Day"},
                new SpecialDay { Date = new DateTime(2021,4,20), Glyph="🏫", Description = "Mid-Semester Report Sent Home", IsSchoolDay = true},
                new SpecialDay { Date = new DateTime(2020,4,26), Glyph="👫", Description = "Parent/Teacher", IsSchoolDay = true, IsEarlyDismissal = true},
                new SpecialDay { Date = new DateTime(2021,5,24), Glyph="👑", Description = "Victoria Day"},
                new SpecialDay { Date = new DateTime(2021,6,9), Glyph="🏫", EndDate = new DateTime(2021,6,11), Description = "High School Exams", IsSchoolDay = true},
                new SpecialDay { Date = new DateTime(2021,6,11), Glyph="🏫", Description = "Last Day of School", IsSchoolDay = true, IsEarlyDismissal = true},
                new SpecialDay { Date = new DateTime(2021,6,14), Glyph="🏫", EndDate = new DateTime(2021,6,16), Description = "Reporting Days"},
                new SpecialDay { Date = new DateTime(2021,6,17), Glyph="🎓", Description = "Graduation (Location/Time TBA)"},
                new SpecialDay { Date = new DateTime(2021,6,18), Glyph="🏆", Description = "Awards Night (Location/Time TBA)"},
            };



            var settings = new SchoolYearConfiguration();
            settings.YearStart = new DateTime(2020, 9, 9);
            settings.YearEnd = new DateTime(2021, 6, 11);
            settings.SpecialDays = specialDays;
            settings.MealWeekMenuRotationSchedule = slots;
            settings.HotLunchMenu = items;
            settings.Id = Guid.NewGuid().ToString();
            await UpdateSchoolSettings(settings);
        }
    }
}
