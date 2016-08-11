using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QUERION.Common.Authorization;
using QUERION.Common.Helpers;
using QUERION.Models.Model;

namespace QUERION.Filters
{
    public class QuerionAuthentication
    {       
            private readonly AuthorizationContext _filterContext;

            public QuerionAuthentication(AuthorizationContext filterContext)
            {
                _filterContext = filterContext;
            }

            public void Authorize()
            {
                if (_filterContext.HttpContext.Session == null)
                    return;
                var sessionInfo = _filterContext.HttpContext.Session["USER_SESSION_INFORMATION"] as SessionInformation;
                Uri url = _filterContext.HttpContext.Request.UrlReferrer;
                if (sessionInfo == null)
                {
                    string cookieName = FormsAuthentication.FormsCookieName;
                    HttpCookie authCookie = _filterContext.HttpContext.Request.Cookies[cookieName];
                    if (authCookie == null)
                    {
                        _filterContext.Result = new HttpUnauthorizedResult();
                    }
                    else
                    {
                        FormsAuthenticationTicket cookie = FormsAuthentication.Decrypt(authCookie.Value);
                        JObject cookieInfo = JObject.Parse(cookie.Name);
                        var encryptionHelper = new EncryptionHelper();
                        var email = (string)cookieInfo["Email"];
                        string password = encryptionHelper.Decrypt((string)cookieInfo["Password"]);

                        var client = new WebClient();
                        string webApi = ConfigurationManager.AppSettings["WebAPI"];
                        client.BaseAddress = new Uri(webApi).ToString();
                        string strapi = "api/Account/AuthenticateUser?userName=" + email + "&password=" + password;
                        var userModel = JsonConvert.DeserializeObject<User>(client.DownloadString(strapi));

                        if (!userModel.IsActive)
                        {
                            FormsAuthentication.SignOut();
                            _filterContext.Result = new HttpUnauthorizedResult();
                        }
                        else
                        {
                            sessionInfo = new SessionInformation
                            {
                                UserId = userModel.UserId,
                                UserRole = UserRoleHelper.GetUserRole(userModel.UserRole.RoleName),
                                UserName = userModel.FName,
                               
                                EmailId = userModel.EmailId,
                               
                                UserRoleId = userModel.SelectedRoleId,
                               
                            };
                            _filterContext.HttpContext.Session.Add("USER_SESSION_INFORMATION", sessionInfo);
                        }
                    }
                }
            }
        }
    }
