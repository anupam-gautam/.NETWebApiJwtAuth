using System.Data;
using System.Net.Mail;

namespace JwtWebApiAuth.Models
{
    public class UserConstants
    {
        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel()
            {
                UserName = "anupam_admin",
                EmailAddress = "anupam@yopmail.com",
                FirstName = "Anupam",
                LastName = "Gautam",
                Role = "Admin",
                Password = "test123"
            },
            new UserModel()
            {
                UserName = "Ramesh_editor",
                EmailAddress = "ramesh@yopmail.com",
                FirstName = "Ramesh",
                LastName = "Karki",
                Role = "Editor",
                Password = "test123"
            },
        };
    }
}
