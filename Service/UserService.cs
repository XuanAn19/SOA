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

				var verifyPassword = Authentication.VerifyPassword(password, user.Password.ToString());
				if (verifyPassword == true)
				{
					//create token
					var claims = new[]{
									new Claim(JwtRegisteredClaimNames.Sub, _config["jwt:subject"]),
									new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
									new Claim("IdUser",user.IdUser.ToString()),
									new Claim("Email",user.Email.ToString()),

							 };
					var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["jwt:key"]));
					var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
					var token = new JwtSecurityToken(
						_config["jwt:issuer"],
						_config["jwt:audience"],
						claims,
						expires: DateTime.UtcNow.AddMinutes(60),
						signingCredentials: signIn
						);
					string accesstoken = new JwtSecurityTokenHandler().WriteToken(token);
					
					var login_data = new
					{
						status = "ok",
						message = "Login success",
						token = accesstoken,
						email = user.Email
					};
					return login_data;
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
					message = "email is not exists"
				};
			}
		}
	}
}
