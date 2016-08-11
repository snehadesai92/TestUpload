using System;
using System.Data;
using System.Data.SqlClient;
using QUERION.Common;
using QUERION.Common.Helper;
using QUERION.Models.Model;


namespace QUERION.Core.DBClass
{
    public class LoginRepository
    {
        #region Private Data
        private SqlConnection _connection;
        private SqlCommand _command;
        private SqlDataReader _reader;
        #endregion

        #region Login
        public ResponseData<User> Login(string userName,string password)
        {
            ResponseData<User> responseData;
            User user = new User();
            try
            {
                _connection = new SqlConnection(ApiUtility.ConnectionString);
                _connection.Open();
                _command = new SqlCommand("spt_LoginUser", _connection)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 6000
                };
                _command.Parameters.AddWithValue("@UserName", userName);
                _command.Parameters.AddWithValue("@Password", password);

                _reader = _command.ExecuteReader();
              
                if (_reader.HasRows)
                {
                    if (_reader != null)
                    {                     
                        while (_reader.Read()) // Read data from database
                        {
                            user.UserId = Convert.ToInt32(_reader["UserID"]);
                            user.FName = Convert.ToString(_reader["FName"]);
                            user.LName = Convert.ToString(_reader["LName"]);                          

                            if (!DBNull.Value.Equals(_reader["DOB"]))
                                user.DOB = Convert.ToDateTime(_reader["DOB"]);

                            if (!DBNull.Value.Equals(_reader["Gender"]))
                                user.Gender = Convert.ToBoolean(_reader["Gender"]);                         

                            if (!DBNull.Value.Equals(_reader["EmailId"]))
                                user.Email = Convert.ToString(_reader["EmailId"]);

                            if (!DBNull.Value.Equals(_reader["Password"]))
                                user.Password = Convert.ToString(_reader["Password"]);

                            if (!DBNull.Value.Equals(_reader["MobileNo"]))
                                user.Mobile = Convert.ToString(_reader["MobileNo"]);                           
                           

                            if (!DBNull.Value.Equals(_reader["Address"]))
                                user.Address = Convert.ToString(_reader["Address"]);
                           
                            if (!DBNull.Value.Equals(_reader["RoleID"]))
                                user.RoleId = Convert.ToInt32(_reader["RoleID"]);

                            if (!DBNull.Value.Equals(_reader["IsActive"]))
                                user.IsActive = Convert.ToBoolean(_reader["IsActive"]);    
                          
                        }
                        _reader.Close();                      
                    }
                }               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_connection != null)
                {
                    _connection.Close();
                }
            }
            responseData = new ResponseData<User>()
            {
                Status = user != null,
                Model = user != null ? user : null,
                Errormessage = user != null ? null : "Error in UserRepository"
            };

            return responseData;

        }
        #endregion

        #region Forgot Password

        public ResponseData<User> CheckUserExist(string email)
        {
            ResponseData<User> objResponseData;
         var user = new User();
         try
         {
             _connection = new SqlConnection(ApiUtility.ConnectionString);
             _connection.Open();
             _command = new SqlCommand("spt_IsEmailExist", _connection)
             {
                 CommandType = CommandType.StoredProcedure,
                 CommandTimeout = 6000
             };
             _command.Parameters.AddWithValue("@Email", email);
            
             _reader = _command.ExecuteReader();
            
             if (_reader.HasRows)
             {
                 while (_reader.Read()) // Read data from database
                 {
                     if (!DBNull.Value.Equals(_reader["UserID"]))
                         user.UserId = Convert.ToInt32(_reader["UserID"]);

                     if (!DBNull.Value.Equals(_reader["FName"]))
                         user.FName = Convert.ToString(_reader["FName"]);

                     if (!DBNull.Value.Equals(_reader["LName"]))
                         user.LName = Convert.ToString(_reader["LName"]);                  

                     if (!DBNull.Value.Equals(_reader["DOB"]))
                         user.DOB = Convert.ToDateTime(_reader["DOB"]);

                     if (!DBNull.Value.Equals(_reader["Gender"]))
                         user.Gender = Convert.ToBoolean(_reader["Gender"]);

                     if (!DBNull.Value.Equals(_reader["EmailId"]))
                         user.Email = Convert.ToString(_reader["EmailId"]);

                     if (!DBNull.Value.Equals(_reader["Address"]))
                         user.Address = Convert.ToString(_reader["Address"]);
                 }
                 _reader.Close();
                
             }
           
         }
         catch (Exception ex)
         {
             throw ex;
         }
         finally
         {
             if (_connection != null)
             {
                 _connection.Close();
             }
         }

            objResponseData = new ResponseData<User>()
            {
                Status = user != null,
                Model = user != null ? user : null,
                Errormessage = user != null ? null : "Error in checkemailexit"
            };

            return objResponseData;

        }

        public bool ResetPassword(int userId, string password)
        {
            var status = false;
            var noOfRowAffect = 0;
            try
            {
                _connection = new SqlConnection(ApiUtility.ConnectionString);
                _connection.Open();
                _command = new SqlCommand("spt_ResetPassword", _connection)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 6000
                };
                _command.Parameters.Add("@userId", userId);
                _command.Parameters.Add("@password", password);

                noOfRowAffect = _command.ExecuteNonQuery();

                if (noOfRowAffect >= 0)
                {
                    status = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_connection != null)
                {
                    _connection.Close();
                }
            }
            return status;
        }    

        #endregion

        #region ChangePassword

        public bool VerifyOldPassword(int userId, string password)
        {
            var status = false;
            try
            {
                _connection = new SqlConnection(ApiUtility.ConnectionString);
                _connection.Open();
                _command = new SqlCommand("spt_GetPasswordByUserId", _connection)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 6000
                };
                _command.Parameters.Add("@userId", userId);
                _command.Parameters.Add("@oldPassword", password);

                var data = _command.ExecuteScalar();

                if (data != null)
                {
                    status = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_connection != null)
                {
                    _connection.Close();
                }
            }

            return status;
        }

        #endregion

    }
}
