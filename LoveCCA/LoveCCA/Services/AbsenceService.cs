using LoveCCA.Models;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        public async Task<List<AbsenceReport>> GetReports()
        {
            var query = await CrossCloudFirestore.Current
                        .Instance
                        .GetCollection("absencereports")
                        .OrderBy("Date", true)
                        .GetDocumentsAsync();

            var reports = query.ToObjects<AbsenceReport>().ToList();
            return reports;
        }

        public async Task DeleteReport(AbsenceReport report)
        {
            try
            {
                await CrossCloudFirestore.Current
                                         .Instance
                                         .GetCollection("absencereports")
                                         .GetDocument(report.Id)
                                         .DeleteDocumentAsync();
            }
            catch (Exception)
            {
                Debug.WriteLine("Error deleting order");
            }
        }
    }
}
