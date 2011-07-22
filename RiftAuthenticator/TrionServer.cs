﻿#define USE_WEB_REQUEST
//#define USE_HTTP_POST

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RiftAuthenticator
{
    static class TrionServer
    {
        const string TrionApiServer = "https://rift.trionworlds.com";
        const string TrionAuthServer = "https://auth.trionworlds.com";

        private static byte[] ExecuteRequest(Uri uri)
        {
            return ExecuteRequest(uri, new Dictionary<string, string>());
        }

        private static byte[] ExecuteRequest(Uri uri, Dictionary<string, string> postVariables)
        {
#if !USE_HTTP_POST || !USE_WEB_REQUEST
            var uriBuilder = new UriBuilder(uri);
            var oldQuery = uriBuilder.Query;
            if (oldQuery != null && oldQuery.StartsWith("?"))
                oldQuery = oldQuery.Substring(1);
            var queryIsEmpty = string.IsNullOrEmpty(oldQuery);
            var query = new StringBuilder();
            query.Append(oldQuery);
            foreach (var postVariable in postVariables)
            {
                var name = postVariable.Key;
                var value = postVariable.Value;
                if (!queryIsEmpty)
                    query.Append("&");
                else
                    queryIsEmpty = false;
                query.AppendFormat("{0}={1}", name, Uri.EscapeDataString(value));
            }
            uriBuilder.Query = query.ToString();
            uri = uriBuilder.Uri;
#endif

#if USE_WEB_REQUEST
            var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
#if USE_HTTP_POST
            request.Method = System.Net.WebRequestMethods.Http.Post;
            using (var requestStream = request.GetRequestStream())
            {
                var requestWriter = new System.IO.StreamWriter(requestStream) { NewLine = "&" };
                foreach (var postVariable in postVariables)
                {
                    var name = postVariable.Key;
                    var value = postVariable.Value;
                    requestWriter.WriteLine("{0}={1}", name, Uri.EscapeDataString(value));
                }
                requestWriter.Flush();
            }
#endif
            var response = (System.Net.HttpWebResponse)request.GetResponse();
            using (var responseStream = response.GetResponseStream())
            {
                var buffer = new System.IO.MemoryStream();
                responseStream.CopyTo(buffer);
                return buffer.ToArray();
            }
#else
            var client = new System.Net.WebClient();
            return client.DownloadData(uri);
#endif
        }

        public static long GetTimeOffset()
        {
            var uri = new Uri(string.Format("{0}{1}", TrionAuthServer, "/time"));
            var requestResult = Encoding.Default.GetString(ExecuteRequest(uri));
            var currentMillis = Util.currentTimeMillis();
            var serverMillis = long.Parse(requestResult);
            return serverMillis - currentMillis;
        }

        public static string[] GetSecurityQuestions(string userName, string password)
        {
            var variables = new Dictionary<string, string>
            {
                { "emailAddress", userName },
                { "password", password },
            };
            var uri = new Uri(string.Format("{0}/external/get-account-security-questions.action", TrionApiServer));
            var result = new System.IO.MemoryStream(ExecuteRequest(uri, variables));
            var resultXml = new System.Xml.XmlDocument();
            resultXml.Load(result);
            var questions = new string[2];
            foreach (var questionXml in resultXml.SelectNodes("/SecurityQuestions/*").Cast<System.Xml.XmlElement>())
            {
                switch (questionXml.LocalName)
                {
                    case "EmailAddress":
                        // Ignore
                        break;
                    case "FirstQuestion":
                        questions[0] = questionXml.InnerText;
                        break;
                    case "SecondQuestion":
                        questions[1] = questionXml.InnerText;
                        break;
                    case "ErrorCode":
                        throw new TrionServerException(questionXml.InnerText);
                }
            }
            return questions;
        }

        public static void RecoverSecurityKey(string userName, string password, string[] securityQuestionAnswers, Configuration config)
        {
            var variables = new Dictionary<string, string>
            {
                { "emailAddress", userName },
                { "password", password },
                { "deviceId", config.DeviceId },
                { "securityAnswer", securityQuestionAnswers[0] ?? string.Empty },
                { "secondSecurityAnswer", securityQuestionAnswers[1] ?? string.Empty },
            };
            var uri = new Uri(string.Format("{0}/external/retrieve-device-key.action", TrionApiServer));
            var result = new System.IO.MemoryStream(ExecuteRequest(uri, variables));
            var resultXml = new System.Xml.XmlDocument();
            resultXml.Load(result);
            ProcessSecretKeyResult(config, resultXml);
        }

        private static void ProcessSecretKeyResult(Configuration config, System.Xml.XmlDocument resultXml)
        {
            foreach (var questionXml in resultXml.SelectNodes("/DeviceKey/*").Cast<System.Xml.XmlElement>())
            {
                switch (questionXml.LocalName)
                {
                    case "DeviceId":
                        config.DeviceId = questionXml.InnerText;
                        break;
                    case "SerialKey":
                        config.SerialKey = questionXml.InnerText;
                        break;
                    case "SecretKey":
                        config.SecretKey = questionXml.InnerText;
                        break;
                    case "ErrorCode":
                        throw new TrionServerException(questionXml.InnerText);
                }
            }
        }

        public static void CreateSecurityKey(Configuration config)
        {
            var variables = new Dictionary<string, string>
            {
                { "deviceId", config.DeviceId },
            };
            var uri = new Uri(string.Format("{0}/external/create-device-key", TrionApiServer));
            var result = new System.IO.MemoryStream(ExecuteRequest(uri, variables));
            var resultXml = new System.Xml.XmlDocument();
            resultXml.Load(result);
            ProcessSecretKeyResult(config, resultXml);
        }
    }
}
