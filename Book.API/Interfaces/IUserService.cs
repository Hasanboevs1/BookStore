using Book.API.Models.Users;

namespace Book.API.Interfaces;

public interface IUserService
{
    Task<User> GetAsync(long id);
    Task<IEnumerable<User>> GetAllAsync();
    Task<bool> DeleteAsync(long id);

}
