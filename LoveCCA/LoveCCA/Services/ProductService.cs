using LoveCCA.Models;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveCCA.Services
{
    class ProductService
    {
        public static async Task<List<Product>> LoadProducts(string name)
        {
            try
            {
                var query = await CrossCloudFirestore.Current
                            .Instance
                            .GetCollection("products")
                            .WhereEqualsTo("Name", name)
                            .GetDocumentsAsync();

                return query.ToObjects<Product>().ToList();
            }
            catch (System.Exception)
            {

                System.Diagnostics.Debug.WriteLine("Error loading products");
                return null;
            }
        }

        public static async Task<List<Product>> LoadProducts()
        {
            try
            {
                var query = await CrossCloudFirestore.Current
                            .Instance
                            .GetCollection("products")
                            .GetDocumentsAsync();

                return query.ToObjects<Product>().ToList();
            }
            catch (System.Exception)
            {

                System.Diagnostics.Debug.WriteLine("Error loading products");
                return null;
            }
        }
    }


}
