using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    class Class1
    {
        OfflineMessageEntities db = new OfflineMessageEntities();


        public void test()
        {
            db.Dispose();
        }
    }
}
