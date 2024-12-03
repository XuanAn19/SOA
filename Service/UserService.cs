using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.IdentityModel.Tokens;
using SOA.DTOs;
using SOA.Middleware;
using SOA.Models;
using SOA.Repository.Interface;
using SOA.Service.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SOA.Service
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IConfiguration _config;

		public UserService(IUserRepository userRepository, IConfiguration config)
		{
			_userRepository = userRepository;
			_config = config;
		}

		public async Task RegisterUser(UserDTO user)
		{
			var checkuser = await _userRepository.GetUserByEmail(user.Email);
			if (checkuser != null)
			{
				throw new InvalidOperationException("Email đã được đăng kí.");
			}
			var hashPassword = Authentication.GetSha256Hash(user.Password);
			var usernew = new UserModel
			{
				Email = user.Email,
				Password = hashPassword,
			};

			await _userRepository.AddUser(usernew);

		}
        public async Task<object> UserLogin(string email, string password)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user != null)
            {
                // Xác minh mật khẩu
                var verifyPassword = Authentication.VerifyPassword(password, user.Password.ToString());
                if (verifyPassword == true)
                {
                    // Khai báo claims
                    var claims = new[]
                    {
                new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("IdUser", user.IdUser.ToString()), // Thông tin user
                new Claim("Email", user.Email)               // Thông tin email
            };

                    // Tạo key và credentials
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    // Tạo token
                    var token = new JwtSecurityToken(
                        issuer: _config["Jwt:Issuer"],        // Phát hành từ
                        audience: _config["Jwt:Audience"],    // Người nhận hợp lệ
                        claims: claims,                       // Đính kèm thông tin user
                        expires: DateTime.UtcNow.AddMinutes(60), // Thời gian hết hạn
                        signingCredentials: signIn            // Cách ký
                    );

                    // Kết quả
                    string accessToken = new JwtSecurityTokenHandler().WriteToken(token);
                    var loginData = new
                    {
                        status = "ok",
                        message = "Login success",
                        token = accessToken,    // Trả về token
                        email = user.Email
                    };

                    return loginData;
                }
                else
                {
                    return new
                    {
                        status = "no",
                        message = "Incorrect password"
                    };
                }
            }
            else
            {
                return new
                {
                    status = "no",
                    message = "Email does not exist"
                };
            }
        }

    }
}
