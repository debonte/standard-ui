using System;

namespace StandardUI.CodeGenerator
{
    public class UserViewableException : Exception
    {
        public UserViewableException(string message) : base(message)
        {
        }
    }
}