using AutoMapper;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using URLShortener.BLL.DTOs;
using URLShortener.BLL.Helpers;
using URLShortener.BLL.Services.Interfaces;
using URLShortener.DAL.Components;
using URLShortener.Data;

namespace URLShortener.BLL.Services.Implemantations
{
    public class UserService : BaseService, IUserService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserService(IConfiguration configuration,
            UserDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IMapper mapper,
            ILogger<UserService> logger,
            IHttpContextAccessor hca) : base(userManager, context, mapper, hca, logger, configuration)
        {
            _roleManager = roleManager;
        }

        public Task<GenericResponse> AddRoleToUser(Guid userid, UserDTO req)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<Guid>> AddUser(UserInput dto)
        {
            var serviceResponse = new ServiceResponse<Guid>();

            try
            {
                var u = mapper.Map<ApplicationUser>(dto);
                u.PhoneNumber = u.PhoneNumber.ToPhone();

                var needsPassword = string.IsNullOrEmpty(dto.Password);

                // if we're not asking for a password, make one on the fly
                if (needsPassword)
                    dto.Password = SecurityHelper.GeneratePassword();

                var res = await userManager.CreateAsync(u, dto.Password);

                if (!res.Succeeded)
                    return serviceResponse.BadRequest(res.Errors);

                db.SaveChanges();

                return serviceResponse.Ok();
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                return serviceResponse.BadRequest(ex.Message);
            }
        }

        public Task<GenericResponse> DeleteRoleFromUser(Guid userid, Guid roleid)
        {
            throw new NotImplementedException();
        }

        public Task<GenericResponse> DeleteUser(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Claim>> GetRoleClaimsAsync(IList<string> roles)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<UserDTO>> GetUser(Guid id)
        {
            var serviceResponse = new ServiceResponse<UserDTO>();

            try
            {
                var u = await userManager.Users
                                .FirstOrDefaultAsync(o => o.Id == id);

                if (u == null)
                    return serviceResponse.NotFound("user not found");

                var dto = mapper.Map<UserDTO>(u);

                return serviceResponse.Ok(dto);
            }
            catch (Exception ex)
            {
                return serviceResponse.BadRequest(ex.Message);
            }
        }

        public ServiceResponse<IEnumerable<UserDTO>> GetUserRoles(Guid userid)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<UserDTO>>();

            var u = userManager.Users
                            .Include(o => o.UserRoles)
                                .ThenInclude(o => o.Role)
                            .FirstOrDefault(o => o.Id == userid);

            if (u == null)
                return serviceResponse.NotFound("user not found");

            var dto = mapper.Map<IEnumerable<UserDTO>>(u.UserRoles);

            return serviceResponse.Ok(dto);
        }

        public Task<ServiceResponse<UserDTO>> UpdateUser(string id, UserSimpleDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
