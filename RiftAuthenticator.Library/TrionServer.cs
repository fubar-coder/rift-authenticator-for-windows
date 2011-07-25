/**
 * This file is part of RIFT Authenticator for Windows.
 *
 * RIFT Authenticator for Windows is free software: you can redistribute 
 * it and/or modify it under the terms of the GNU General Public License 
 * as published by the Free Software Foundation, either version 3 of the 
 * License, or (at your option) any later version.
 *
 * RIFT Authenticator for Windows is distributed in the hope that it will 
 * be useful, but WITHOUT ANY WARRANTY; without even the implied warranty 
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU 
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with RIFT Authenticator for Windows.  If not, see 
 * <http://www.gnu.org/licenses/>.
 */

#define USE_WEB_REQUEST
#define USE_HTTP_POST

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace RiftAuthenticator.Library
{
    static public class TrionServer
    {
        const string TrionApiServer = "https://rift.trionworlds.com";
        const string TrionAuthServer = "https://auth.trionworlds.com";

        public static string UserAgent
        {
            get
            {
                return string.Format("RIFT Mobile Authenticator {0}; Android {1} ({2}); {3}, {4}, {5};",
                    "1.0.4",    // Authenticator version
                    "2.3.3",    // Android version
                    10,         // Android SDK version
                    "GINGERBREAD",  // Android product (don't know if this is correct...)
                    "HTC Desire",   // Cell phone model (don't know if this is correct...)
                    "O2"        // Cell phone brand (don't know if this is correct...)
                );
            }
        }

        private static byte[] ExecuteRequest(Uri uri)
        {
            return ExecuteRequest(uri, new Dictionary<string, string>());
        }

        private static byte[] ExecuteRequest(Uri uri, Dictionary<string, string> postVariables)
        {
#if (!USE_HTTP_POST && !USE_HTTP_HEADER) || !USE_WEB_REQUEST
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
            if (postVariables.Count != 0)
            {
                request.Method = System.Net.WebRequestMethods.Http.Post;
                request.ContentType = "application/x-www-form-urlencoded";
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
            }
#endif
            request.UserAgent = UserAgent;
            var response = (System.Net.HttpWebResponse)request.GetResponse();
            using (var responseStream = response.GetResponseStream())
            {
                var buffer = new System.IO.MemoryStream();
                CopyTo(responseStream, buffer);
                return buffer.ToArray();
            }
#else
            var client = new System.Net.WebClient();
            client.Headers["User-Agent"] = UserAgent;
            return client.DownloadData(uri);
#endif
        }

        private static void CopyTo(System.IO.Stream src, System.IO.Stream dst)
        {
            var bufferSize = 4096;
            var buffer = new byte[bufferSize];
            int copySize;
            while ((copySize = src.Read(buffer, 0, bufferSize)) != 0)
                dst.Write(buffer, 0, copySize);
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
            foreach (System.Xml.XmlElement questionXml in resultXml.SelectNodes("/SecurityQuestions/*"))
            {
                var value = (questionXml.InnerText == "null" ? null : questionXml.InnerText);
                switch (questionXml.LocalName)
                {
                    case "EmailAddress":
                        // Ignore
                        break;
                    case "FirstQuestion":
                        questions[0] = value;
                        break;
                    case "SecondQuestion":
                        questions[1] = value;
                        break;
                    case "ErrorCode":
                        throw new TrionServerException(value);
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
            foreach (System.Xml.XmlElement itemXml in resultXml.SelectNodes("/DeviceKey/*"))
            {
                var value = (itemXml.InnerText == "null" ? null : itemXml.InnerText);
                switch (itemXml.LocalName)
                {
                    case "DeviceId":
                        config.DeviceId = value;
                        break;
                    case "SerialKey":
                        config.SerialKey = value;
                        break;
                    case "SecretKey":
                        config.SecretKey = value;
                        break;
                    case "ErrorCode":
                        throw new TrionServerException(value);
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

        public static bool CertificateIsValid(object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors != System.Net.Security.SslPolicyErrors.None)
                return false;
            var cert = certificate as X509Certificate2;
            var hostName = cert.GetNameInfo(X509NameType.DnsName, false);
            var validTrionHostNames = new string[] {
                ".trionworlds.com",
                ".triongames.com",
                ".trionworld.priv",
                ".triongames.priv",
            };
            foreach (var validTrionHostName in validTrionHostNames)
            {
                if (hostName.EndsWith(validTrionHostName))
                    return true;
            }
            return false;
        }
    }
}
