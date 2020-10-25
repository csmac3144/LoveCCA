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
        private static List<string> _productClasses { get; set; }


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

        public static async Task UpdateProducts(List<Product> products)
        {
            try
            {
                var query = await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("products")
                         .GetDocumentsAsync();

                var documents = (query.ToObjects<Product>()).ToList();

                foreach (var doc in documents)
                {
                    await CrossCloudFirestore.Current
                             .Instance
                             .GetCollection("products")
                             .GetDocument(doc.Id)
                             .DeleteDocumentAsync();
                }

                foreach (var p in products)
                {
                    await CrossCloudFirestore.Current
                                 .Instance
                                 .GetCollection("products")
                                 .AddDocumentAsync(p);
                }


            }
            catch (Exception ex)
            {

            }
        }

        public static async Task<List<string>> GetProductClasses()
        {
            if (_productClasses == null)
                await LoadProducts();
            return _productClasses;
        }

        public static async Task<List<Product>> LoadProducts()
        {
            try
            {
                var query = await CrossCloudFirestore.Current
                            .Instance
                            .GetCollection("products")
                            .GetDocumentsAsync();

                var products = query.ToObjects<Product>().ToList();
                _productClasses = products.Select(p => p.Name).Distinct().ToList();
                return products;

            }
            catch (System.Exception)
            {

                System.Diagnostics.Debug.WriteLine("Error loading products");
                return null;
            }
        }
    }


}
