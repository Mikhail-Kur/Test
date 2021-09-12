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
            byte[] hash;
            string passwor;
            string Req;
            string Pass= "8e49ff607b1f46e1a5e8f6ad5d312a80";
            WebRequest request = WebRequest.Create("http://api.pixlpark.com/oauth/requesttoken");
            request.Method = "POST";

            Stream dataStream = request.GetRequestStream();

            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            using (dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                dynamic resp = JObject.Parse(responseFromServer);
                // Display the content.
                Console.WriteLine(resp.RequestToken);
                Req = resp.RequestToken;
                Console.WriteLine(Req + Pass);
            }
            response.Close();

            // Close the response.
            request= WebRequest.Create("http://api.pixlpark.com/oauth/accesstoken");
            request.Method="GET";
            passwor = Req + Pass;
            SHA1 sHA = SHA1.Create();
            hash = sHA.ComputeHash(Encoding.Default.GetBytes(passwor));
            string acces = "";
            for (int i = 0; i < hash.Length; i++)
            {
                acces += hash[i];
            }
            request.Headers.Add("oauth_token",Req);
            request.Headers.Add("grant_type", "POST");
            request.Headers.Add("username", "38cd79b5f2b2486d86f562e3c43034f8");

            request.Headers.Add("password", acces);


            // Get the response.
            response = request.GetResponse();
            dataStream = response.GetResponseStream();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            using (dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFrom = reader.ReadToEnd();
                dynamic resp = JObject.Parse(responseFrom);
                // Display the content.
                Console.WriteLine(responseFrom+"   ==");
            }   

            /*
            String id = "38cd79b5f2b2486d86f562e3c43034f8";
            String secret = "8e49ff607b1f46e1a5e8f6ad5d312a80";

            var client = new RestClient("http://api.pixlpark.com/oauth/requesttoken");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials&scope=all&client_id=" + id + "&client_secret=" + secret, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            dynamic resp = JObject.Parse(response.Content);
            String token = resp.RequestToken;
            Console.WriteLine(response.Cookies);
            Console.WriteLine(response.Headers);

            var request1 = new GetRequest("http://api.pixlpark.com/orders", token,id,secret);
            request1.Run();
            */

        }
    }
}
