using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum.Application.Responses
{
    public class ApiResponse<T>
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public ApiResponse(int status, string message, T data)
        {
            Status = status;
            Message = message;
            Data = data;
        }

        public ApiResponse(int status, string message)
        {
            Status = status;
            Message = message;
        }
    }

    public class ApiResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }

        public ApiResponse(int status, string message)
        {
            Status = status;
            Message = message;
        }
    }
}
