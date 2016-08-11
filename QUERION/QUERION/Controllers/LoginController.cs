using System.Configuration;
using System.Text;
using QUERION.Common.Authorization;
using Newtonsoft.Json;
using QUERION.BAL;
using QUERION.Common.Authorization;
using QUERION.Common.Enum;
using QUERION.Common.Helper;
using QUERION.Common.Helpers;
using QUERION.Models.Model;
using QUERION.Filters;
using QUERION.Models;
using System;
using  QUERION.Common;
using System.Web.Mvc;
using System.Web.Security;


namespace QUERION.Controllers
{
    public class LoginController : Controller
    {

        #region private data
        public string UrlAfterLogin = null;
        private LoginBal objLoginBal;
        private EncryptionHelper objHelper;
        #endregion private data

        #region Login Module
        // GET: /Login/
        public ActionResult Login(string returnUrl)
        {
         try
            {               
                ViewBag.ReturnUrl = returnUrl;
                var filterContext = new AuthorizationContext(ControllerContext);
                var querionAuthentication = new QuerionAuthentication(filterContext);
                querionAuthentication.Authorize();
                SessionInformation sessionInformation = SessionManager.GetSessionInformation();
                if (sessionInformation != null)
                {
                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    if (SessionManager.GetUserRoleOfCurrentlyLoggedInUser().Equals(UserRoleEnum.Admin))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                   
                    if (SessionManager.GetUserRoleOfCurrentlyLoggedInUser().Equals(UserRoleEnum.NormalUser))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    UrlAfterLogin = Request.QueryString["ReturnUrl"];
                    var loginModel = new LoginModel();
                    return PartialView(loginModel);
                }
            }
            catch (Exception ex)
            {              
                return null;
            }
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                Uri url = HttpContext.Request.UrlReferrer;
                objHelper=new EncryptionHelper();
                model.Password = objHelper.Encrypt(model.Password);
                objLoginBal=new LoginBal();
                ResponseData<User> objResponseData = objLoginBal.AuthenticateUser(model.UserName, model.Password);

                if (objResponseData.Status)
                {
                    var userModel = objResponseData.Model;
                    if (SessionManager.GetSessionInformation() == null)
                    {
                        if (userModel != null)
                        {
                            if (userModel.IsActive)
                            {
                                var sessionInfo = new SessionInformation
                                {
                                    UserId = userModel.UserId,
                                    UserRoleId =userModel.RoleId,
                                    EmailId = userModel.Email,
                                    Password = userModel.Password,
                                    UserName = userModel.FName +userModel.LName
                                };
                                SessionManager.SetSessionInformation(sessionInfo);
                                if (model.RememberMe)
                                {
                                    var cookieInformation = new CookieInformation
                                    {
                                        Email = model.UserName,
                                        Password = model.Password
                                    };
                                    string cookie = JsonConvert.SerializeObject(cookieInformation);
                                    FormsAuthentication.SetAuthCookie(cookie, model.RememberMe);
                                }
                            
                                if (!string.IsNullOrEmpty(url.Query))
                                {
                                    return Redirect(returnUrl);
                                }
                                return RedirectToAction("Index", "Home");
                            }
                            Session["LOGIN_FAILED"] = "Oops!!! Your account is not active. Please Contact Admin.";
                            return View(model);
                        }
                        Session["LOGIN_FAILED"] = "Oops!!! Invalid username or password, Please Try Again.";
                    }                   
                }
                else
                {
                    SessionManager.RemoveSessionInformation();
                }
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }
            catch (Exception ex)
            {
                return View("Login");
            }
        }


        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]//need to remove because it is giving error on login after clearing cache.
        //public ActionResult Login(LoginModel model, string returnUrl)
        //{
        //    if (ModelState.IsValid && Membership.ValidateUser(model.UserName, model.Password) == true)
        //    {
        //       // Login1.Visible = true;
        //        Session["user"] = User.Identity.Name;
        //        FormsAuthentication.RedirectFromLoginPage(model.UserName, true);
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "The user name or password provided is incorrect.");
        //    }

        //   // MembershipUser user = Membership.GetUser("John Hoube");
        //   // string pass = user.GetPassword();
        //   //// user.ChangePassword(user.GetPassword(), newpw);

        //   // // If we got this far, something failed, redisplay form
        //    ModelState.AddModelError("", "The user name or password provided is incorrect.");
        //    return View(model);
        //}

        #endregion  Login Module

        #region Forgot Password

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var objEncryptionHelper = new EncryptionHelper();
               
               
                bool isEmailSend = false;
                if (!string.IsNullOrEmpty(model.Email))
                {
                    objLoginBal=new LoginBal();
                    ResponseData<User> objResponseData = objLoginBal.CheckUserExist(model.Email);

                    if (objResponseData.Model != null && objResponseData.Model.UserId != 0)
                    {
                        #region Send Reset Password Email to User
                        const string mailSubject = "Querion Team :: Reset Password";
                        string mainUrl = ConfigurationManager.AppSettings["MainURL"];
                        var sbMailBody = new StringBuilder();
                        sbMailBody.Append("Hi " + objResponseData.Model.FName + " " + objResponseData.Model.LName + ",");
                        sbMailBody.Append(" <br /> Reset your password. !!!");
                        sbMailBody.Append("<br /><br /> Please reset your password and activate you'r account <a href='" + mainUrl + "Login/ResetPassword?userId=" + objEncryptionHelper.Encrypt(Convert.ToString(objResponseData.Model.UserId)) + "' target='_blank'>" + "Reset Password" + "<a/> .");
                        sbMailBody.Append("<br /><br /> ");
                        sbMailBody.Append("<strong> Thanks & Regards, <br /> Querion Team </strong>");
                        sbMailBody.Append("<br /><br /> ");
                        //Send Reset Password Email to User
                        isEmailSend = EmailHelper.EmailComposeActionsAndSendEmail(model.Email, sbMailBody.ToString(), mailSubject, null);
                        if (isEmailSend == true)
                        {
                            return RedirectToAction("ForgotPasswordConfirmation");
                        }
                        else
                        {
                            //return RedirectToAction("ErrorOnPage", "Check your Internet connection, Failed to send email");
                            ModelState.AddModelError("", "Failed to send emails");
                            return View();
                        }
                        #endregion
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email Does Not Exists. Please Enter Valid Email Address");
                        return View();
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Login/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        //
        // GET: /Login/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string userId)
        {
            var objResetPasswordViewModel = new ResetPasswordViewModel();
            var objEncryptionHelper = new EncryptionHelper();
            objResetPasswordViewModel.UserId = Convert.ToInt32(objEncryptionHelper.Decrypt(userId));
            if (userId == null)
            {
                return null;
            }
            else
            {
                return View(objResetPasswordViewModel);
            }
        }

        
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var objEncryptionHelper = new EncryptionHelper();               
                model.Password = objEncryptionHelper.Encrypt(model.Password);
                objLoginBal=new LoginBal();
                var response = objLoginBal.ResetPassword(model.UserId, model.Password);

                if (response)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }
                else
                    return null;

            }
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #endregion Forgot Password

        #region Change Password
       
        [AllowAnonymous]
        public ActionResult ChangePassword(int userId)
        {
            var objChangePasswordViewModel = new ChangePasswordViewModel();         
            return View(objChangePasswordViewModel);
        }

        [HttpPost]
        public ActionResult VerifyOldPassword(int userId, string password)
        {
            if (ModelState.IsValid)
            {
                var objChangePasswordViewModel = new ChangePasswordViewModel();
                objChangePasswordViewModel.UserId = userId;
                var objEncryptionHelper = new EncryptionHelper();
                password = objEncryptionHelper.Encrypt(password);

               objLoginBal=new LoginBal();
               var response= objLoginBal.VerifyOldPassword(userId, password);
               
                if (response)
                {                   
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            return View();
        }
      
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var objEncryptionHelper = new EncryptionHelper();
                model.NewPassword = objEncryptionHelper.Encrypt(model.NewPassword);
                var response=objLoginBal.ResetPassword(model.UserId, model.NewPassword);

                if (response)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }
                else
                    return null;
            }
            return View();
        }


        #endregion
    }
}
