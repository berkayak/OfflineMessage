using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Models;

namespace Business
{
    public static class Block
    {

        /// <summary>
        /// Kullanıcı blocklama işlemi yapar
        /// </summary>
        /// <param name="username"></param>
        /// <param name="otherUsername"></param>
        /// <param name="token"></param>
        public static BusinessResponse addBlock(string username, string otherUsername, string token)
        {
            DataAccess.User user = User.getUser(username, token);
            DataAccess.User otherUser = User.getUser(otherUsername);
            if (user == null || user == null)
                return new BusinessResponse() { isSuccess = false, message = "Kullanıcı bulunamadı!" };
            using (OfflineMessageEntities db = new OfflineMessageEntities())
            {
                bool isAny = (from s in db.Block where s.blockerID == user.ID && s.blockedID == otherUser.ID select s).Any();
                if (isAny)
                    return new BusinessResponse() { isSuccess = false, message = "Bloklama işlemi daha önce yapılmış" };
                else
                {
                    try
                    {
                        DataAccess.Block block = new DataAccess.Block();
                        block.blockerID = user.ID;
                        block.blockedID = otherUser.ID;
                        block.date = DateTime.Now;
                        db.Block.Add(block);
                        db.SaveChanges();
                        Log.AddLog("Bloklandı", user.ID, false);
                        return new BusinessResponse() { isSuccess = true, message = "Ok" };
                    }
                    catch (Exception ex)
                    {
                        Log.AddLog(ex.Message, "addBlock");
                        return new BusinessResponse() { isSuccess = false, message = "Kritik Hata" };
                    } 
                }
            }

        }

        /// <summary>
        /// Var olan bir bloklama kaldırılır
        /// </summary>
        /// <param name="username"></param>
        /// <param name="otherUsername"></param>
        /// <param name="token"></param>
        public static BusinessResponse removeBlock(string username, string otherUsername, string token)
        {
            DataAccess.User user = User.getUser(username, token);
            DataAccess.User otherUser = User.getUser(otherUsername);
            if (user == null || user == null)
                return new BusinessResponse() { isSuccess = false, message = "Kullanııcı Bulunamadı" };
            using (OfflineMessageEntities db = new OfflineMessageEntities())
            {
                try
                {
                    List<DataAccess.Block> blocks = (from s in db.Block where s.blockerID == user.ID && s.blockedID == otherUser.ID select s).ToList();
                    if (blocks.Count > 0)
                    {
                        db.Block.RemoveRange(blocks);
                        db.SaveChanges();
                        Log.AddLog("Block Kaldırıldı", user.ID, false);
                        return new BusinessResponse() { isSuccess = true, message = "Ok" };
                    }
                    else
                    {
                        Log.AddLog("Block Kaldırılamadı: Bloklama yapılmamış", user.ID, false);
                        return new BusinessResponse() { isSuccess = false, message = "Kaldırılacak bir blok bulunamadı!" };
                    }                    

                }
                catch (Exception ex)
                {
                    Log.AddLog(ex.Message, "removeBlock");
                    return new BusinessResponse() { isSuccess = false, message = "Kritik Hata " + DateTime.Now.ToString() };
                }                
            }
        }
        
        /// <summary>
        /// İki kullanıcı arasında blok olup olmadığını kontrol eder
        /// </summary>
        /// <param name="username"></param>
        /// <param name="otherUsername"></param>
        /// <returns></returns>
        public static BusinessResponse<DataAccess.Block> checkBlock(string username, string otherUsername)
        {
            DataAccess.User user = User.getUser(username);
            DataAccess.User otherUser = User.getUser(otherUsername);
            if (user == null || otherUser == null)
                return new BusinessResponse<DataAccess.Block>() { isSuccess = false, message = "Kullanıcı Bulunamadı", Data = null };

            using (OfflineMessageEntities db = new OfflineMessageEntities())
            {
                List<DataAccess.Block> blocks;

                try
                {
                    blocks = (from s in db.Block where (s.blockerID == user.ID && s.blockedID == otherUser.ID) || (s.blockerID == otherUser.ID && s.blockedID == user.ID) select s).ToList();
                    if (blocks.Count > 0)
                        return new BusinessResponse<DataAccess.Block>() { isSuccess = true, message = "Bloklandı!", Data = blocks[0] }; //Bloklanmış
                    else
                        return new BusinessResponse<DataAccess.Block>() { isSuccess = true, message = "Ok", Data = null }; //Bloklama yok
                }
                catch (Exception ex)
                {
                    Log.AddLog(ex.Message, "checkBlock");
                    return new BusinessResponse<DataAccess.Block>() { isSuccess = false, message = "Kritik Hata", Data = null };
                }
            }    

        }
    }
}
