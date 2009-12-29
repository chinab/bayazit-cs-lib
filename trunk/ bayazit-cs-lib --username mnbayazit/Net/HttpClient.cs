using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Bayazit.Threading;
using System.Net;

namespace Bayazit.Net
{
    #region Delegates
    public delegate void ResponseReceivedEventHandler(HttpConnection conn);
    public delegate void DownloadCompletedEventHandler(HttpConnection conn);
    public delegate void TimedOutEventHandler(HttpConnection conn);
    public delegate void ProgressChangedEventHandler(HttpConnection conn);
    public delegate void BufferFilledEventHandler(HttpConnection conn);
    #endregion

    public class HttpClient
    {
        #region Events
        public event ResponseReceivedEventHandler ResponseReceived;
        public event DownloadCompletedEventHandler DownloadCompleted;
        public event TimedOutEventHandler TimedOut;
        public event ProgressChangedEventHandler ProgressChanged;
        public event BufferFilledEventHandler BufferFilled;
        #endregion

        #region Properties
        public int TimeOutInterval { get; set; }
        public int ActiveDownloads { get { return counter.CurrentCount; } }
        public int MaxConnections { get { return counter.MaxCount; } set { counter.MaxCount = value; } }
        public int MaxBufferSize { get; set; }
        public string Method { get; set; }
        #endregion

        #region Variables
        private ThreadCounter counter = new ThreadCounter();
        #endregion

        #region Constructor
        public HttpClient() {
            TimeOutInterval = 15 * 1000;
            MaxBufferSize = 4 * 1024 * 1024;
            Method = "GET";
        }
        #endregion

        #region Public Methods
        public HttpConnection SendRequest(Uri uri) {
            counter.Increment();
            Console.WriteLine("Requesting {0}", uri.AbsoluteUri);
            var conn = new HttpConnection(uri);
            conn.request.Method = Method;
            var asyncResult = conn.request.BeginGetResponse(ResponseCallback, conn);
            ThreadPool.RegisterWaitForSingleObject(asyncResult.AsyncWaitHandle, TimeoutCallback, conn, TimeOutInterval, true);
            return conn;
        }

        public void Wait() {
            counter.WaitUntilZero();
        }
        #endregion

        #region Private Methods
        private void TimeoutCallback(object state, bool timedOut) {
            var conn = state as HttpConnection;

            if (timedOut) {
                if (TimedOut != null) TimedOut(conn);
                else conn.Abort();
            }
        }

        private void ResponseCallback(IAsyncResult asyncResult) {
            var conn = asyncResult.AsyncState as HttpConnection;

            try {
                conn.response = conn.request.EndGetResponse(asyncResult) as HttpWebResponse;
            } catch (WebException e) {
                if (e.Status == WebExceptionStatus.RequestCanceled) {
                    counter.Decrement();
                    return;
                } else throw;
            }

            if (ResponseReceived != null) ResponseReceived(conn);
            int capacity = conn.ContentLength == -1 || conn.ContentLength > MaxBufferSize ? MaxBufferSize : (int)conn.ContentLength;
            conn.data = new byte[capacity];
            conn.respStream = conn.response.GetResponseStream();
            ReadStream(conn);
        }

        private void ReadStream(HttpConnection conn) {
            int offset = conn.bytesRead % conn.data.Length;
            int bytesLeft = conn.data.Length - offset;

            if (BufferFilled != null && conn.bytesRead > 0 && offset == 0) {
                BufferFilled(conn);
            }

            conn.respStream.BeginRead(conn.data, offset, bytesLeft, ReadCallback, conn);
        }

        private void ReadCallback(IAsyncResult asyncResult) {
            var conn = asyncResult.AsyncState as HttpConnection;
            int bytesRead = conn.respStream.EndRead(asyncResult);

            if (bytesRead > 0) {
                conn.bytesRead += bytesRead;
                if (ProgressChanged != null) ProgressChanged(conn);
                ReadStream(conn);
            } else {
                if (conn.bytesRead < conn.data.Length) Array.Resize(ref conn.data, conn.bytesRead);
                if (DownloadCompleted != null) DownloadCompleted(conn);
                conn.Close();
                counter.Decrement();
            }
        }
        #endregion
    }
}