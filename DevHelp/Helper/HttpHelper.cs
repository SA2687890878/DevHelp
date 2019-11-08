using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text;
using System.Web;

namespace DevHelp
{
    /// <summary>
    /// http请求辅助类
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// http请求的get方法辅助
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        public static string GetHttpResponse(string url, int Timeout)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = Timeout;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string retString = GetResponseString(response); //将响应信息转成string

            return retString;
        }
        /// <summary>
        /// http(https)请求的post方法辅助
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">参数集合</param>
        /// <param name="timeout">超时时间</param>
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>
        /// <returns></returns>
        public static string CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int timeout, string userAgent, CookieCollection cookies)
        {
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            //设置代理UserAgent和超时
            //request.UserAgent = userAgent;
            //request.Timeout = timeout; 

            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //发送POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        i++;
                    }
                }
                byte[] data = Encoding.ASCII.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            string[] values = request.Headers.GetValues("Content-Type");
            string responseString = GetResponseString(request.GetResponse() as HttpWebResponse);
            return responseString;
        }
        /// <summary>
        /// http(https)请求的post方法辅助
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求数据</param>
        /// <returns></returns>
        public static string CreatePostHttpResponse(string url,string data)
        {
            HttpWebRequest hwr = WebRequest.Create(url) as HttpWebRequest;
            hwr.Method = "POST";
            hwr.ContentType = "application/x-www-form-urlencoded";
            byte[] payload;
            payload = System.Text.Encoding.UTF8.GetBytes(data);
            hwr.ContentLength = payload.Length;
            Stream writer = hwr.GetRequestStream();
            writer.Write(payload, 0, payload.Length);
            writer.Close();
            string responseStr = GetResponseString(hwr.GetResponse() as HttpWebResponse);
            return responseStr;
        }
        /// <summary>
        /// 获取响应的数据
        /// </summary>
        public static string GetResponseString(HttpWebResponse webresponse)
        {
            using (Stream s = webresponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                return reader.ReadToEnd();

            }
        }
        /// <summary>
        /// 获取请求的数据
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns></returns>
        public static string GetRequstString(HttpRequest request)
        {
            using (Stream stream = request.InputStream)
            {
                 Byte[] postBytes = new Byte[stream.Length];
                 stream.Read(postBytes, 0, (Int32)stream.Length);
                 string postString = Encoding.UTF8.GetString(postBytes);
                 return postString;
            }
        }

    }
}
