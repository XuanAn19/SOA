using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace SOA.Middleware
{
	public class Authentication
	{
		private readonly RequestDelegate _next;
		private readonly IConfiguration _config;

		public Authentication(RequestDelegate next, IConfiguration config)
		{
			_next = next;
			_config = config;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
				if (token != null)
				{
					var auth = AttachUserToContext(context, token);
					if (auth)
					{
						await _next(context);
					}
					else
					{
						var myObject = new { status = "no", message = "Token không đúng" };
						var jsonResponse = JsonConvert.SerializeObject(myObject);
						context.Response.StatusCode = 401;
						await context.Response.WriteAsync(jsonResponse);
					}
				}
				else
				{
					var myObject = new { status = "no", message = "Unauthorized: Missing or invalid token." };
					var jsonResponse = JsonConvert.SerializeObject(myObject);
					context.Response.StatusCode = 401;
					await context.Response.WriteAsync(jsonResponse);
				}

			}
			catch (Exception ex)
			{
				var myObject = new { status = "no", message = ex.Message };
				var jsonResponse = JsonConvert.SerializeObject(myObject);
				await context.Response.WriteAsync(jsonResponse);
			}

		}

		private Boolean AttachUserToContext(HttpContext context, string token)
		{
			// Thực hiện giải mã token và lưu thông tin vào HttpContext
			// Ví dụ sử dụng JWT để giải mã token
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

			try
			{
				var claims = tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					// Đặt ClockSkew bằng TimeSpan.Zero để bỏ qua khoảng thời gian cho phép
					ClockSkew = TimeSpan.Zero
				}, out SecurityToken validatedToken);

				var jwtToken = (JwtSecurityToken)validatedToken;
				var userId = jwtToken.Claims.First(x => x.Type == "IdUser").Value;
				var email = jwtToken.Claims.First(x => x.Type == "Email").Value;

				// Đính kèm thông tin người dùng vào context
				context.Items["IdUser"] = userId;
				context.Items["Email"] = email;

				return true;
			}
			catch
			{
				return false;
			}
		}

		public static string GetSha256Hash(string input)
		{
			using (SHA256 sha256 = SHA256.Create())
			{
				byte[] inputBytes = Encoding.ASCII.GetBytes(input);
				byte[] hashBytes = sha256.ComputeHash(inputBytes);
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < hashBytes.Length; i++)
				{
					sb.Append(hashBytes[i].ToString("X2"));
				}
				return sb.ToString();
			}
		}

		public static bool VerifyPassword(string inputPassword, string storedHashedPassword)
		{
			string inputHashedPassword = GetSha256Hash(inputPassword);
			return StringComparer.OrdinalIgnoreCase.Compare(inputHashedPassword, storedHashedPassword) == 0;
		}
	}
}
