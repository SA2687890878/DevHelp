using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DevHelp
{
    /// <summary>
    /// 对象转换帮助类
    /// </summary>
    public class ObjectHelper
    {
        /// <summary>
        /// 对象转换成byte数组
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ObjectToBytes(object obj)
        {
            byte[] a;
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                a = ms.GetBuffer();
            }
            return a;
        }
        /// <summary>
        /// byte数组转换成对象
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static object BytesToObject(byte[] buff)
        {
            object obj;
            using (MemoryStream ms = new MemoryStream(buff))
            {
                IFormatter formatter = new BinaryFormatter();
                obj = formatter.Deserialize(ms);
            }
            return obj;
        }
    }
}
