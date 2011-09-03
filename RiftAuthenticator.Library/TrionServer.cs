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

        private static ISecretKeyEncryption _defaultSecretKeyEncryption = new PlatformUtils.Android.AndroidSecretKeyEncryption();

        /// <summary>
        /// The platform object used to get and use platform specific data and functions
        /// </summary>
        public static IPlatform Platform { get; set; }

        /// <summary>
        /// Get the secret key encryption object
        /// </summary>
        /// <remarks>
        /// This secret key encryption object is used to encrypt and decrypt
        /// the secret key in a platform specific way.
        /// </remarks>
        public static ISecretKeyEncryption SecretKeyEncryption
        {
            get
            {
                if (Platform == null || Platform.SecretKeyEncryption == null)
                    return _defaultSecretKeyEncryption;
                return Platform.SecretKeyEncryption;
            }
        }

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

        private static string UrlEncode(string value)
        {
#if WINDOWS_PHONE
            return System.Net.HttpUtility.UrlEncode(value);
#else
            return Uri.EscapeDataString(value);
#endif
        }

        private static void WritePostVariables(System.IO.Stream requestStream, Dictionary<string, string> postVariables)
        {
            var requestWriter = new System.IO.StreamWriter(requestStream) { NewLine = "&" };
            foreach (var postVariable in postVariables)
            {
                var name = postVariable.Key;
                var value = postVariable.Value;
                requestWriter.WriteLine("{0}={1}", name, UrlEncode(value));
            }
            requestWriter.Flush();
        }

        private static void WriteRequest(System.Net.HttpWebRequest request, Dictionary<string, string> postVariables, Action requestSent)
        {
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            var asyncRequest = request.BeginGetRequestStream((ar) =>
            {
                using (var requestStream = request.EndGetRequestStream(ar))
                {
                    WritePostVariables(requestStream, postVariables);
                }
                requestSent();
            }, null);
        }

        class ExecuteRequestAsyncResult : Helpers.AsyncResult<byte[]>
        {
            private Dictionary<string, string> PostVariables { get; set; }
            public System.Net.HttpWebRequest WebRequest { get; private set; }

            public ExecuteRequestAsyncResult(AsyncCallback userCallback, object stateObject, object owner, string operationId, System.Net.HttpWebRequest request, Dictionary<string, string> postVariables)
                : base(userCallback, stateObject, owner, operationId)
            {
                WebRequest = request;
                PostVariables = postVariables;
            }

            private void ReadResponse(bool rethrowExceptions)
            {
                try
                {
                    var asyncResponse = WebRequest.BeginGetResponse((ar) =>
                    {
                        try
                        {
                            var response = WebRequest.EndGetResponse(ar);
                            var buffer = new System.IO.MemoryStream();
                            using (var responseStream = response.GetResponseStream())
                            {
                                CopyTo(responseStream, buffer);
                            }
                            SetResult(buffer.ToArray());
                            Complete(null, ar.CompletedSynchronously);
                        }
                        catch (System.Net.WebException ex)
                        {
                            Complete(ex);
                        }
                    }, null);
                }
                catch (System.Net.WebException ex)
                {
                    if (rethrowExceptions)
                        throw;
                    Complete(ex);
                }
            }

            internal override void Process()
            {
                if (PostVariables.Count != 0)
                {
                    WebRequest.Method = "POST";
                    WebRequest.ContentType = "application/x-www-form-urlencoded";
                    WriteRequest(WebRequest, PostVariables, () =>
                    {
                        ReadResponse(false);
                    });
                }
                else
                {
                    ReadResponse(true);
                }
            }
        }

        private static IAsyncResult BeginExecuteRequest(AsyncCallback userCallback, object stateObject, Uri uri)
        {
            return BeginExecuteRequest(userCallback, stateObject, uri, new Dictionary<string, string>());
        }

        private static IAsyncResult BeginExecuteRequest(AsyncCallback userCallback, object stateObject, Uri uri, Dictionary<string, string> postVariables)
        {
            var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
            request.UserAgent = (Platform == null ? DefaultUserAgent : (Platform.UserAgent ?? DefaultUserAgent));
            var result = new ExecuteRequestAsyncResult(userCallback, stateObject, typeof(TrionServer), "execute-request", request, postVariables);
            result.Process();
            return result;
        }

        private static byte[] EndExecuteRequest(IAsyncResult asyncResult)
        {
            return Helpers.AsyncResult<byte[]>.End(asyncResult, typeof(TrionServer), "execute-request");
        }

        private static void CopyTo(System.IO.Stream src, System.IO.Stream dst)
        {
            var bufferSize = 4096;
            var buffer = new byte[bufferSize];
            int copySize;
            while ((copySize = src.Read(buffer, 0, bufferSize)) != 0)
                dst.Write(buffer, 0, copySize);
        }

        class ExecuteTimeOffsetAsyncResult : Helpers.AsyncResult<long>
        {
            public Uri RequestUri { get; private set; }

            public ExecuteTimeOffsetAsyncResult(Uri uri, AsyncCallback userCallback, object stateObject, object owner, string operationId)
                : base(userCallback, stateObject, owner, operationId)
            {
                RequestUri = uri;
            }

            internal override void Process()
            {
                TrionServer.BeginExecuteRequest((ar) =>
                {
                    try
                    {
                        var requestResultBytes = TrionServer.EndExecuteRequest(ar);
                        var requestResult = Encoding.UTF8.GetString(requestResultBytes, 0, requestResultBytes.Length);
                        var currentMillis = Util.CurrentTimeMillis();
                        var serverMillis = long.Parse(requestResult);
                        SetResult(serverMillis - currentMillis);
                        Complete(null, ar.CompletedSynchronously);
                    }
                    catch (System.Net.WebException ex)
                    {
                        Complete(ex, false);
                    }
                }, null, RequestUri);
            }
        }

        /// <summary>
        /// Get the time difference between the client and the server
        /// </summary>
        /// <param name="stateObject">A user defined value</param>
        /// <param name="userCallback">The function that get's called when the function is finished</param>
        /// <returns>Object which implements IAsyncResult that allows the caller to wait for the function to return</returns>
        public static IAsyncResult BeginGetTimeOffset(AsyncCallback userCallback, object stateObject)
        {
            var uri = new Uri(string.Format("{0}{1}", TrionAuthServer, "/time"));
            var result = new ExecuteTimeOffsetAsyncResult(uri, userCallback, stateObject, typeof(TrionServer), "time-offset");
            result.Process();
            return result;
        }

        /// <summary>
        /// Gets the data for the finished asynchronous call
        /// </summary>
        /// <param name="asyncResult">The object from the "Begin..." function</param>
        /// <returns>The time difference in milliseconds</returns>
        public static long EndGetTimeOffset(IAsyncResult asyncResult)
        {
            return Helpers.AsyncResult<long>.End(asyncResult, typeof(TrionServer), "time-offset");
        }

        class ExecuteGetSecurityQuestionsAsyncResult : Helpers.AsyncResult<string[]>
        {
            private Uri RequestUri { get; set; }
            private Dictionary<string, string> PostVariables { get; set; }

            public ExecuteGetSecurityQuestionsAsyncResult(AsyncCallback userCallback, object stateObject, object owner, string operationId, Uri requestUri, Dictionary<string, string> postVariables)
                : base(userCallback, stateObject, owner, operationId)
            {
                RequestUri = requestUri;
                PostVariables = postVariables;
            }

            internal override void Process()
            {
                TrionServer.BeginExecuteRequest((ar) =>
                {
                    try
                    {
                        var requestResultBytes = TrionServer.EndExecuteRequest(ar);
                        var resultStream = new System.IO.MemoryStream(requestResultBytes);
                        var resultXml = System.Xml.Linq.XDocument.Load(resultStream);
                        var questions = new string[2];
                        foreach (var questionXml in resultXml.Element("SecurityQuestions").Elements())
                        {
                            var value = (questionXml.Value == "null" ? null : questionXml.Value);
                            switch (questionXml.Name.LocalName)
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
                        SetResult(questions);
                        Complete(null, ar.CompletedSynchronously);
                    }
                    catch (System.Net.WebException ex)
                    {
                        Complete(ex, false);
                    }
                    catch (System.Xml.XmlException ex)
                    {
                        Complete(ex, false);
                    }
                    catch (TrionServerException ex)
                    {
                        Complete(ex, false);
                    }
                }, null, RequestUri, PostVariables);
            }
        }

        /// <summary>
        /// Gets the security questions assigned to a RIFT account
        /// </summary>
        /// <param name="stateObject">A user defined value</param>
        /// <param name="userCallback">The function that get's called when the function is finished</param>
        /// <param name="userName">User name for a RIFT account</param>
        /// <param name="password">Password for a RIFT account</param>
        /// <returns>Object which implements IAsyncResult that allows the caller to wait for the function to return</returns>
        public static IAsyncResult BeginGetSecurityQuestions(AsyncCallback userCallback, object stateObject, string userName, string password)
        {
            var variables = new Dictionary<string, string>
            {
                { "emailAddress", userName },
                { "password", password },
            };
            var uri = new Uri(string.Format("{0}/external/get-account-security-questions.action", TrionApiServer));
            var result = new ExecuteGetSecurityQuestionsAsyncResult(userCallback, stateObject, typeof(TrionServer), "get-security-questions", uri, variables);
            result.Process();
            return result;
        }

        /// <summary>
        /// Gets the data for the finished asynchronous call
        /// </summary>
        /// <param name="asyncResult">The object from the "Begin..." function</param>
        /// <returns>The security questions. The array has a length of 2.</returns>
        public static string[] EndGetSecurityQuestions(IAsyncResult asyncResult)
        {
            return Helpers.AsyncResult<string[]>.End(asyncResult, typeof(TrionServer), "get-security-questions");
        }

        class ExecuteSecurityKeyAsyncResult : Helpers.AsyncResultNoResult
        {
            private Uri RequestUri { get; set; }
            private Dictionary<string, string> PostVariables { get; set; }
            private IAccount Account { get; set; }
            private string DeviceId { get; set; }

            public ExecuteSecurityKeyAsyncResult(AsyncCallback userCallback, object stateObject, object owner, string operationId, Uri requestUri, Dictionary<string, string> postVariables, IAccount account, string deviceId)
                : base(userCallback, stateObject, owner, operationId)
            {
                Account = account;
                RequestUri = requestUri;
                PostVariables = postVariables;
                DeviceId = deviceId;
            }

            internal override void Process()
            {
                TrionServer.BeginExecuteRequest((ar) =>
                {
                    try
                    {
                        var requestResultBytes = TrionServer.EndExecuteRequest(ar);
                        var resultStream = new System.IO.MemoryStream(requestResultBytes);
                        var resultXml = System.Xml.Linq.XDocument.Load(resultStream);
                        ProcessSecretKeyResult(Account, resultXml);
                        Account.DeviceId = DeviceId;
                        Complete(null, ar.CompletedSynchronously);
                    }
                    catch (System.Net.WebException ex)
                    {
                        Complete(ex, false);
                    }
                    catch (System.Xml.XmlException ex)
                    {
                        Complete(ex, false);
                    }
                    catch (TrionServerException ex)
                    {
                        Complete(ex, false);
                    }
                }, null, RequestUri, PostVariables);
            }
        }

        /// <summary>
        /// Recover an authenticator configuration for a given account and device id
        /// </summary>
        /// <param name="stateObject">A user defined value</param>
        /// <param name="userCallback">The function that get's called when the function is finished</param>
        /// <param name="account">The account to recover</param>
        /// <param name="userName">User name for the RIFT account</param>
        /// <param name="password">Password for the RIFT account</param>
        /// <param name="securityQuestionAnswers">The answers to the security questions</param>
        /// <param name="deviceId">The device id used to recover the authenticator configuration</param>
        /// <returns>Object which implements IAsyncResult that allows the caller to wait for the function to return</returns>
        public static IAsyncResult BeginRecoverSecurityKey(AsyncCallback userCallback, object stateObject, IAccount account, string userName, string password, string[] securityQuestionAnswers, string deviceId)
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
            var result = new ExecuteSecurityKeyAsyncResult(userCallback, stateObject, typeof(TrionServer), "recover-security-key", uri, variables, account, deviceId);
            result.Process();
            return result;
        }

        /// <summary>
        /// Gets the data for the finished asynchronous call
        /// </summary>
        /// <param name="asyncResult">The object from the "Begin..." function</param>
        public static void EndRecoverSecurityKey(IAsyncResult asyncResult)
        {
            Helpers.AsyncResultNoResult.End(asyncResult, typeof(TrionServer), "recover-security-key");
        }

        private static void ProcessSecretKeyResult(IAccount account, System.Xml.Linq.XDocument resultXml)
        {
            foreach (var itemXml in resultXml.Element("DeviceKey").Elements())
            {
                var value = (itemXml.Value == "null" ? null : itemXml.Value);
                switch (itemXml.Name.LocalName)
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
        /// <param name="stateObject">A user defined value</param>
        /// <param name="userCallback">The function that get's called when the function is finished</param>
        /// <returns>Object which implements IAsyncResult that allows the caller to wait for the function to return</returns>
        public static IAsyncResult BeginCreateSecurityKey(AsyncCallback userCallback, object stateObject, IAccount account, string deviceId)
        {
            var variables = new Dictionary<string, string>
            {
                { "deviceId", deviceId },
            };
            var uri = new Uri(string.Format("{0}/external/create-device-key", TrionApiServer));
            var result = new ExecuteSecurityKeyAsyncResult(userCallback, stateObject, typeof(TrionServer), "create-security-key", uri, variables, account, deviceId);
            result.Process();
            return result;
        }

        /// <summary>
        /// Gets the data for the finished asynchronous call
        /// </summary>
        /// <param name="asyncResult">The object from the "Begin..." function</param>
        public static void EndCreateSecurityKey(IAsyncResult asyncResult)
        {
            Helpers.AsyncResultNoResult.End(asyncResult, typeof(TrionServer), "create-security-key");
        }

#if !WINDOWS_PHONE
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
#endif

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
