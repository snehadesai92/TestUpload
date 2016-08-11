using System;
using System.Web;
using System.Web.Mvc;
using QUERION.Common.Authorization;
using QUERION.Common.Enum;

namespace QUERION.Filters
{
    public class AdminAuthFilterAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            SessionInformation sessionInfo = SessionManager.GetSessionInformation();

            if (sessionInfo != null && sessionInfo.UserRoleId.Equals(Convert.ToInt32(UserRoleEnum.Admin)))
            {
            }
            else
            {
                filterContext.Result = new HttpUnauthorizedResult();
                HttpContext.Current.Response.Redirect("~/Login/Login", true);
            }
        }
    }

}