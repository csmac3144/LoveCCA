using LoveCCA.Models;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoveCCA.Services
{
    public class AbsenceService
    {
        public async Task SubmitReport(AbsenceReport report)
        {
            report.Id = Guid.NewGuid().ToString();
            await CrossCloudFirestore.Current
                        .Instance
                        .GetCollection("absencereports")
                        .GetDocument(report.Id)
                        .SetDataAsync(report);
        }
    }
}
