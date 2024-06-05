namespace ObjectPath
{
    public class InvalidObjectPathException : Exception
    {
        public InvalidObjectPathException() : base("Invalid object path.")
        {
        }

        public InvalidObjectPathException(string message) : base(message)
        {
        }

        public InvalidObjectPathException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}