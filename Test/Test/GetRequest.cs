using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Test
{
    public class GetRequest
    {


        HttpWebRequest _request;
        string _address;

        public Dictionary<string, string> Headers { get; set; }

        public string Response { get; set; }
        public string Oauth_token { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Grant_type { get; set; }
        public WebProxy Proxy { get; set; }

        public GetRequest(string address)
        {
            _address = address;
            Headers = new Dictionary<string, string>();
        }

        public void Run(CookieContainer cookieContainer)
        {
            _request = (HttpWebRequest)WebRequest.Create(_address);
            _request.Method = "Get";
            _request.CookieContainer = cookieContainer;
            _request.Proxy = Proxy;
            _request.Headers.Add("oauth_token", Oauth_token);
            _request.Headers.Add("username", Username);
            _request.Headers.Add("password", Password);
            _request.Headers.Add("grant_type", Grant_type);
           
            foreach (var pair in Headers)
            {
                _request.Headers.Add(pair.Key, pair.Value);
            }

            try
            {
                HttpWebResponse response = (HttpWebResponse)_request.GetResponse();
                var stream = response.GetResponseStream();
                if (stream != null) Response = new StreamReader(stream).ReadToEnd();
            }
            catch (Exception)
            {
            }
        }

    }
}
