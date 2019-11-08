using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enyim.Caching;
using Enyim.Caching.Memcached;

namespace DevHelp
{
    /// <summary>
    /// memcached 缓存辅助类
    /// </summary>
    public class MemcachedHelper
    {
        /// <summary>
        /// 保存数据到缓存服务器中
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>true为保存成功,false为保存失败</returns>
        public static bool Save(string key,object value)
        {
            byte[] a = ObjectHelper.ObjectToBytes(value);
            MemcachedClient memcached = new MemcachedClient();
            return memcached.Store(StoreMode.Set, key, a); 
        }
        /// <summary>
        /// 从缓存服务器中读取存入的数据
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static object GetValue(string key)
        {
            MemcachedClient memcached = new MemcachedClient();
            byte[] a= (byte[])memcached.Get(key);
            return ObjectHelper.BytesToObject(a);
        }
    }
}
