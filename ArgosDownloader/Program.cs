using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace ArgosDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            const string username = "BURCH";
            const string password = "LOUGAROU";
            string argosId = args[0];
            string path = Path.Combine(@"C:\tmp",argosId + ".aws");  //path for the output file
            string results = GetArgosAsCsv(username, password, argosId);
            Console.WriteLine(results);
            File.WriteAllText(path,results);
#else
            if (args.Length != 4)
            {
                Console.WriteLine("Argument error - wrong number.");
                return;
            }
            string username = args[0];
            string password = args[1];
            string argosId = args[2];
            string path = args[3];  //path for the output file
            string results = GetArgosAsCsv(username, password, argosId);
            Console.WriteLine(results);
            File.WriteAllText(path,results);
#endif
        }

        private const string url = @"http://ws-argos.clsamerica.com/argosDws/services/DixService";


//TDC request - the PlatformId changes for each request
        private const string csvRequest = @"
<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:argos=""http://service.dataxmldistribution.argos.cls.fr/types"">
<soap:Header/>
<soap:Body>
<argos:csvRequest>
<argos:username>{0}</argos:username>
<argos:password>{1}</argos:password>
<argos:platformId>{2}</argos:platformId>
<argos:nbDaysFromNow>10</argos:nbDaysFromNow>
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
        //You can also use the programNumber to get all the collar in a program
        //<argos:programNumber>{2}</argos:programNumber>

        static private string GetArgosAsCsv(string username, string password, string argosId)
        {
            try
            {
                var request = String.Format(csvRequest, username, password, argosId );
                HttpWebRequest req = GetRequest(request);
                string response = GetResponse(req);
                if (response == "<errors><error code=\"4\">no data</error></errors>")
                    response = "Problem: No data\nunknown collar, or no fixes in last 10 days";
                else if (response == "<errors><error code=\"3\">authentification error</error></errors>")
                    response = "Problem: Authentication Error\nbad username or password";
                else if (response.Contains("<errors>"))
                    response = "Problem - Unknown error:\n" + response;
                return response;
                // FIXME - upload this file to database; save to file system
                // FIXME - process file with TDC.  We know the collar, so we can limit the TPF files used.
            }
            catch (Exception ex)
            {
                return "EXCEPTION\n\n" + ex.Message;
            }
        }

        static private HttpWebRequest GetRequest(string request)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(request);
            var req = WebRequest.Create(url);
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
