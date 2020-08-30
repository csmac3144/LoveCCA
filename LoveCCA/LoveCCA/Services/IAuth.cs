using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoveCCA.Services
{
    public interface IAuth
    {
        Task<string> LoginWithEmailPassword(string email, string password);
        Task<string> CreateUserWithEmailPassword(string email, string password);
    }
}
