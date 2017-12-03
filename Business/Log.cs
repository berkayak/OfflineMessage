using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    static class Log
    {
        /// <summary>
        /// System Log Ekleme
        /// </summary>
        /// <param name="exceptionMessage"></param>
        /// <param name="methodName"></param>
        public static void AddLog(string exceptionMessage, string methodName)
        {
            using (OfflineMessageEntities db = new OfflineMessageEntities())
            {
                DataAccess.SystemLog sLog = new SystemLog();
                sLog.date = DateTime.Now;
                sLog.logMessage = "Method: " + methodName + " Hata Mesajı: " + exceptionMessage;
                db.SystemLog.Add(sLog);
                db.SaveChanges();
            }            
        }

        /// <summary>
        /// Kullanıcı Log Ekleme
        /// </summary>
        /// <param name="logMessage"></param>
        /// <param name="userID"></param>
        /// <param name="isError"></param>
        public static void AddLog(string logMessage, int userID, bool isError)
        {
            using (OfflineMessageEntities db = new OfflineMessageEntities())
            {
                DataAccess.UserLog uLog = new UserLog();
                uLog.date = DateTime.Now;
                uLog.logMessage = logMessage;
                uLog.isError = isError;
                uLog.userID = userID;
                db.UserLog.Add(uLog);
                db.SaveChanges();
            }
        }
    }
}
