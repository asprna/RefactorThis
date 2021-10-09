namespace Application.Helper
{
    /// <summary>
    /// Global Application Exception Class.
    /// </summary>
	public class AppException
	{
        public AppException(int statusCode, string message, string details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
}
