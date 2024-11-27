using SOA.DTOs;

namespace SOA.Service.Interface
{
	public interface IUserService
	{
		Task RegisterUser(UserDTO user);
		Task<object> UserLogin(string email, string password);
	}
}
