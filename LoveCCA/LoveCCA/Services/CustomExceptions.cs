using System;
using System.Collections.Generic;
using System.Text;

namespace LoveCCA.Services
{
    public class WeakPasswordException : Exception
    {
        public WeakPasswordException() 
        {
        }
    }
    public class InvalidLoginException : Exception
    {
        public InvalidLoginException() 
        {
        }
    }
    public class SignUpErrorException : Exception
    {
        public SignUpErrorException() 
        {
        }
    }
    public class BadEmailFormatException : Exception
    {
        public BadEmailFormatException() 
        {
        }
    }
    public class EmailInUseException : Exception
    {
        public EmailInUseException() 
        {
        }
    }
}
