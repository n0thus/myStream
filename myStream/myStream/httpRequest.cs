using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace myStream
{
    class httpRequest
    {
        public static string currentAccessToken = "";

        public enum headerList : int { normal = 1, sub = 2, nothing = 3, oAuth = 4 };
        

        public static JObject createRequest(string url, string method, headerList hl)
        {
            string[] headersList;
            Console.WriteLine(hl.GetHashCode() + "");
            switch (hl.GetHashCode())
            {
                case 1:
                    headersList = new string[1] { "Client-ID: " + applicationInformations.clientID };
                    break;
                case 2:
                    headersList = new string[2] { "Client-ID: " + applicationInformations.clientID, "Authorization: Bearer " + currentAccessToken };
                    break;
                case 4:
                    headersList = new string[2] { "Client-ID: "+applicationInformations.clientID, "Authorization: OAuth " + currentAccessToken };
                    break;
                default:
                    headersList = new string[0];
                    break;
            }


            JObject result;
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.ContentType = "application/json";
            httpRequest.Accept = "application/vnd.twitchtv.v5+json";
            httpRequest.Method = method;

            foreach (string str in headersList)
            {

                //Console.WriteLine(str);
                httpRequest.Headers.Add(str);
            }


            if (method == "PUT")
            {
                httpRequest.ContentLength = 0;
            }

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException we)
            {
                response = (HttpWebResponse)we.Response;
            }

            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                result = JObject.Parse(streamReader.ReadToEnd());
            }

            return result;
        }
    }
}
