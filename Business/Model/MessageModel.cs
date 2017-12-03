using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class MessageModel
    {
        public DateTime firstMessageDate { get; set; }
        public DateTime lastMessageDate { get; set; }
        public string otherUser { get; set; }
        public List<message> MyProperty { get; set; }
    }

    public class message
    {
        public string sender { get; set; }
        public string recipient { get; set; }
        public DateTime date { get; set; }
        public string text { get; set; }
        public bool isRead { get; set; }
        public DateTime? readDate { get; set; }
    }
}
