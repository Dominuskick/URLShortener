﻿using AutoMapper;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using URLShortener.BLL.DTOs;
using URLShortener.BLL.Services.Implemantations;
using URLShortener.BLL.Services.Interfaces;
using URLShortener.Data;

namespace URLShortener.BLL.Services.Core
{
    public class AuthService : BaseService, IAuthService
    {
        #region Constructor        
        protected RoleManager<ApplicationRole> _roleManager;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration,
            IMapper mapper,
            IUserService us,
            UserDbContext context,
        ILogger<AuthService> logger,
            IHttpContextAccessor hca) : base(userManager, context, mapper, hca, logger, configuration)
        {
            _roleManager = roleManager;
        }
        #endregion

        #region Login
        public async Task<ServiceResponse<UserDTO>> Login(Login model)
        {
            var r = new ServiceResponse<UserDTO>();

            try
            {
                var user = await userManager.FindByNameAsync(model.Username);
                if (user != null && userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) != PasswordVerificationResult.Failed);
                {
                    var dto = await GetUserInfo(user);

                    return r.Ok(dto);
                }
                return r.BadRequest("Either Username or Password is Incorrect");
            }
            catch (Exception ex)
            {
                return r.BadRequest(ex.Message);
            }
        }
        #endregion

        #region Validations
        public async Task<ServiceResponse<UserDTO>> ConfirmEmail(ConfirmInput model)
        {
            var r = new ServiceResponse<UserDTO>();
            try
            {
                var user = await userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                    return r.NotFound("user does not exist");

                // if there are spaces, then replace with a + because JS is being dumb
                model.EmailToken = model.EmailToken.Replace(" ", "+");

                var result = await userManager.ConfirmEmailAsync(user, model.EmailToken);

                if (result.Succeeded)
                {
                    var dto = await GetUserInfo(user);
                    return r.Ok(dto);
                }

                return r.BadRequest("There was a problem with the token.");
            }
            catch (Exception ex)
            {
                return r.BadRequest(ex.Message);
            }
        }

        public async Task<ServiceResponse<UserDTO>> ConfirmPassword(ConfirmInput model)
        {
            var r = new ServiceResponse<UserDTO>();
            try
            {
                var user = await userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                    return r.NotFound("user does not exist");

                // if there are spaces, then replace with a + because JS is being dumb
                model.PasswordToken = model.PasswordToken.Replace(" ", "+");

                // update the password
                var result = await userManager.ResetPasswordAsync(user, model.PasswordToken, model.Password);
                if (!result.Succeeded)
                    return r.BadRequest(result.Errors);

                // if we have an email token, try to validate that too
                user.EmailConfirmed = true;
                await userManager.UpdateAsync(user);

                var dto = await GetUserInfo(user);
                return r.Ok(dto);
            }
            catch (Exception ex)
            {
                return r.BadRequest($"Error: {ex.Message}");
            }
        }

        public async Task<GenericResponse> ResetPassword(string email)
        {
            var r = new GenericResponse();

            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
                return r.NotFound("user not found with that email");

            return r.Ok();
        }
        #endregion

        #region Helpers
        private async Task<UserDTO> GetUserInfo(ApplicationUser user)
        {
            try
            {

                // get user roles
                user.UserRoles = db.UserRoles
                .Include(o => o.Role)
                                    .Where(o => o.UserId == user.Id).ToList();

                // send down user info including the new JWT token
                var dto = mapper.Map<UserDTO>(user);
                dto.Token = GetToken(user);

                // update user's lastlogin date
                await userManager.UpdateAsync(user);

                return dto;
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return null;
            }
        }

        private string GetToken(ApplicationUser user)
        {
            var authClaims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            // get security key
            var appSettingsSection = config.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            // create a signing key from the app settings
            var authSigningKey = new SymmetricSecurityKey(key);

            // generate JWT token
            var token = new JwtSecurityToken(
                //issuer: "http://dotnetdetail.net",
                //audience: "http://dotnetdetail.net",
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            // for debugging
            // IdentityModelEventSource.ShowPII = true;

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}
