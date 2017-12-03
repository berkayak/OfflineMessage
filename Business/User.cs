using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using System.Security.Cryptography;
using Models;

namespace Business
{
    public static class User 
    {

        
        /// <summary>
        /// Tek parametre ile kullanıcı getirir, iki parametre kullanılırsa token doğrulanıp kullanıcı getirillir
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static DataAccess.User getUser(string username, string token = "")
        {
            using (OfflineMessageEntities db = new OfflineMessageEntities())
            {
                DataAccess.User user;
                if (string.IsNullOrEmpty(username))
                {
                    return null;
                }                    
                else
                {
                    user = (from s in db.User where s.username == username select s).FirstOrDefault();
                }

                if (user != null && !string.IsNullOrEmpty(token))
                {
                    bool isAny = (from s in db.UserToken where s.token == token && s.expireDate > DateTime.Now select s).Any();
                    if (isAny)
                        return user;
                }
                else if (user != null)
                    return user;

                return null;
            }
        }

        /// <summary>
        /// Giriş yaptıktan sonra token döner
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static BusinessResponse<DataAccess.UserToken> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return new BusinessResponse<UserToken>() { isSuccess = true, message = "Kullanıcı adı veya şifre yanlış!", Data = null };
            using (OfflineMessageEntities db = new OfflineMessageEntities())
            {
                DataAccess.User user = User.getUser(username);
                if (user == null)
                    return new BusinessResponse<UserToken>() { isSuccess = true, message = "Kullanıcı adı veya şifre yanlış!", Data = null };
                else if(user.password == CalculateSha1(password))
                {
                    try
                    {
                        List<UserToken> tokens = (from s in db.UserToken where s.userID == user.ID && s.expireDate > DateTime.Now orderby s.expireDate descending select s).ToList();
                        if (tokens.Count < 5)
                        {
                            UserToken token = new UserToken();
                            token.date = DateTime.Now;
                            token.userID = user.ID;
                            token.token = createToken();
                            token.expireDate = DateTime.Now.AddDays(14);
                            db.UserToken.Add(token);
                            db.SaveChanges();
                            Log.AddLog("Giriş Yapıldı, yeni token alındı.", user.ID, false);
                            return new BusinessResponse<UserToken>() { isSuccess = true, message = "OK", Data = token };
                        }
                        else
                        {
                            Log.AddLog("Giriş Yapıldı", user.ID, false);
                            return new BusinessResponse<UserToken>() { isSuccess = true, message = "OK", Data = tokens[0] };
                        }
                        
                    }
                    catch(Exception ex)
                    {
                        Log.AddLog(ex.Message, "Login");
                        return new BusinessResponse<UserToken>() { isSuccess = false, message = "Kritik Hata! " + DateTime.Now.ToString(), Data = null };
                    }                    
                }
                else
                {
                    Log.AddLog("Başarısız Giriş", user.ID, false);
                    return new BusinessResponse<UserToken>() { isSuccess = true, message = "Kullanıcı adı veya şifre yanlış!", Data = null };
                }
            }    
        }

        /// <summary>
        /// Üyellik gerçekleştirmek için kullanılır
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="confirmPassword"></param>
        public static BusinessResponse Register(string username, string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
                return new BusinessResponse() { isSuccess = false, message = "Geçersiz istek!" };
            else if (password != confirmPassword)
                return new BusinessResponse() { isSuccess = false, message = "Parolalar eşleşmiyor!" };
            using (OfflineMessageEntities db = new OfflineMessageEntities())
            {
                DataAccess.User user = (from s in db.User where s.username == username select s).FirstOrDefault();
                if (user != null)
                    return new BusinessResponse() { isSuccess = false, message = "Aynı isimli kullanıcı mevcut!" };
                else
                {
                    try
                    {
                        user = new DataAccess.User();
                        user.username = username;
                        user.password = CalculateSha1(password);
                        user.date = DateTime.Now;
                        user.status = 1;
                        db.User.Add(user);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Log.AddLog(ex.Message, "Register");
                        return new BusinessResponse() { isSuccess = false, message = "İstenmeyen bir hata oluştu! Sistem loglarını kontrol ediniz. " + DateTime.Now.ToString() };
                    }

                    Log.AddLog("Üyelik Gerçekleşti", user.ID, false);
                    return new BusinessResponse() { isSuccess = true, message = "Ok"};
                }
            }
        }

        public static string createToken()
        {
            string token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
            return token;
        }

        public static string CalculateSha1(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            SHA1CryptoServiceProvider cryptoTransformSha1 = new SHA1CryptoServiceProvider();
            string hash = BitConverter.ToString(cryptoTransformSha1.ComputeHash(buffer)).Replace("-", "");
            return hash;
        }

    }
}
