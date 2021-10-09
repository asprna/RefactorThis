namespace Application.Helper
{
	/// <summary>
	/// Global Result Class
	/// </summary>
	/// <typeparam name="T">The type of the result.</typeparam>
	public class Result<T>
	{
		public bool IsSuccess { get; set; }
		public T Value { get; set; }
		public string Error { get; set; }

		/// <summary>
		/// The Result Success
		/// </summary>
		/// <param name="value">The result</param>
		/// <returns></returns>
		public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Value = value };

		/// <summary>
		/// Raise an Error
		/// </summary>
		/// <param name="error">Error Message</param>
		/// <returns></returns>
		public static Result<T> Failure(string error) => new Result<T> { IsSuccess = false, Error = error };
	}
}
