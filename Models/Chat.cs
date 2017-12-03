using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Chat
    {
        public DateTime firstMessageDate { get; set; }
        public DateTime lastMessageDate { get; set; }
        public string otherUser { get; set; }
        public List<message> messages { get; set; }
    }

    public class singleChat
    {
        public string otherUser { get; set; }
        public message mesage { get; set; }
    }
}
