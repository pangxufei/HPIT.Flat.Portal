using HPIT.Evalute.Data;
using HPIT.Evalute.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Web.Security;

namespace HPIT.Flat.Portal.Controllers
{
    public class LoginController : Controller
    {
        /// <summary>
        /// 允许匿名登录
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        // GET: Login
        public JsonResult LoginIn(string username, string password)
        {
            List<HPITMemberInfo> users = EvaluteDal.Instance.LoginMember(username,password);
            JsonResult jsonResult = new JsonResult();
            //如果找到的话，就添加到缓存当中，并且跳转到主页面
            if (users != null && users.Count >=1)
            {
                string json = JsonConvert.SerializeObject(users.FirstOrDefault());
                //
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(2, "loginUser", DateTime.Now, DateTime.Now.AddDays(1), false, json);
                //加密
                var ticketEncrypt = FormsAuthentication.Encrypt(ticket);
                //添加到cookie
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,ticketEncrypt);
                //过期时间
                cookie.Expires = DateTime.Now.AddHours(12);
                //域
                cookie.Domain = FormsAuthentication.CookieDomain;
                //http协议
                cookie.HttpOnly = true;
                //ssl  安全套接字
                cookie.Secure = FormsAuthentication.RequireSSL;
                // / cookie浏览器的存储路径
                cookie.Path = FormsAuthentication.FormsCookiePath;

                //HttpCookie cokie = new HttpCookie("login",Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json)));
                //Response.Cookies.Add(cokie);
                //先删除
                Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
                //添加
                Response.Cookies.Add(cookie);
                jsonResult.Data = new { data = json, state = "200" };
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jsonResult;
            }
            else
            {
                jsonResult.Data = new { data = "未找到用户!", state = "403" };
                return jsonResult;
            }
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return View("Index");
        }
    }
}