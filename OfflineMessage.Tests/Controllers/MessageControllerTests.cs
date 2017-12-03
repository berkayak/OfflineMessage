using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfflineMessage.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace OfflineMessage.Controllers.Tests
{
    [TestClass()]
    public class MessageControllerTests
    {
        [TestMethod()]
        public void getChatsTest()
        {
            var req = new HttpRequestMessage(HttpMethod.Post, "http://localhost:65033/");
            req.Headers.Add("Token", "833aa4fcb7db4dd8b907ae3c233b56604baa5ec070c0484fb121810fde03a88e");

            var httpActionContext = new HttpActionContext
            {
                ControllerContext = new HttpControllerContext
                {
                    Request = req
                }
            };

            var t = new MessageController();
            t.ActionContext = httpActionContext;
             var a = t.getChats("berkay");


        }
    }
}