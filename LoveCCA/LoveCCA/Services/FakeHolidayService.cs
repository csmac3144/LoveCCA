using System.Threading.Tasks;

namespace LoveCCA.Services
{

    public class FakeHolidayService : HolidayService, IHolidayService
    {
        public override async Task<SchoolYearSettings> LoadSchoolSettings()
        {
            await Task.Yield();
            return GetDefault();
        }
    }
}
