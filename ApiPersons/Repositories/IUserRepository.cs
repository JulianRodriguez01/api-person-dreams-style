using ApiPersons.Models;
using ApiPersons.Utilities;

namespace ApiPersons.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> getListUsers();
        Task<User> getUser(string documentNumber);
        Task<bool> addUser(User user);
        Task<bool> removeUser(User user);
        Task<bool> updateUser(User user);
        Task<User> login(string email, string password);
        Task<User> getUserRecoveryAccount(string email);
        Task<User> UpdateNewPassword(string email, string token, string newPassword);
        Task<User> setToken(string email, string token);
        
    }
}
