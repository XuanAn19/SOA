using SOA.Models;

namespace SOA.Repository.Interface
{
	public interface IUserRepository
	{
		Task AddUser(UserModel userModel);
		Task<UserModel> GetUserByEmail(string email);
	}
}
