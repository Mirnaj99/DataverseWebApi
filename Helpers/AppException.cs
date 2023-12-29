namespace DataverseWebApis.Helpers
{
    public class AppException : Exception
    {
        // Custom error code to identify the type of exception
        public string ErrorCode { get; }

        // Additional context information about the exception
        public object AdditionalInfo { get; }

        // Default constructor
        public AppException()
        {
        }

        // Constructor with a custom error message and code
        public AppException(string message, string errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        // Constructor with a custom error message, code, and inner exception
        public AppException(string message, string errorCode, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        // Constructor with a custom error message, code, inner exception, and additional information
        public AppException(string message, string errorCode, Exception innerException, object additionalInfo)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
            AdditionalInfo = additionalInfo;
        }
    }
}
