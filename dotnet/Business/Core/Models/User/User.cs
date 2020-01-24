using AndcultureCode.CSharp.Core.Models;

namespace Sandbox.Business.Core.Models.Users
{
    public class User : Auditable
    {
        public string Username  { get; set; }
        public string Password  { get; set; }
        public string FirstName { get; set; }
        public string LastName  { get; set; }
    }
}
