using System;

namespace PhoneVerification.Exceptions
{
    public class VerificationCodeException : PhoneVerificationException
    {
        public VerificationCodeException()
        {
        }

        public VerificationCodeException(string message)
            : base(message)
        {
        }

        public VerificationCodeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}