using System.ComponentModel.DataAnnotations;

namespace SOA.Models
{
	public class UserModel
	{
		[Key]
		public int IdUser { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string? Token { get; set; }
	}
}
