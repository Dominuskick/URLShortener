using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLShortener.BLL.DTOs
{
    public class UserSimpleDTO
    {
        // basic user props
        public Guid Id { get; set; }
        public Guid ContactId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UserDTO : UserSimpleDTO
    {
        public ICollection<RoleDTO> UserRoles { get; set; }

        // jwt token
        public string Token { get; set; }
        public ICollection<string> Claims { get; internal set; }
    }

    public class UserInput : UserSimpleDTO
    {
        public string Password { get; set; }
        public bool SendEmail { get; set; }
        public Guid? AppId { get; set; }
        public int? SubscriptionTypeId { get; set; }
    }

    public class ConfirmInput
    {
        public Guid UserId { get; set; }
        public string EmailToken { get; set; }
        public string PasswordToken { get; set; }
        public string Password { get; set; }
    }


    public class Login
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
