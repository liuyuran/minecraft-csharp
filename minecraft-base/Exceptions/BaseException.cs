using System;

namespace Base.Exceptions {
    public class BaseException: Exception {
        protected BaseException(string message) : base(message) { }
        
        public BaseException(string message, Exception innerException) : base(message, innerException) { }
    }
}