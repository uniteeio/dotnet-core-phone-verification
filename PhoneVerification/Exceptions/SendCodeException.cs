using System;

namespace PhoneVerification.Exceptions
{
    public class SendCodeException : PhoneVerificationException
    {
        public SendCodeException()
        {
        }

        public SendCodeException(string message)
            : base(message)
        {
        }

        public SendCodeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}