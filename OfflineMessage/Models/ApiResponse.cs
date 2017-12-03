using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfflineMessage.Models
{
    public class ApiResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        
    }

    public class ApiResponse<T> where T : class
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public T Data { get; set; }

    }
}