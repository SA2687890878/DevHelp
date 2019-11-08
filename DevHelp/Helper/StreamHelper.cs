using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

/// <summary>
/// 流 辅助类
/// </summary>
public class StreamHelper
{
     /// <summary>
     /// 将流文件写入到文件中
     /// </summary>
     /// <param name="stream">流文件</param>
     /// <param name="fileName">文件路径+文件名</param>
     public bool StreamToFile(Stream stream,string fileName)
     {
         try
         {
             ///把steam转换成byte[]
             byte[] bytes = new byte[stream.Length];
             stream.Read(bytes, 0, bytes.Length);
             //设置当前流的位置为流的开始
             stream.Seek(0, SeekOrigin.Begin);

             //把byte[]写入文件
             FileStream fs = new FileStream(fileName, FileMode.Create);
             BinaryWriter bw = new BinaryWriter(fs);
             bw.Write(bytes);
             bw.Close();
             fs.Close();

             return true;
         }
         catch (Exception ex)
         {
             return false;
             throw ex;
         }
     }

    /// <summary>
    /// 从文件中读取stream
    /// </summary>
    /// <param name="fileName">文件路径+文件名</param>
    /// <returns></returns>
    public Stream FileToStream(string fileName)
     {
         //打开文件
         FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
         //读取文件的byte[]
         byte[] bytes = new byte[fileStream.Length];
         fileStream.Read(bytes, 0, bytes.Length);
         fileStream.Close();
         //把byte[]转换成Stream
         Stream stream = new MemoryStream(bytes);
         return stream;
     }
    /// <summary>
    /// 读取文件，将文件转成二进制数组
    /// </summary>
    /// <param name="fileName">文件路径+文件名</param>
    /// <returns></returns>
    public byte[] FileTobyte(string fileName)
    {
        //打开文件
        FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        //读取文件的byte[]
        byte[] bytes = new byte[fileStream.Length];
        fileStream.Read(bytes, 0, bytes.Length);
        fileStream.Close();
        return bytes;
    }
    /// <summary>
    /// base64string转换成byte[]
    /// </summary>
    /// <param name="base64string"></param>
    /// <returns></returns>
    public byte[] Base64ToBytes(string base64string)
    {
        if (!string.IsNullOrEmpty(base64string))
        {
            byte[] bytes = Convert.FromBase64String(base64string);
            return bytes;
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// Image 转成 base64
    /// </summary>
    /// <param name="fileFullName"></param>
    public string ImageToBase64(string fileFullName)
    {
        MemoryStream ms = new MemoryStream();
        try
        {
            Bitmap bmp = new Bitmap(fileFullName);
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] arr = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(arr, 0, (int)ms.Length);
            return Convert.ToBase64String(arr);
        }
        catch (Exception ex)
        {
            return "";
        }
        finally
        {
            ms.Close();
        }
    }
    /// <summary>
    /// Base64String转成图片文件
    /// </summary>
    /// <param name="str">Base64String格式的数据</param>
    /// <param name="fileName">文件路径+文件名</param>
    /// <returns></returns>
    public bool StringToImage(string str,string fileName)
    {
        try
        {
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(str)))
            {
                using (Image image = System.Drawing.Image.FromStream(ms))
                {
                    if (Directory.Exists(fileName))
                    {
                        Directory.Delete(fileName);
                    }
                    image.Save(fileName);
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
            throw ex;
        }
        
    }
}
