using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLShortener.BLL.DTOs;

namespace URLShortener.BLL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<UserDTO>> Login(Login model);
        Task<ServiceResponse<UserDTO>> ConfirmEmail(ConfirmInput model);
        Task<ServiceResponse<UserDTO>> ConfirmPassword(ConfirmInput model);
        Task<GenericResponse> ResetPassword(string email);
    }
}
