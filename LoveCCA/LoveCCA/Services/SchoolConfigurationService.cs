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

            var items = new List<HotLunchMenu>
            {
                new HotLunchMenu { DayOfWeek = (int)DayOfWeek.Wednesday, MenuNumber = 1,
                    Options = new List<MenuOption>() {new MenuOption { Id = Guid.NewGuid().ToString(),
                        Description = "Lunchboxes - 1/2 Sub - Ham and cheese with mayo and lettuce, pkg of goldfish crackers, applesauce and juicebox.",
                        Price = 5 },
                        new MenuOption { Id = Guid.NewGuid().ToString(),
                        Description = "Lunchboxes - 1/2 Sub - Cold cut (ham and salami) with mayo and lettuce, pkg of goldfish crackers, applesauce and juicebox.",
                        Price = 5 },
                        new MenuOption { Id = Guid.NewGuid().ToString(),
                        Description = "Lunchboxes - 1/2 Sub - Sliced turkey with mayo and lettuce, pkg of goldfish crackers, applesauce and juicebox.",
                        Price = 5 },
                        new MenuOption { Id = Guid.NewGuid().ToString(),
                        Description = "Caesar Salad.",
                        Price = 3 }
                    }},
                new HotLunchMenu { DayOfWeek = (int)DayOfWeek.Wednesday, MenuNumber = 2,
                    Options = new List<MenuOption>() {new MenuOption { Id = Guid.NewGuid().ToString(),
                        Description = "Sub (approx. 9 inch) Ham and cheese with mayo and lettuce.",
                        Price = 4 },
                        new MenuOption { Id = Guid.NewGuid().ToString(),
                        Description = "Sub (approx. 9 inch) Cold cut (ham and salami) with mayo and lettuce.",
                        Price = 4 },
                        new MenuOption { Id = Guid.NewGuid().ToString(),
                        Description = "Sub (approx. 9 inch) Sliced turkey with mayo and lettuce.",
                        Price = 4 },
                        new MenuOption { Id = Guid.NewGuid().ToString(),
                        Description = "Caesar Salad.",
                        Price = 3 }
                    }},
                new HotLunchMenu { DayOfWeek = (int)DayOfWeek.Thursday, MenuNumber = 1,
                    Options = new List<MenuOption>() {new MenuOption { Id = Guid.NewGuid().ToString(),
                        Description = "Shepherd's Pie.",
                        Price = 4 },
                        new MenuOption { Id = Guid.NewGuid().ToString(),
                        Description = "Garden Salad.",
                        Price = 3 }
                    }},
                new HotLunchMenu { DayOfWeek = (int)DayOfWeek.Thursday, MenuNumber = 2,
                    Options = new List<MenuOption>() {new MenuOption { Id = Guid.NewGuid().ToString(),
                        Description = "Quesadillas - chicken and cheese",
                        Price = 4.5M },
                        new MenuOption { Id = Guid.NewGuid().ToString(),
                        Description = "Quesadillas - cheese",
                        Price = 4.5M },
                        new MenuOption { Id = Guid.NewGuid().ToString(),
                        Description = "Garden Salad.",
                        Price = 3 }
                    }},
                new HotLunchMenu { DayOfWeek = (int)DayOfWeek.Friday, MenuNumber = 1,
                    Options = new List<MenuOption>() {new MenuOption { Id = Guid.NewGuid().ToString(),
                        Description = "Pepperoni Pizza from Mario's Pizza.",
                        Price = 2.5M },
                        new MenuOption { Id = Guid.NewGuid().ToString(),
                        Description = "Cheese Pizza from Mario's Pizza.",
                        Price = 2.5M }
                    }},
                new HotLunchMenu { DayOfWeek = (int)DayOfWeek.Friday, MenuNumber = 2,
                    Options = new List<MenuOption>() {new MenuOption { Id = Guid.NewGuid().ToString(),
                        Description = "Chicken Shawarma wrap with lettuce and mayo, from Mezza Lebanese",
                        Price = 5M }
                    }},
            };

            var specialDays = new List<SpecialDay>
            {
                new SpecialDay { Date = new DateTime(2020,09,9), Description = "First Day of School", IsSchoolDay = true},
                new SpecialDay { Date = new DateTime(2020,09,16), Description = "Picture Day", IsSchoolDay = true},
                new SpecialDay { Date = new DateTime(2020,10,12), Description = "Thanksgiving"},
                new SpecialDay { Date = new DateTime(2020,11,11), Description = "Remembrance Day"},
                new SpecialDay { Date = new DateTime(2020,11,12), Description = "Professional Development Day"},
                new SpecialDay { Date = new DateTime(2020,11,13), Description = "Professional Development Day"},
                new SpecialDay { Date = new DateTime(2020,11,23), Description = "Parent/Teacher (Early Dismissal)", IsSchoolDay = true, IsEarlyDismissal = true},
                new SpecialDay { Date = new DateTime(2020,12,04), Description = "Christmas Program (Location TBA)", IsSchoolDay = true},
                new SpecialDay { Date = new DateTime(2020,12,11), Description = "Last Day Before Break (Early Dismissal)", IsSchoolDay = true, IsEarlyDismissal = true},
                new SpecialDay { Date = new DateTime(2020,12,14), EndDate = new DateTime(2021,1,3), Description = "Christmas Break"},
                new SpecialDay { Date = new DateTime(2021,1,4), Description = "Classes Resume", IsSchoolDay = true},
                new SpecialDay { Date = new DateTime(2021,1,25), EndDate = new DateTime(2021,1,27), Description = "High School Exams", IsSchoolDay = true},
                new SpecialDay { Date = new DateTime(2021,1,29), Description = "Professional Development Day"},
                new SpecialDay { Date = new DateTime(2021,2,1), Description = "First Semester Report Sent Home", IsSchoolDay = true},
                new SpecialDay { Date = new DateTime(2021,2,15), Description = "Heritage Day"},
                new SpecialDay { Date = new DateTime(2021,3,15), EndDate = new DateTime(2021,3,19), Description = "March Break"},
                new SpecialDay { Date = new DateTime(2021,4,2), Description = "Good Friday"},
                new SpecialDay { Date = new DateTime(2021,4,5), Description = "Easter Monday"},
                new SpecialDay { Date = new DateTime(2021,4,16), Description = "Professional Development Day"},
                new SpecialDay { Date = new DateTime(2021,4,20), Description = "Mid-Semester Report Sent Home", IsSchoolDay = true},
                new SpecialDay { Date = new DateTime(2020,4,26), Description = "Parent/Teacher (Early Dismissal)", IsSchoolDay = true, IsEarlyDismissal = true},
                new SpecialDay { Date = new DateTime(2021,5,24), Description = "Victoria Day"},
                new SpecialDay { Date = new DateTime(2021,6,9), EndDate = new DateTime(2021,6,11), Description = "High School Exams", IsSchoolDay = true},
                new SpecialDay { Date = new DateTime(2021,6,11), Description = "Last Day of School (Early Dismissal)", IsSchoolDay = true, IsEarlyDismissal = true},
                new SpecialDay { Date = new DateTime(2021,6,14), EndDate = new DateTime(2021,6,16), Description = "Reporting Days"},
                new SpecialDay { Date = new DateTime(2021,6,17), Description = "Graduation (Location/Time TBA)"},
                new SpecialDay { Date = new DateTime(2021,6,18), Description = "Awards Night (Location/Time TBA)"},
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
