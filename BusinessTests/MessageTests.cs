using Microsoft.VisualStudio.TestTools.UnitTesting;
using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Business.Tests
{
    [TestClass()]
    public class MessageTests
    {
        [TestMethod()]
        public void sendMessageTest()
        {
            Message.sendMessage("berkay", "berkay2", "833aa4fcb7db4dd8b907ae3c233b56604baa5ec070c0484fb121810fde03a88e", "Merhaba Dünya");
        }

        [TestMethod()]
        public void getAllMessagesTest()
        {
            var a = Message.getAllMessages("berkay", "12d6b663fe7c454fb6719dc11b44f6c78223cdf972ff47d992e8a7cdd0e861a0");


            foreach (var item in a.Data)
            {
                Trace.WriteLine(item.text);
            }
            Trace.WriteLine(a.Data.Count.ToString());
        }

        [TestMethod()]
        public void getMessagesTest()
        {
            var a = Message.getMessages("berkay", "berkay4", "12d6b663fe7c454fb6719dc11b44f6c78223cdf972ff47d992e8a7cdd0e861a0");

            foreach (var item in a.Data)
            {
                Trace.WriteLine(item.text);
            }
            Trace.WriteLine(a.Data.Count.ToString());
        }
    }
}