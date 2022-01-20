using Leson.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Leson.Controllers
{
    public class HomeController : Controller
    {

        public string GetRequestToken(string url) {
            string str;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var stream = response.GetResponseStream();
            string resp = new StreamReader(stream).ReadToEnd();
            dynamic json = JObject.Parse(resp);
            str = json.RequestToken;
            return str;
        }

        public string GetAccesToken(string url, string reqToken)
        {
            string str;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Host = "api.pixlpark.com";
            request.Referer = "http://api.pixlpark.com/account/login";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.85 YaBrowser/21.11.4.727 Yowser/2.5 Safari/537.36";
            string pass = reqToken + "8e49ff607b1f46e1a5e8f6ad5d312a80";
            var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(sha1.ComputeHash(Encoding.UTF8.GetBytes(pass)));
            var sb = new StringBuilder();     
            string pass1 = sb.ToString();
            var proxy = new WebProxy("127.0.0.1:8888");
            request.Proxy = proxy;
            var cookieContainer = new CookieContainer();
            Cookie ck = new Cookie("oauth_token", reqToken);
            Cookie ck1 = new Cookie("grant_type", "api");
            Cookie ck2 = new Cookie("username", "38cd79b5f2b2486d86f562e3c43034f8");
            Cookie ck3 = new Cookie("password", pass1);
            ck.Domain = request.Host;
            ck1.Domain = request.Host;
            ck2.Domain = request.Host;
            ck3.Domain = request.Host;
            var ckCon = new CookieContainer();
            ckCon.Add(ck);
            ckCon.Add(ck1);
            ckCon.Add(ck2);
            ckCon.Add(ck3);
            request.CookieContainer = ckCon;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var stream = response.GetResponseStream();
            string resp = new StreamReader(stream).ReadToEnd();
            dynamic json = JObject.Parse(resp);
            str = json.RequestToken;
            return str;
        }


        public IActionResult Index()
        {

            return View();
        }
        public ViewResult Parse() {
            SiteResponse siteResp = new SiteResponse();
            siteResp.reqtoken = GetRequestToken("http://api.pixlpark.com/oauth/requesttoken");
            

            siteResp.accestoken = GetAccesToken("http://api.pixlpark.com/oauth/accesstoken",siteResp.reqtoken);
            return View();        
        }
        [HttpGet]
        public ViewResult RsvpForm()
        {
            return View();
        }

        [HttpPost]
        public ViewResult RsvpForm(GuestResponse guest)
        {
            if (ModelState.IsValid)
                // Нужно отправить данные нового гостя по электронной почте 
                // организатору вечеринки.
                return View("Thanks", guest);
            else
                // Обнаружена ошибка проверки достоверности
                return View();
        }
    }
}
