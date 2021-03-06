﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccess;
using Models;

namespace OfflineMessage.Controllers
{
    public class UserController : ApiController
    {
        /// <summary>
        /// Login işlemi yapar, Giriş başarılı ise token döner
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse<UserToken> Login(string username, string password)
        {
            var t = Business.User.Login(username, password);
            if(t.isSuccess && t.Data != null)
            {
                return new ApiResponse<UserToken>() { isSuccess = true, message = t.message, Data = t.Data };
            }
            else
            {
                return new ApiResponse<UserToken>() { isSuccess = false, message = t.message, Data = null };
            }
        }

        /// <summary>
        /// Register işlemi yapar
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="confirmPassword"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse Register(string username, string password, string confirmPassword)
        {
            var t = Business.User.Register(username, password, confirmPassword);
            if (t.isSuccess)
            {
                return new ApiResponse() { isSuccess = true, message = t.message };
            }
            else
            {
                return new ApiResponse() { isSuccess = false, message = t.message };
            }
        }
    }
}
