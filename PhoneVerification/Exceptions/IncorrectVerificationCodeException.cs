using System;

namespace PhoneVerification.Exceptions
{
    public class IncorrectVerificationCodeException : VerificationCodeException
    {
        public IncorrectVerificationCodeException()
        {
        }

        public IncorrectVerificationCodeException(string message)
            : base(message)
        {
        }

        public IncorrectVerificationCodeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}