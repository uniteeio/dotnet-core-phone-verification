using System;

namespace PhoneVerification.Exceptions
{
    public class PhoneVerificationException : Exception
    {
        public PhoneVerificationException()
        {
        }

        public PhoneVerificationException(string message)
            : base(message)
        {
        }

        public PhoneVerificationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}