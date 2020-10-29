using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Schema;

namespace ah_2class
{
    /// <summary>
    /// 网络连接类
    /// </summary>
    public class HttpHelper
    {

        public CookieContainer cookie = new CookieContainer();
        public Encoding Encode { get; set; }

        public HttpHelper()
        {
            this.Encode = Encoding.GetEncoding("UTF-8");
        }

        /// <summary>
        /// 提供编码下载页面
        /// </summary>
        /// <param name="enc"></param>
        /// <returns></returns>
        public string DownloadHtml(string Url)
        {
            if (Url.Length == 0)
            {

                return null;
            }
            var html = string.Empty;
            HttpWebRequest request = HttpWebRequest.Create(Url) as HttpWebRequest;
            request.Timeout = 30 * 1000;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko)";
            request.ContentType = "text/html; charset=gb2312";
            request.CookieContainer = cookie;
            //Encoding enc = Encoding.GetEncoding("GBK");
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine(string.Format("抓取{0}网址失败!", Url));
                }
                else
                {
                    try
                    {
                        StreamReader sr = new StreamReader(response.GetResponseStream(), Encode);
                        html = sr.ReadToEnd();
                        sr.Close();
                    }
                    catch (Exception er)
                    {

                        Console.WriteLine(string.Format("抓取{0}网址失败!\nerror:{1}", Url, er.Message));
                        html = null;
                    }
                }
            }
            
            return html;


        }

        /// <summary>
        /// 发送带参数的POST请求
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public string HttpPost(string Url, string postDataStr,string[] headers)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8";
            if (headers!=null)
            {
                foreach (string header in headers)
                {
                    request.Headers.Add(header);
                }
            }
            
            request.UserAgent="User-Agent:Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4287.0 Safari/537.36 Edg/88.0.673.0";
            byte[] postData = this.Encode.GetBytes(postDataStr);
            request.ContentLength = postData.Length;
            request.CookieContainer = this.cookie;
            Stream myRequestStream = request.GetRequestStream();
            myRequestStream.Write(postData, 0, postData.Length);
            myRequestStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encode);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            response.Close();
            return retString;
        }

        /// <summary>
        /// 发送带参数的GET请求
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html; charset=gb2312";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.Headers["Accept-Language"] = "zh-CN";
            request.CookieContainer = this.cookie;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encode);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            response.Close();
            return retString;
        }


        /// <summary>
        /// 获取页面重定向url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="referer"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public  string GetRedirectUrl(string url, string referer = "", string cookie = "")
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "HEAD";
                req.Referer = referer;
                req.AllowAutoRedirect = false;
                if (cookie.Length > 0)
                {
                    req.Headers.Add("Cookie:" + cookie);
                }
                WebResponse response = req.GetResponse();
                string ret = response.Headers["Location"];
                response.Close();
                return ret;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}

