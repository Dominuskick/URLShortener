using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using URLShortener.BLL.DTOs;

namespace URLShortener.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<UserDTO>> GetUser(Guid id);
        Task<ServiceResponse<Guid>> AddUser(UserInput dto);
        Task<ServiceResponse<UserDTO>> UpdateUser(string id, UserSimpleDTO dto);
        Task<GenericResponse> DeleteUser(string id);

        ServiceResponse<IEnumerable<UserDTO>> GetUserRoles(Guid userid);
        Task<GenericResponse> AddRoleToUser(Guid userid, UserDTO req);
        Task<GenericResponse> DeleteRoleFromUser(Guid userid, Guid roleid);
        Task<IList<Claim>> GetRoleClaimsAsync(IList<string> roles);
    }
}
