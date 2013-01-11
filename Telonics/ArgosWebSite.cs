using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Telonics
{
    static public class ArgosWebSite
    {
        private const string _argosUrl = @"http://ws-argos.clsamerica.com/argosDws/services/DixService";

        //Argos soap request for messages as CSV
        private const string _csvRequest = @"
<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:argos=""http://service.dataxmldistribution.argos.cls.fr/types"">
<soap:Header/>
<soap:Body>
<argos:csvRequest>
<argos:username>{0}</argos:username>
<argos:password>{1}</argos:password>
{2}
<argos:nbDaysFromNow>{3}</argos:nbDaysFromNow>
<argos:displayLocation>true</argos:displayLocation>
<argos:displayDiagnostic>true</argos:displayDiagnostic>
<argos:displayRawData>true</argos:displayRawData>
<argos:displayImageLocation>true</argos:displayImageLocation>
<argos:displayHexId>true</argos:displayHexId>
<argos:showHeader>true</argos:showHeader>
</argos:csvRequest>
</soap:Body>
</soap:Envelope>
";
        //replace {2} with either platform or program
        //Use the programNumber to get all the collars in a program
        //Use platform to get a specific collar (platformId = ArgosId)
        private const string _platform = "<argos:platformId>{0}</argos:platformId>";
        private const string _program = "<argos:programNumber>{0}</argos:programNumber>";


        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="collar"></param>
        /// <param name="days"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        static public ArgosWebResult GetCollarAsCsv(string username, string password, string collar, int days, out string error)
        {
            //FIXME - parameter checking
            var selector = String.Format(_platform, collar);
            var request = String.Format(_csvRequest, username, password, selector, days);
            return GetCsv(request, out error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="program"></param>
        /// <param name="days"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        static public ArgosWebResult GetProgramAsCsv(string username, string password, string program, int days, out string error)
        {
            //FIXME - parameter checking
            var selector = String.Format(_program, program);
            var request = String.Format(_csvRequest, username, password, selector, days);
            return GetCsv(request, out error);
        }


        static private ArgosWebResult GetCsv(string request, out string error)
        {
            error = null;
            try
            {
                HttpWebRequest req = GetRequest(request);
                string response = GetResponse(req);
                if (String.IsNullOrEmpty(response))
                    error = "No response";
                else if (response == "<errors><error code=\"4\">no data</error></errors>")
                    error = "No data:  unknown program/collar, or no fixes in requested time period";
                else if (response == "<errors><error code=\"3\">authentification error</error></errors>")
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

        static private HttpWebRequest GetRequest(string request)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(request);
            var req = WebRequest.Create(_argosUrl);
            //req.ContentType = "application/soap+xml; charset=utf-8";
            req.Method = "POST";
            req.ContentLength = buffer.Length;
            var sw = req.GetRequestStream();
            sw.Write(buffer, 0, buffer.Length);
            sw.Close();
            return (HttpWebRequest)req;
        }

        static private string GetResponse(HttpWebRequest request)
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
