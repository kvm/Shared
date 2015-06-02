﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace YoutubeExtractor
{
    public static class HttpHelper
    {
        static string str;
        static ManualResetEvent completedEvent;
        public static string DownloadString(string url)
        {
// TODO: anichopr
//#if PORTABLE
            var request = WebRequest.Create(url);
            request.Method = "GET";
            request.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.101 Safari/537.36";

            System.Threading.Tasks.Task<WebResponse> task = System.Threading.Tasks.Task.Factory.FromAsync(
                request.BeginGetResponse,
                asyncResult => request.EndGetResponse(asyncResult),
                null);

            return task.ContinueWith(t => ReadStreamFromResponse(t.Result)).Result;
//#else
//            using (var client = new WebClient())
//            {
//                client.Encoding = System.Text.Encoding.UTF8;
//                return client.DownloadString(url);
//            }
//#endif
        }

        public static string HtmlDecode(string value)
        {
//TODO: anichopr
//#if PORTABLE
            return System.Net.WebUtility.HtmlDecode(value);
//#else
//            return System.Web.HttpUtility.HtmlDecode(value);
//#endif
        }

        public static IDictionary<string, string> ParseQueryString(string s)
        {
            // remove anything other than query string from url
            if (s.Contains("?"))
            {
                s = s.Substring(s.IndexOf('?') + 1);
            }

            var dictionary = new Dictionary<string, string>();

            foreach (string vp in Regex.Split(s, "&"))
            {
                string[] strings = Regex.Split(vp, "=");
                dictionary.Add(strings[0], strings.Length == 2 ? UrlDecode(strings[1]) : string.Empty);
            }

            return dictionary;
        }

        public static string ReplaceQueryStringParameter(string currentPageUrl, string paramToReplace, string newValue)
        {
            var query = ParseQueryString(currentPageUrl);

            query[paramToReplace] = newValue;

            var resultQuery = new StringBuilder();
            bool isFirst = true;

            foreach (KeyValuePair<string, string> pair in query)
            {
                if (!isFirst)
                {
                    resultQuery.Append("&");
                }

                resultQuery.Append(pair.Key);
                resultQuery.Append("=");
                resultQuery.Append(pair.Value);

                isFirst = false;
            }

            var uriBuilder = new UriBuilder(currentPageUrl)
            {
                Query = resultQuery.ToString()
            };

            return uriBuilder.ToString();
        }

        public static string UrlDecode(string url)
        {
// TODO anichopr
//#if PORTABLE
            return System.Net.WebUtility.UrlDecode(url);
//#else
//            return System.Web.HttpUtility.UrlDecode(url);
//#endif
        }

        private static string ReadStreamFromResponse(WebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream())
            {
                using (var sr = new StreamReader(responseStream))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        public static WebResponse GetHeadeResponse(string url)
        {
            var request = WebRequest.Create(url);
            request.Method = "HEAD";
            request.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.101 Safari/537.36";

            System.Threading.Tasks.Task<WebResponse> task = System.Threading.Tasks.Task.Factory.FromAsync(
                request.BeginGetResponse,
                asyncResult => request.EndGetResponse(asyncResult),
                null);

            return task.Result;
        }
    }
}