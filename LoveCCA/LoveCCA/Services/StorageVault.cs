using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LoveCCA.Services
{
    class StorageVault
    {
        public static async Task<Tuple<string,string>> GetCredentials()
        {
            try
            {
                string email = await GetValue("email");
                string password = await GetValue("password");
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                    throw new Exception();
                return new Tuple<string, string>(email, password);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task SetCredentials(string email, string password)
        {
            try
            {
                await SetValue("email", email);
                await SetValue("password", password);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task SetValue(string key, string value)
        {
            try
            {
                await SecureStorage.SetAsync(key, value);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task SetToken(string token)
        {
            try
            {
                await SecureStorage.SetAsync("token", token);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void ClearCredentials()
        {
            try
            {
                SecureStorage.Remove("email");
                SecureStorage.Remove("password");
                SecureStorage.Remove("token");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task GetToken()
        {
            try
            {
                await SecureStorage.GetAsync("token");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<string> GetValue(string key)
        {
            try
            {
                return await SecureStorage.GetAsync(key);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
