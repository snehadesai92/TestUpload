using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QUERION.Models.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public DateTime DOB { get; set; }
        public bool Gender { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
    }
}

