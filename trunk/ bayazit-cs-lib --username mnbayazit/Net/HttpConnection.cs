using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Security;
using System.Net.Cache;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace Bayazit.Net
{
    public class HttpConnection
    {
        #region Variables
        internal HttpWebRequest request = null;
        internal HttpWebResponse response = null;
        internal Stream respStream = null;
        internal byte[] data = null;
        internal int bytesRead = 0;
        #endregion

        #region Properties
        public byte[] Data { get { return data; } }
        public int BytesRead { get { return bytesRead; } }
        public float PercentComplete { get { return ContentLength <= 0 ? float.NaN : BytesRead * 100f / ContentLength; } }

        public string CharacterSet { get { return response.CharacterSet; } }
        public string ContentEncoding { get { return response.ContentEncoding; } }
        public long ContentLength { get { return response.ContentLength; } }
        public string ContentType { get { return response.ContentType; } }
        public CookieCollection Cookies { get { return response.Cookies; } }
        public WebHeaderCollection Headers { get { return response.Headers; } }
        public bool IsFromCache { get { return response.IsFromCache; } }
        public bool IsMutuallyAuthenticated { get { return response.IsMutuallyAuthenticated; } }
        public DateTime LastModified { get { return response.LastModified; } }
        public string Method { get { return response.Method; } }
        public Version ProtocolVersion { get { return response.ProtocolVersion; } }
        public Uri ResponseUri { get { return response.ResponseUri; } }
        public string Server { get { return response.Server; } }
        public HttpStatusCode StatusCode { get { return response.StatusCode; } }
        public string StatusDescription { get { return response.StatusDescription; } }

        public string Accept { get { return request.Accept; } }
        public Uri Address { get { return request.Address; } }
        public bool AllowAutoRedirect { get { return request.AllowAutoRedirect; } }
        public bool AllowWriteStreamBuffering { get { return request.AllowWriteStreamBuffering; } }
        public AuthenticationLevel AuthenticationLevel { get { return request.AuthenticationLevel; } }
        public DecompressionMethods AutomaticDecompression { get { return request.AutomaticDecompression; } }
        public RequestCachePolicy CachePolicy { get { return request.CachePolicy; } }
        public X509CertificateCollection ClientCertificates { get { return request.ClientCertificates; } }
        public string Connection { get { return request.Connection; } }
        public string ConnectionGroupName { get { return request.ConnectionGroupName; } }
        public HttpContinueDelegate ContinueDelegate { get { return request.ContinueDelegate; } }
        public CookieContainer CookieContainer { get { return request.CookieContainer; } }
        public ICredentials Credentials { get { return request.Credentials; } }
        public DateTime Date { get { return request.Date; } }
        public string Expect { get { return request.Expect; } }
        public bool HaveResponse { get { return request.HaveResponse; } }
        public string Host { get { return request.Host; } }
        public DateTime IfModifiedSince { get { return request.IfModifiedSince; } }
        public TokenImpersonationLevel ImpersonationLevel { get { return request.ImpersonationLevel; } }
        public bool KeepAlive { get { return request.KeepAlive; } }
        public int MaximumAutomaticRedirections { get { return request.MaximumAutomaticRedirections; } }
        public int MaximumResponseHeadersLength { get { return request.MaximumResponseHeadersLength; } }
        public string MediaType { get { return request.MediaType; } }
        public bool Pipelined { get { return request.Pipelined; } }
        public bool PreAuthenticate { get { return request.PreAuthenticate; } }
        public IWebProxy Proxy { get { return request.Proxy; } }
        public int ReadWriteTimeout { get { return request.ReadWriteTimeout; } }
        public string Referer { get { return request.Referer; } }
        public Uri RequestUri { get { return request.RequestUri; } }
        public bool SendChunked { get { return request.SendChunked; } }
        public ServicePoint ServicePoint { get { return request.ServicePoint; } }
        public int Timeout { get { return request.Timeout; } }
        public string TransferEncoding { get { return request.TransferEncoding; } }
        public bool UnsafeAuthenticatedConnectionSharing { get { return request.UnsafeAuthenticatedConnectionSharing; } }
        public bool UseDefaultCredentials { get { return request.UseDefaultCredentials; } }
        public string UserAgent { get { return request.UserAgent; } }
        #endregion

        #region Constructor
        internal HttpConnection(Uri uri)
        {
            request = WebRequest.Create(uri) as HttpWebRequest;
        }
        #endregion

        #region Methods
        public void Abort()
        {
            request.Abort();
            Close();
        }

        internal void Close()
        {
            if (response != null) response.Close();
        }
        #endregion
    }
}
