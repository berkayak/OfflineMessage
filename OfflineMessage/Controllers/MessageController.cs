using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccess;
using Business;
using Models;

namespace OfflineMessage.Controllers
{
    public class MessageController : ApiController
    {

        [HttpPost]
        public ApiResponse sendMessage(string username, string otherUsername, string msg)
        {
            string token = Request.Headers.GetValues("Token").FirstOrDefault();

            var b = Business.Block.checkBlock(username, otherUsername);
            if (!b.isSuccess)
                return new ApiResponse() { isSuccess = false, message = b.message };
            else if (b.isSuccess && b.Data != null)
                return new ApiResponse() { isSuccess = true, message = b.message };

            var bResponse = Business.Message.sendMessage(username, otherUsername, token, msg);
            if (bResponse.isSuccess)
            {
                return new ApiResponse() { isSuccess = true, message = bResponse.message };
            }
            else
            {
                return new ApiResponse() { isSuccess = false, message = bResponse.message };
            }
        }

        [HttpGet]
        public ApiResponse<List<message>> getMessages(string username, string otherUsername = "")
        {
            string token = Request.Headers.GetValues("Token").FirstOrDefault();

            BusinessResponse<List<message>> bResponse;

            if (string.IsNullOrEmpty(otherUsername))
                bResponse = Business.Message.getAllMessages(username, token);
            else
                bResponse = Business.Message.getMessages(username, otherUsername, token);

            if (bResponse.isSuccess)
            {
                return new ApiResponse<List<message>>() { isSuccess = true, message = bResponse.message, Data = bResponse.Data };
            }
            else
            {
                return new ApiResponse<List<message>>() { isSuccess = false, message = bResponse.message, Data = null };
            }
        }

        [HttpGet]
        public ApiResponse<List<Chat>> getChats(string username, string otherUser = "")
        {
            string token = Request.Headers.GetValues("Token").FirstOrDefault();

            var bResponse = Business.Message.getAllMessages(username, token);
            if (bResponse.isSuccess && bResponse.Data != null)
            {
                List<Chat> chat = MessageListToChatList(bResponse.Data, username, otherUser);

                return new ApiResponse<List<Chat>>() { isSuccess = true, message = "Ok", Data = chat };
            }
            else
            {
                return null;
            }
            
        }

        private List<Chat> MessageListToChatList(List<message> data, string user, string otherUser = "")
        {
            var list = (from s in data
                        orderby s.date descending
                        select new singleChat
                        {
                            otherUser = (s.sender == user ? s.recipient : s.sender),
                            mesage = s,
                        }).ToList();

            List<Chat> chat = (from s in list 
                               where (!string.IsNullOrEmpty(otherUser) ? s.otherUser == otherUser : true)
                               group s by s.otherUser into grp
                               
                               select new Chat
                               {
                                   messages = grp.Select(x => x.mesage).ToList(),
                                   otherUser = grp.FirstOrDefault().otherUser,
                                   firstMessageDate = grp.LastOrDefault().mesage.date,
                                   lastMessageDate = grp.FirstOrDefault().mesage.date,
                               }).ToList();


            return chat;
        }

        [HttpPost]
        public ApiResponse BlockUSer(string blocker, string blocked)
        {
            string token = Request.Headers.GetValues("Token").FirstOrDefault();

            var t = Business.Block.addBlock(blocker, blocked, token);

            if(t.isSuccess == true)
            {
                return new ApiResponse() { isSuccess = true, message = t.message };
            }
            else
            {
                return new ApiResponse() { isSuccess = false, message = t.message };
            }
        }

        [HttpPost]
        public ApiResponse UnBlockUSer(string blocker, string blocked)
        {
            string token = Request.Headers.GetValues("Token").FirstOrDefault();

            var t = Business.Block.removeBlock(blocker, blocked, token);

            if (t.isSuccess == true)
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
