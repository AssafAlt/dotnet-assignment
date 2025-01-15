namespace ColorsManagement.Common
{
    public class ServiceResponse<T>
    {
        public int StatusCode { get; set; }
        public T Data { get; set; }

        public ServiceResponse(int statusCode, T data)
        {
            StatusCode = statusCode;
            Data = data;
        }
    }
}
