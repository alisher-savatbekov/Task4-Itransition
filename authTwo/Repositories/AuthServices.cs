
using authTwo.Repositories;
using authTwo.Context;
using authTwo.ModelDTO;
using authTwo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace authTwo.Services
{
    public class AuthServices: IAuthRepository
    {
        private readonly EntityConnect _context;
        private readonly IConfiguration _configuration;

        public AuthServices(EntityConnect context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> LoginUserAsync(LoginDTO model)
        {
            try
            {
                var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user == null)
                {
                    return null;
                }

                if (user.IsBlocked)
                {
                    throw new Exception("User is blocked and cannot log in.");
                }

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);

                if (!isPasswordValid)
                {
                    return null;
                }

                
                var token = GenerateJwtToken(user);
                user.LastLoginTime = DateTime.UtcNow.ToString();
                await _context.SaveChangesAsync();
                return token;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Role, user.Role),
        new Claim(type:ClaimTypes.Name,value:user.UserName)
    };

            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecretKeyyyyyyyyyyyyyyyyyy12345"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "MyIssuer",  
                audience: "MyAudience",  
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task RegisterUserAsync(RegisterDTO model)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email || u.UserName == model.UserName);

                if (existingUser != null)
                {

                    if (existingUser.DeletedAt != null)
                    {

                        existingUser.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                        existingUser.RegistrationDate = DateTime.UtcNow.ToString();
                        existingUser.DeletedAt = null;
                        existingUser.IsBlocked = false;

                        _context.Users.Update(existingUser);
                        await _context.SaveChangesAsync();
                        return;
                    }
                    else
                    {
                        throw new InvalidOperationException("Email or UserName is already in use.");
                    }
                }
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                var user = new User
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    Password = hashedPassword,
                    RegistrationDate = DateTime.Now.ToString(),
                    IsBlocked = false
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
