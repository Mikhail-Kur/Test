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
            
                
                string passwor;
                string Req;
                string Pass = "8e49ff607b1f46e1a5e8f6ad5d312a80";
                WebRequest request = WebRequest.Create("http://api.pixlpark.com/oauth/requesttoken");
                request.Method = "GET";
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

                
                request = WebRequest.Create("http://api.pixlpark.com/oauth/accesstoken");
                request.Method = "GET";
                passwor = Req + Pass;
                Console.WriteLine(passwor);
                SHA1 sha = SHA1.Create();
                byte[] bytes = new ASCIIEncoding().GetBytes(passwor);
                sha.ComputeHash(bytes);
                string acces = Convert.ToBase64String(sha.Hash);
                acces = acces.Remove(acces.Length-1);
                Console.WriteLine(acces);
                request.Headers.Add("oauth_token", Req);
                request.Headers.Add("username", "38cd79b5f2b2486d86f562e3c43034f8");
                request.Headers.Add("password", acces);
                request.Headers.Add("grant_type", "api");


                response = request.GetResponse();
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                using (Stream dataStream = response.GetResponseStream())
                {
                    
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFrom = reader.ReadToEnd();
                    dynamic resp = JObject.Parse(responseFrom);
                    Console.WriteLine(resp);
                }
                response.Close();

            

        }
    }
}
