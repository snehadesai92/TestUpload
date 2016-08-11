using System;
using QUERION.Common.Helper;
using QUERION.Core.DBClass;
using QUERION.Models.Model;


namespace QUERION.BAL
{
    public class LoginBal
    {
        #region PrivateData

        private LoginRepository objLoginRepository;

        #endregion

        #region Login

        public ResponseData<User> AuthenticateUser(string userName, string password)
        {
            ResponseData<User> objResponceData=new ResponseData<User>();
            try
            {
                objResponceData=objLoginRepository.Login(userName, password);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return objResponceData;
        }

        #endregion

        #region Forgot Password

        public ResponseData<User> CheckUserExist(string email)
        {
            ResponseData<User> objResponseData;
            try
            {
                objLoginRepository=new LoginRepository();
                objResponseData = objLoginRepository.CheckUserExist(email);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return objResponseData;
        }

        public bool ResetPassword(int userId, string password)
        {
            bool status = false;
            try
            {
                LoginRepository objRepository = new LoginRepository();
                status = objRepository.ResetPassword(userId, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }

        #endregion

        #region ChangePassword

        public bool VerifyOldPassword(int userId, string password)
        {
            bool status = false;
            try
            {
                status=objLoginRepository.VerifyOldPassword(userId, password);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return status;
        }

        #endregion

    }
}
