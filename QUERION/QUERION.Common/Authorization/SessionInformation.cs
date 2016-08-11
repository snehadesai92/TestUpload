using System;
using QUERION.Common.Enum;

namespace QUERION.Common.Authorization
{
    public class SessionInformation
    {
        public Int64 UserId { get; set; }
        public string UserName { get; set; }       
        public Int64 UserRoleId { get; set; }
        public UserRoleEnum UserRole { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
       
    }
}