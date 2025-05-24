namespace E_commerce_Admin_Dashboard.Helpers
{
    public class ServiceResult<T>
    {
        public bool OK { get; set; }
        public string ErrorMessage { get; set; } = null;
        public T Data { get; set; }

        public static ServiceResult<T> Success(T data) =>
            new ServiceResult<T> { OK = true, Data = data };

        public static ServiceResult<T> Fail(string errorMessage) =>
            new ServiceResult<T> { OK = false, ErrorMessage = errorMessage };
    }
}
