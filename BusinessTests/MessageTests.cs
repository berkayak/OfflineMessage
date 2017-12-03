using Microsoft.VisualStudio.TestTools.UnitTesting;
using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}