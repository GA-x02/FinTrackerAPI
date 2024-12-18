using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Infrastructure.Data;
using FinTracker.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationApi.Infrastructure.Repositories
{
    public class AppUserRepository(AuthenticationDbContext context, IConfiguration config) : IAppUser
    {
        private async Task<AppUser> GetUserByEmail(string email)
        {
            try
            {
                var user = await context.AppUsers.FirstOrDefaultAsync(x => x.Email == email);
                return user!;
            }
            catch (Exception)
            {
                throw new Exception("Error occurred retrieving user by email");
            }
        }

        public async Task<GetAppUserDTO> GetAppUser(Guid userId)
        {
            try
            {
                var user = await context.AppUsers.FindAsync(userId);
                return user is null ? null! : new GetAppUserDTO(
                    user.Id,
                    user.Name!,
                    user.Email!,
                    user.TelephoneNumber!,
                    user.UserName!,
                    user.Role!);
            }
            catch (Exception)
            {
                throw new Exception("Error occurred retrieving user by id");
            }
        }

        public async Task<Response> Login(LoginDTO loginDTO)
        {
            try
            {
                var getUser = await GetUserByEmail(loginDTO.Email);
                if (getUser is null)
                {
                    return new Response(false, "User with this email was not found");
                }

                bool verifyPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, getUser.Password);
                if (!verifyPassword)
                {
                    return new Response(false, "Password is incorrect");
                }

                return new Response(true, GenerateToken(getUser));
            }
            catch (Exception)
            {
                throw new Exception("Error occurred login user");
            }
        }

        private string GenerateToken(AppUser appUser)
        {
            var key = Encoding.UTF8.GetBytes(config.GetSection("Authentication:Key").Value!);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new (ClaimTypes.Name, appUser.Name!),
                new (ClaimTypes.Email, appUser.Email!)
            };

            if (!string.IsNullOrEmpty(appUser.Role) || !Equals("string", appUser.Role))
            {
                claims.Add(new(ClaimTypes.Role, appUser.Role!));
            }

            var token = new JwtSecurityToken(
                issuer: config["Authentication:Issuer"],
                audience: config["Authentication:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Response> Register(AppUserDTO appUserDTO)
        {
            try
            {
                var getUser = await GetUserByEmail(appUserDTO.Email);
                if (getUser != null)
                {
                    return new Response(false, "Email already exist");
                }
                var result = context.AppUsers.Add(new AppUser()
                {
                    Id = appUserDTO.Id,
                    Name = appUserDTO.Name,
                    Email = appUserDTO.Email,
                    TelephoneNumber = appUserDTO.TelephoneNumber,
                    UserName = appUserDTO.UserName,
                    Role = appUserDTO.Role,
                    Password = BCrypt.Net.BCrypt.HashPassword(appUserDTO.Password)
                });
                await context.SaveChangesAsync();
                return new Response(true, "User registered successfully");
            }
            catch (Exception)
            {
                throw new Exception("Error occurred register user");
            }
        }
    }
}
