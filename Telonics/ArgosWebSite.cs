using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace Telonics
{
    /// <summary>
    /// Provides access to data on the Argos Web Server
    /// </summary>
    public static class ArgosWebSite
    {

        /// <summary>
        /// Provides the results (in a modified CSV format) from the Argos web server
        /// This object is immutable, and should only be created by an ArgosWebsite object
        /// Thhis object encapsulates the representation of the results.
        /// </summary>
        public class ArgosWebResult
        {
            private readonly string _text;

            //We can't make the constructor private (ArgosWebSite can't create it)
            //The best we can do is make it internal, and make sure no other methods in the
            //library call the constructor.
            internal ArgosWebResult(string text)
            {
                if (String.IsNullOrEmpty(text))
                    throw new ArgumentNullException("text");
                _text = text;
            }

            /// <summary>
            /// Returns the Argos web results as a single string
            /// </summary>
            /// <returns>string</returns>
            public override string ToString()
            {
                return _text;
            }

            /// <summary>
            /// Returns the Argos web results as an array of UTF8 bytes.
            /// </summary>
            /// <returns>Byte Array</returns>
            public Byte[] ToBytes()
            {
                var e = new UTF8Encoding();
                return e.GetBytes(_text);
            }
        }


        public static readonly int MinDays = Properties.Settings.Default.ArgosServerMinDownloadDays;
        public static readonly int MaxDays = Properties.Settings.Default.ArgosServerMaxDownloadDays;
        public static readonly string ArgosUrl = Properties.Settings.Default.ArgosUrl;
        public static readonly string ArgosPlatformListSoapRequest =
            Properties.Settings.Default.ArgosPlatformListSoapRequest;
        public static readonly string ArgosPlatformSoapRequest =
            Properties.Settings.Default.ArgosPlatformSoapRequest;
        public static readonly string ArgosProgramSoapRequest = Properties.Settings.Default.ArgosProgramSoapRequest;


        /// <summary>
        /// Queries the Argos Web Services, and returns the results for a collar.
        /// </summary>
        /// <param name="username">A user name assigned by the Argos Web Service</param>
        /// <param name="password">The user's password</param>
        /// <param name="collar">A collar (platform) identifier in the user's account</param>
        /// <param name="days">Number of days in the past to retrieve (1 to 10)</param>
        /// <param name="error">Contains any errors encountered; null with no errors</param>
        /// <returns>Returns the results from the web server.  If null check the error output parameter</returns>
        public static ArgosWebResult GetCollar(string username, string password, string collar, int days, out string error)
        {
            error = CheckParameters(username, password, collar, days);
            if (error != null)
                return null;
            var request = String.Format(ArgosPlatformSoapRequest, username, password, collar, days);
            return GetArgosWebResult(request, out error);
        }

        /// <summary>
        /// Queries the Argos Web Services, and returns the results for a program (collection of collars).
        /// </summary>
        /// <param name="username">A user name assigned by the Argos Web Service</param>
        /// <param name="password">The user's password</param>
        /// <param name="program">A program identifier in the user's account</param>
        /// <param name="days">Number of days in the past to retrieve (1 to 10)</param>
        /// <param name="error">Contains any errors encountered; null with no errors</param>
        /// <returns>Returns the results from the web server.  If null check the error output parameter</returns>
        public static ArgosWebResult GetProgram(string username, string password, string program, int days, out string error)
        {
            error = CheckParameters(username, password, program, days);
            if (error != null)
                return null;
            var request = String.Format(ArgosProgramSoapRequest, username, password, program, days);
            return GetArgosWebResult(request, out error);
        }

        /// <summary>
        /// Queries the Argos Web Services, and returns the list of platforms (collars) for a program (collection of collars).
        /// </summary>
        /// <param name="username">A user name assigned by the Argos Web Service</param>
        /// <param name="password">The user's password</param>
        /// <param name="error">Contains any errors encountered; null with no errors</param>
        /// <returns>Returns the results from the web server.  If null check the error output parameter</returns>
        public static IEnumerable<Tuple<string,string>> GetPlatformList(string username, string password, out string error)
        {
            error = CheckParameters(username, password, "no selector required", MinDays);
            if (error != null)
                return null;
            var request = String.Format(ArgosPlatformListSoapRequest, username, password);
            var response = GetArgosWebResult(request, out error);
            if (error != null)
                return new Tuple<string, string>[0];
            var xml = XDocument.Load(new StringReader(response.ToString()));
            var list = new List<Tuple<string, string>>();
            foreach (var program in xml.Descendants("program"))
            {
                list.AddRange(
                    program.Descendants("platform")
                           .Select(
                               platform =>
                               new Tuple<string, string>((string) program.Element("programNumber"),
                                                         (string) platform.Element("platformId"))));
            }
            return list;
        }


        private static string CheckParameters(string username, string password, string selector, int days)
        {
            string error = null;
            if (String.IsNullOrEmpty(username))
                error = "No username provided";
            if (String.IsNullOrEmpty(password))
            {
                const string msg = "No password provided";
                if (error == null)
                    error = msg;
                else
                    error += "; " + msg;
            }
            if (String.IsNullOrEmpty(selector))
            {
                const string msg = "No selector (collar or program) was provided";
                if (error == null)
                    error = msg;
                else
                    error += "; " + msg;
            }
            if (days < MinDays || MaxDays < days)
            {
                var msg = String.Format("Days out of range ({0}..{1})", MinDays, MaxDays);
                if (error == null)
                    error = msg;
                else
                    error += "; " + msg;
            }
            return error;
        }

        private static ArgosWebResult GetArgosWebResult(string request, out string error)
        {
            error = null;
            try
            {
                HttpWebRequest req = GetRequest(request);
                string response = GetResponse(req);
                if (String.IsNullOrEmpty(response))
                    error = "No response";
                else if (response.Contains("<errors><error code=\"4\">no data</error></errors>"))
                    error = "No data or unknown id";
                else if (response.Contains("<errors><error code=\"3\">authentification error</error></errors>"))
                    error = "Authentication Error: bad username or password";
                else if (response.Contains("<errors>"))
                    error = "Unknown error: " + response;
                if (error != null)
                    return null;
                return new ArgosWebResult(response);
            }
            catch (Exception ex)
            {
                error = "EXCEPTION\n\n" + ex.Message;
                return null;
            }
        }

        private static HttpWebRequest GetRequest(string request)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(request);
            var req = WebRequest.Create(ArgosUrl);
            //req.ContentType = "application/soap+xml; charset=utf-8";
            req.Method = "POST";
            req.ContentLength = buffer.Length;
            var sw = req.GetRequestStream();
            sw.Write(buffer, 0, buffer.Length);
            sw.Close();
            return (HttpWebRequest)req;
        }

        private static string GetResponse(WebRequest request)
        {
            var resp = (HttpWebResponse)request.GetResponse();
            var respStream = resp.GetResponseStream();
            if (respStream == null)
                throw new ApplicationException("Null response from Web Service");
            string response;
            using (var stream = new StreamReader(respStream))
            {
                response = stream.ReadToEnd();
            }
            int start = response.IndexOf("<return>", StringComparison.Ordinal) + 8;
            int end = response.IndexOf("</return>", StringComparison.Ordinal);
            response = response.Substring(start, end - start);
            return HttpUtility.HtmlDecode(response);
        }
    }
}
