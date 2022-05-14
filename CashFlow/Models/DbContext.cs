using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace CashFlow.Models
{
    //singlton access to database
    public static class DbContext
    {
        static LiteDatabase _instance;

        public static void Init(string fileName)
        {
            _instance = new LiteDatabase(fileName);
        }

        public static LiteDatabase GetInstance()
        {
            return _instance;
        }
    }
}
