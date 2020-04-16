using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ModuleLauncherRebuild.Tools
{
    public static class NetTools
    {
        public static string PostHttp(Uri uri, string jsonParam, string encode = "utf-8",string requestMehod = "POST")
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
                request.Method = requestMehod;
                request.ContentType = "application/json;charset=" + encode.ToUpper();

                byte[] payload;

                payload = Encoding.GetEncoding(encode.ToUpper()).GetBytes(jsonParam);
                request.ContentLength = payload.Length;

                Stream writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;


                Stream s = response.GetResponseStream();
                string StrDate = "";
                string strValue = "";
                StreamReader Reader = new StreamReader(s, Encoding.GetEncoding(encode.ToUpper()));
                while ((StrDate = Reader.ReadLine()) != null)
                {
                    strValue += StrDate + "\r\n";
                }
                return strValue;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static async Task<string> PostHttpAsync(Uri uri, string jsonParam, string encode = "utf-8", string requestMehod = "POST")
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
                request.Method = requestMehod;
                request.ContentType = "application/json;charset=" + encode.ToUpper();

                byte[] payload;

                payload = Encoding.GetEncoding(encode.ToUpper()).GetBytes(jsonParam);
                request.ContentLength = payload.Length;

                Stream writer = await request.GetRequestStreamAsync();
                writer.Write(payload, 0, payload.Length);
                writer.Close();

                HttpWebResponse response = (await request.GetResponseAsync()) as HttpWebResponse;

                StreamReader Reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encode.ToUpper()));
                return await Reader.ReadToEndAsync(); ;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static string GetHttp(Uri uri, String Method = "POST", String UserAgent = "ModuleLauncherdotNet/1.1 (+http://blog.ahpxchina.cn)")
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                httpWebRequest.UserAgent = UserAgent;
                httpWebRequest.Method = Method;

                Stream stream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
                String reader = streamReader.ReadToEnd();
                streamReader.Dispose();
                return reader;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static async Task<string> GetHttpAsync(Uri uri, String Method = "POST", String UserAgent = "ModuleLauncherdotNet/1.1 (+http://blog.ahpxchina.cn)")
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.UserAgent = UserAgent;
                httpWebRequest.Method = Method.ToUpper();

                HttpWebResponse httpWebResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
                Stream stream = httpWebResponse.GetResponseStream();

                StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
                String reader = await streamReader.ReadToEndAsync();
                streamReader.Dispose();
                return reader;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static void DownloadFile(Uri uri, String savePath)
        {
            try
            {
                WebClient webClient = new WebClient() { };
                webClient.DownloadFile(uri, savePath);
                webClient.Dispose();
            }
            catch (Exception)
            {
            }
        }
        public static async Task DownloadFileAsync(Uri uri, String savePath)
        {
            try
            {
                WebClient webClient = new WebClient() { };
                await webClient.DownloadFileTaskAsync(uri, savePath);
                webClient.Dispose();
            }
            catch (Exception)
            {
            }
        }
    }
}
