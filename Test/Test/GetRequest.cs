using Microsoft.Web.WebPages.OAuth;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using xNet;

namespace Test
{
    class GetRequest
    {
        HttpWebRequest _request;
        string _address;
        string _token;
        string _secret;
        string _id;
        public string requesttoken { get; set; }
        public string Response { get; set; }


        public GetRequest(string address,string token,string id,string secret)
        {
            _address = address;
            _token = token;
            _id = id;
            _secret = secret;
        }

        public void Run()
        {
            var client = new RestClient(_address);
            var request = new RestRequest(Method.GET);
            request.AddHeader("oauth_token", _token);
            request.AddHeader("Id", "1");
            IRestResponse response = client.Execute(request);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials&scope=all&client_id=" + _id + "&client_secret=" + _secret, ParameterType.RequestBody);
            string s = response.Content;
            Console.WriteLine(s);
            dynamic resp = JObject.Parse(response.Content);
            //Console.WriteLine(resp);

            
            
        }
    }
}
