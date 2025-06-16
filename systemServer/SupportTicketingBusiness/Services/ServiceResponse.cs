using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportTicketingBusiness.Services
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static ServiceResponse<T> SuccessResponse(T data, string message = "")
        {
            return new ServiceResponse<T> { Success = true, Message = message, Data = data };
        }

        public static ServiceResponse<T> FailureResponse(string message)
        {
            return new ServiceResponse<T> { Success = false, Message = message };
        }
    }

}
