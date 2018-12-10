using System;

namespace PhoneVerification.Exceptions
{
    public class NoPendingVerificationException : VerificationCodeException
    {
        public NoPendingVerificationException()
        {
        }

        public NoPendingVerificationException(string message)
            : base(message)
        {
        }

        public NoPendingVerificationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}