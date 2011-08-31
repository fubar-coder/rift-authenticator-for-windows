/*
 * This file is part of RIFT™ Authenticator for Windows.
 *
 * RIFT™ Authenticator for Windows is free software: you can redistribute 
 * it and/or modify it under the terms of the GNU General Public License 
 * as published by the Free Software Foundation, either version 3 of the 
 * License, or (at your option) any later version.
 *
 * RIFT™ Authenticator for Windows is distributed in the hope that it will 
 * be useful, but WITHOUT ANY WARRANTY; without even the implied warranty 
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU 
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with RIFT™ Authenticator for Windows.  If not, see 
 * <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace RiftAuthenticator.Library
{
    /// <summary>
    /// This class encapsulates the communication with TRIONs servers
    /// </summary>
    static public class TrionServer
    {
        const string TrionApiServer = "https://rift.trionworlds.com";
        const string TrionAuthServer = "https://auth.trionworlds.com";

        private static string _defaultUserAgent = string.Format(
            "RIFT Mobile Authenticator {0}; Android {1} ({2}); {3}, {4}, {5};",
                "1.0.4",    // Authenticator version
                "2.3.3",    // Android version
                10,         // Android SDK version
                "GINGERBREAD",  // Android product (don't know if this is correct...)
                "HTC Desire",   // Cell phone model (don't know if this is correct...)
                "O2"        // Cell phone brand (don't know if this is correct...)
            );

        /// <summary>
        /// The platform object used to get and use platform specific data and functions
        /// </summary>
        public static IPlatform Platform { get; set; }

        /// <summary>
        /// The default user agent used for the HTTP requests
        /// </summary>
        public static string DefaultUserAgent
        {
            get
            {
                return _defaultUserAgent;
            }
            set
            {
                _defaultUserAgent = value;
            }
        }

        private static byte[] ExecuteRequest(Uri uri)
        {
            return ExecuteRequest(uri, new Dictionary<string, string>());
        }

        private static byte[] ExecuteRequest(Uri uri, Dictionary<string, string> postVariables)
        {
            var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
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
            request.UserAgent = (Platform == null ? DefaultUserAgent : (Platform.UserAgent ?? DefaultUserAgent));
            var response = (System.Net.HttpWebResponse)request.GetResponse();
            using (var responseStream = response.GetResponseStream())
            {
                var buffer = new System.IO.MemoryStream();
                CopyTo(responseStream, buffer);
                return buffer.ToArray();
            }
        }

        private static void CopyTo(System.IO.Stream src, System.IO.Stream dst)
        {
            var bufferSize = 4096;
            var buffer = new byte[bufferSize];
            int copySize;
            while ((copySize = src.Read(buffer, 0, bufferSize)) != 0)
                dst.Write(buffer, 0, copySize);
        }

        /// <summary>
        /// Get the time difference between the client and the server
        /// </summary>
        /// <returns>Time difference in milliseconds</returns>
        public static long GetTimeOffset()
        {
            var uri = new Uri(string.Format("{0}{1}", TrionAuthServer, "/time"));
            var requestResult = Encoding.Default.GetString(ExecuteRequest(uri));
            var currentMillis = Util.CurrentTimeMillis();
            var serverMillis = long.Parse(requestResult);
            return serverMillis - currentMillis;
        }

        /// <summary>
        /// Gets the security questions assigned to a RIFT account
        /// </summary>
        /// <param name="userName">User name for a RIFT account</param>
        /// <param name="password">Password for a RIFT account</param>
        /// <returns>Array of security questions. The array has a length of 2.</returns>
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

        /// <summary>
        /// Recover an authenticator configuration for a given account and device id
        /// </summary>
        /// <param name="account">The account to recover</param>
        /// <param name="userName">User name for the RIFT account</param>
        /// <param name="password">Password for the RIFT account</param>
        /// <param name="securityQuestionAnswers">The answers to the security questions</param>
        /// <param name="deviceId">The device id used to recover the authenticator configuration</param>
        public static void RecoverSecurityKey(IAccount account, string userName, string password, string[] securityQuestionAnswers, string deviceId)
        {
            var variables = new Dictionary<string, string>
            {
                { "emailAddress", userName },
                { "password", password },
                { "deviceId", deviceId },
                { "securityAnswer", securityQuestionAnswers[0] ?? string.Empty },
                { "secondSecurityAnswer", securityQuestionAnswers[1] ?? string.Empty },
            };
            var uri = new Uri(string.Format("{0}/external/retrieve-device-key.action", TrionApiServer));
            var result = new System.IO.MemoryStream(ExecuteRequest(uri, variables));
            var resultXml = new System.Xml.XmlDocument();
            resultXml.Load(result);
            ProcessSecretKeyResult(account, resultXml);
            account.DeviceId = deviceId;
        }

        private static void ProcessSecretKeyResult(IAccount account, System.Xml.XmlDocument resultXml)
        {
            foreach (System.Xml.XmlElement itemXml in resultXml.SelectNodes("/DeviceKey/*"))
            {
                var value = (itemXml.InnerText == "null" ? null : itemXml.InnerText);
                switch (itemXml.LocalName)
                {
                    case "DeviceId":
                        account.DeviceId = value;
                        break;
                    case "SerialKey":
                        account.SerialKey = value;
                        break;
                    case "SecretKey":
                        account.SecretKey = value;
                        break;
                    case "ErrorCode":
                        throw new TrionServerException(value);
                }
            }
        }

        /// <summary>
        /// Creates a new security key (and other account information) for a given device id
        /// </summary>
        /// <param name="account">The account object used to write the new data to</param>
        /// <param name="deviceId">The device id to create the account information</param>
        public static void CreateSecurityKey(IAccount account, string deviceId)
        {
            var variables = new Dictionary<string, string>
            {
                { "deviceId", deviceId },
            };
            var uri = new Uri(string.Format("{0}/external/create-device-key", TrionApiServer));
            var result = new System.IO.MemoryStream(ExecuteRequest(uri, variables));
            var resultXml = new System.Xml.XmlDocument();
            resultXml.Load(result);
            ProcessSecretKeyResult(account, resultXml);
            account.DeviceId = deviceId;
        }

        /// <summary>
        /// Tests if the server certificate is valid.
        /// </summary>
        /// <param name="sender">An object that contains state information for this validation.</param>
        /// <param name="certificate">The certificate used to authenticate the remote party.</param>
        /// <param name="chain">The chain of certificate authorities associated with the remote certificate.</param>
        /// <param name="sslPolicyErrors">One or more errors associated with the remote certificate.</param>
        /// <returns>A System.Boolean value that determines whether the specified certificate is accepted for authentication.</returns>
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

        /// <summary>
        /// Returns the real device ID or (if none available) a random device ID.
        /// </summary>
        /// <returns>The real or random device ID</returns>
        public static string GetOrCreateRandomDeviceId()
        {
            var deviceId = GetDeviceId();
            if (deviceId == null)
                deviceId = CreateRandomDeviceId();
            return deviceId;
        }

        /// <summary>
        /// Creates a random device ID that's accepted by TRION
        /// </summary>
        /// <returns>Random device ID</returns>
        public static string CreateRandomDeviceId()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty).ToUpper();
        }

        /// <summary>
        /// Returns the real device ID
        /// </summary>
        /// <returns>null if no device ID available</returns>
        public static string GetDeviceId()
        {
            if (Platform == null)
                return null;
            return Platform.DeviceId;
        }
    }
}
