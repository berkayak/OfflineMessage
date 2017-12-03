using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfflineMessage.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OfflineMessage.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        [TestMethod()]
        public void LoginTest()
        {
            
            UserController controller = new UserController();

            controller.Login("berkay", "123123");
        }
    }
}