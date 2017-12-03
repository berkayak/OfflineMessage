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
    public class UserTests
    {
        [TestMethod()]
        public void createTokenTest()
        {
            //string key = User.createToken();
            //Trace.Write(key);

        }

        [TestMethod()]
        public void CalculateSha1()
        {
            //string sha1 = User.CalculateSha1("berkay");
            //Trace.Write(sha1);
        }

        [TestMethod()]
        public void RegisterTest()
        {
            //User.Register("berkay", "123123", "123123");
            //User.Register("berkay2", "123123", "123123");
            //User.Register("berkay3", "123123", "123123");
            
        }

        [TestMethod()]
        public void LoginTest()
        {
            //var token = User.Login("berkay", "123123");
            User.Login("berkay2", "123123");
            User.Login("berkay3", "123123");
        }
    }
}