using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BusinessResponse<T> where T : class
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public T Data { get; set; }
    }

    public class BusinessResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
    }
}
