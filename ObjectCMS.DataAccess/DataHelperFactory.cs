
using System.Collections.Generic;

namespace ObjectCMS.DataAccess
{
    public class DataHelperFactory
    {
        private static Dictionary<string, SqlHelper> DBPool = new Dictionary<string, SqlHelper>();
        private static object DBPool_lock = new object();

        private DataHelperFactory()
        { }

        public static SqlHelper Create(string dbName)
        {
            if (!DBPool.ContainsKey(dbName))
            {
                lock (DBPool_lock)
                {
                    if (!DBPool.ContainsKey(dbName))
                    {
                        DBPool.Add(dbName, new SqlHelper(dbName));
                    }
                }
            }

            return DBPool[dbName];
        }
        
    }
}
