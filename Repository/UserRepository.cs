using Microsoft.EntityFrameworkCore;
using SOA.Data;
using SOA.Models;
using SOA.Repository.Interface;

namespace SOA.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly DataContext _dbContext;
		public UserRepository(DataContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<UserModel> GetUserByEmail(string email)
		{
			return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
		}
		public async Task AddUser(UserModel userModel)
		{
			_dbContext.Users.Add(userModel);
			await _dbContext.SaveChangesAsync();
		}
	}
}
