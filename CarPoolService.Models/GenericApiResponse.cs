using System.Net;

namespace CarPoolService.Models
{
    public class GenericApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }

        public GenericApiResponse<T> CreateApiResponse(bool isSuccess, HttpStatusCode httpStatusCode, T data, string errorMessage = null)
        {
            return new GenericApiResponse<T>
            {
                IsSuccess = isSuccess,
                HttpStatusCode = httpStatusCode,
                ErrorMessage = errorMessage,
                Data = data
            };
        }
    }
}
