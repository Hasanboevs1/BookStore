using Book.API.Contexts;
using Book.API.Exceptions;
using Book.API.Interfaces;
using Book.API.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Book.API.Services;

public class UserService : IUserService
{
    private readonly Context context;
    public UserService(Context context) => this.context = context;

    public async Task<bool> DeleteAsync(long id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
            throw new CustomException(404, "user_not_found");
        context.Users.Remove(user);
        return true;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<User> GetAsync(long id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
            throw new CustomException(404, "user_not_found");
        return user;                                           
    }
}
