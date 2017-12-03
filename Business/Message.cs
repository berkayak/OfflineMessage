using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Models;
using System.Globalization;

namespace Business
{
    public static class Message
    {
        /// <summary>
        /// Bir kişi olan mesajları getirir
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="otherUserName"></param>
        /// <param name="token"></param>
        public static BusinessResponse<List<message>> getMessages(string userName, string otherUserName, string token)
        {
            DataAccess.User user = User.getUser(userName, token);
            DataAccess.User otherUser = User.getUser(otherUserName);
            List<message> messages;
            if (user == null || otherUser == null)
                return new BusinessResponse<List<message>>() { isSuccess = true, message = "Kullanıcı adı yanlış girildi", Data = null };
            else
            {
                using (OfflineMessageEntities db = new OfflineMessageEntities())
                {
                    try
                    {
                        CultureInfo tr = new CultureInfo("tr-TR");
                        messages = (from s in db.Message.ToList()
                                    where (s.senderID == user.ID && s.recipientID == otherUser.ID) ||
                                    (s.recipientID == user.ID && s.senderID == otherUser.ID)
                                    orderby s.date descending
                                    select new message
                                    {
                                        date = s.date.ToString("G", tr),
                                        text = s.messageContent,
                                        sender = (from p in db.User where p.ID == s.senderID select p.username).FirstOrDefault(),
                                        recipient = (from p in db.User where p.ID == s.recipientID select p.username).FirstOrDefault(),
                                        isRead = false,
                                    }).ToList();



                        Log.AddLog("Mesajlar Listelendi", user.ID, false);
                        return new BusinessResponse<List<message>>() { isSuccess = true, message = "Ok", Data = messages };
                    }
                    catch (Exception ex)
                    {
                        Log.AddLog(ex.Message, "getMessages");
                        return new BusinessResponse<List<message>>() { isSuccess = false, message = "Kritik Hata", Data = null };
                    }
                }
            }
        }

        /// <summary>
        /// Mesaj göndermek için kullanılan method
        /// </summary>
        /// <param name="senderUser"></param>
        /// <param name="recipientUser"></param>
        /// <param name="token"></param>
        /// <param name="message"></param>
        public static BusinessResponse sendMessage(string senderUser, string recipientUser, string token, string message)
        {
            DataAccess.User sender = User.getUser(senderUser, token);
            DataAccess.User recipient = User.getUser(recipientUser);

            if (sender != null && recipient != null)
            {
                using (OfflineMessageEntities db = new OfflineMessageEntities())
                {
                    try
                    {
                        DataAccess.Message msg = new DataAccess.Message();
                        msg.senderID = sender.ID;
                        msg.recipientID = recipient.ID;
                        msg.messageContent = message;
                        msg.date = DateTime.Now;
                        msg.isRead = false;
                        db.Message.Add(msg);
                        db.SaveChanges();
                        Log.AddLog("Mesaj Kaydedildi", sender.ID, false);
                        return new BusinessResponse() { isSuccess = true, message = "Ok" };
                    }
                    catch (Exception ex)
                    {
                        Log.AddLog(ex.Message, "sendMessage");
                        return new BusinessResponse() { isSuccess = false, message = "Kritik Hata" };
                    }
                }

            }
            else
                return new BusinessResponse() { isSuccess = true, message = "Geçersiz kullanıcı bilgisi!" };
        }

        /// <summary>
        /// Kullanıcının gönderdiği ve aldığı tüm mesajları listeler
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static BusinessResponse<List<message>> getAllMessages(string userName, string token)
        {
            List<message> message;
            DataAccess.User user = User.getUser(userName, token);
            if (user == null)
                return new BusinessResponse<List<message>>() { isSuccess = true, message = "Geçersiz kullanııcı", Data = null };

            using (OfflineMessageEntities db = new OfflineMessageEntities())
            {
                try
                {
                    CultureInfo tr = new CultureInfo("tr-TR");
                    message = (from s in db.Message.ToList()
                               where s.senderID == user.ID || s.recipientID == user.ID
                               orderby s.date descending
                               select new message
                               {
                                   date = s.date.ToString("G", tr),
                                   text = s.messageContent,
                                   sender = (from p in db.User where p.ID == s.senderID select p.username).FirstOrDefault(),
                                   recipient = (from p in db.User where p.ID == s.recipientID select p.username).FirstOrDefault(),
                                   isRead = false,
                               }).ToList();
                    Log.AddLog("Tüm Mesajlar Listelendi", user.ID, false);
                    return new BusinessResponse<List<message>>() { isSuccess = true, message = "Ok", Data = message };
                }
                catch (Exception ex)
                {
                    Log.AddLog(ex.Message, "getAllMessages");
                    return new BusinessResponse<List<message>>() { isSuccess = false, message = "Kritik Hata", Data = null };
                }

            }
        }

    }
}
