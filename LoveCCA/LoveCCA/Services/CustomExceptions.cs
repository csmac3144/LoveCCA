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
    public class SendPasswordResetLinkException : Exception
    {
        public SendPasswordResetLinkException() 
        {
        }
    }
    public class UpdatePasswordException : Exception
    {
        public UpdatePasswordException() 
        {
        }
    }
    public class SendAccountVerificationLinkException : Exception
    {
        public SendAccountVerificationLinkException() 
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
