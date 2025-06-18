namespace E_commerce_Admin_Dashboard.Helpers
{
    public class ServiceResult<T>
    {
        public bool OK { get; set; }
        public string ErrorMessage { get; set; } = null;
        public int StatusCode { get; set; }
        public T Data { get; set; }

        public static ServiceResult<T> Success(T data, int statusCode) =>
            new ServiceResult<T> { OK = true, Data = data, StatusCode = statusCode };

        public static ServiceResult<T> Fail(string errorMessage, int statusCode) =>
            new ServiceResult<T> { OK = false, ErrorMessage = errorMessage, StatusCode = statusCode };
    }
}
