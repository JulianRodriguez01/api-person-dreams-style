using ApiPersons.Models;

namespace ApiPersons.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> getListUsers();
        Task<User> getUser(string documentNumber);
        Task<bool> addUser(User user);
        Task<bool> removeUser(User user);
        Task<bool> updateUser(User user);
       // Task<bool> login(string email, string password);
    }
}
