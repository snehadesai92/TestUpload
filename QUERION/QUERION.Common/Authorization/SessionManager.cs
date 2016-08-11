using System;
using System.Web;
using QUERION.Common.Enum;


namespace QUERION.Common.Authorization
{
    public static class SessionManager
    {
        public static void SetSessionInformation(SessionInformation sessionInformation)
        {
            if (HttpContext.Current.Session["USER_SESSION_INFORMATION"] == null)
            {
                HttpContext.Current.Session.Add("USER_SESSION_INFORMATION", sessionInformation);
            }
        }

        public static void RemoveSessionInformation()
        {
            HttpContext.Current.Session["USER_SESSION_INFORMATION"] = null;
        }

        public static SessionInformation GetSessionInformation()
        {
            if (HttpContext.Current.Session["USER_SESSION_INFORMATION"] as SessionInformation != null)
            {
                var session = HttpContext.Current.Session["USER_SESSION_INFORMATION"] as SessionInformation;
                return session;
            }
            return null;
        }

        public static Int64 GetCurrentlyLoggedInUserId()
        {
            Int64 loggedInUserId = 0;

            if (GetSessionInformation() == null)
            {
                return loggedInUserId;
            }
            loggedInUserId = GetSessionInformation().UserId;
            return loggedInUserId;
        }

        public static UserRoleEnum? GetUserRoleOfCurrentlyLoggedInUser()
        {
            if (HttpContext.Current.Session["USER_SESSION_INFORMATION"] as SessionInformation != null)
            {
                var sessionInfo = HttpContext.Current.Session["USER_SESSION_INFORMATION"] as SessionInformation;
                if (sessionInfo != null)
                    return sessionInfo.UserRole;
            }
            return null;
        }

        public static Int64? GetUserRoleIdOfCurrentlyLoggedInUser()
        {
            if (HttpContext.Current.Session["USER_SESSION_INFORMATION"] as SessionInformation != null)
            {
                var sessionInfo = HttpContext.Current.Session["USER_SESSION_INFORMATION"] as SessionInformation;
                if (sessionInfo != null)
                    return sessionInfo.UserRoleId;
            }
            return null;
        }
    }
}