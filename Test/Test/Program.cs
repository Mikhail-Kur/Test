using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string sha1(string input)
            {
                byte[] hash;
                using (var sha1 = new SHA1CryptoServiceProvider())
                    hash = sha1.ComputeHash(Encoding.Unicode.GetBytes(input));
                var sb = new StringBuilder();
                foreach (byte b in hash) sb.AppendFormat("{0:x2}", b);
                return sb.ToString();
            }



            string passwor;
            string Req;
            string Pass = "8e49ff607b1f46e1a5e8f6ad5d312a80";
            string Username = "38cd79b5f2b2486d86f562e3c43034f8";

            WebRequest request = WebRequest.Create("http://api.pixlpark.com/oauth/requesttoken");
            request.Method = "GET";
            var proxy = new WebProxy("127.0.0.1:8888");
            request.Proxy = proxy;

            WebResponse response = request.GetResponse();


                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    dynamic resp = JObject.Parse(responseFromServer);
                    Console.WriteLine(resp.RequestToken);
                    Req = resp.RequestToken;

                }
                response.Close();


            passwor = Req + Pass;
            Console.WriteLine(passwor);
            string hash = sha1(passwor);

            var cookieContainer = new CookieContainer();
            var getRequest = new GetRequest("http://api.pixlpark.com/oauth/accesstoken");
            getRequest.Grant_type = "api";
            getRequest.Oauth_token = Req;
            getRequest.Username = Username;
            getRequest.Password = hash;
            getRequest.Proxy = proxy;
            getRequest.Run(cookieContainer);
            Console.WriteLine(getRequest.Response);
           



        }
    }
}
