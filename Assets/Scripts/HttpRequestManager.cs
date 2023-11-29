using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HttpRequestManager
{
    public class HttpRequestParams
    {
        //Default Values Below
        public string Content = "";
        public List<KeyValuePair<string, string>> HeaderPairs = new List<KeyValuePair<string, string>>();
        public bool PreAuthenticate = false;
        public string ContentType = "application/json";
        public int Timeout = 60;
    }

    private static HttpRequestParams defaultParams = new HttpRequestParams();

    public static async Task<string> GetHttpRequest(string url, string content)
    {
        HttpRequestParams httpParams = new HttpRequestParams();
        httpParams.Content = content;
        var task = await GetHttpRequest(url, httpParams);
        return task;
    }

    public static async Task<string> GetHttpRequest(string url, HttpRequestParams httpParams = null)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";

        if (httpParams == null)
            httpParams = defaultParams;

        request.Timeout = httpParams.Timeout * 1000;
        request.ContentType = httpParams.ContentType;

        if (httpParams.HeaderPairs.Count > 0)
        {
            foreach (var headerPair in httpParams.HeaderPairs)
            {
                request.Headers.Add(headerPair.Key, headerPair.Value);
            }
        }

        if (!string.IsNullOrEmpty(httpParams.Content))
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(httpParams.Content);
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
        }

        HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
        Stream responseStream = response.GetResponseStream();
        StreamReader responseReader = new StreamReader(responseStream);
        string result = responseReader.ReadToEnd();

        return result;
    }

    public static async Task<string> PostHttpRequest(string url, string body)
    {
        HttpRequestParams httpParams = new HttpRequestParams();
        httpParams.Content = body;
        var task = await PostHttpRequest(url, httpParams);
        return task;
    }

    public static async Task<string> PostHttpRequest(string url, HttpRequestParams httpParams = null)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";

        if (httpParams == null)
            httpParams = defaultParams;

        request.Timeout = httpParams.Timeout * 1000;
        request.ContentType = httpParams.ContentType;

        if (httpParams.HeaderPairs.Count > 0)
        {
            foreach (var headerPair in httpParams.HeaderPairs)
            {
                request.Headers.Add(headerPair.Key, headerPair.Value);
            }
        }

        if (!string.IsNullOrEmpty(httpParams.Content))
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(httpParams.Content);
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
        }

        HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
        Stream responseStream = response.GetResponseStream();
        StreamReader responseReader = new StreamReader(responseStream);
        string result = responseReader.ReadToEnd();

        return result;
    }
}
