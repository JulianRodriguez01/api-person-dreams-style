using ApiPersons.Models;
using ApiPersons.Tests;
using ApiPersons.Utilities;
using Dapper;
using MySqlConnector;
using System.Security.Policy;
using System.Xml.Linq;

namespace ApiPersons.Repositories
{
    public class UserRepository : IUserRepository
    {
        public const string SQL_GET_USERS = "CALL get_users()";
        public const string SQL_GET_USER = "CALL search_user(@document_number)";
        public const string SQL_ADD_USERS = "CALL insert_user(@name_user, @lastname_user, @document_number, @document_type, @person_status, @email, @password)";
        public const string SQL_UPDATE_USERS = "CALL update_user(@name_user, @lastname_user, @document_number, @document_type, @person_status, @email, @password)";
        public const string SQL_DELETE_USERS = "CALL delete_user(@document_number)";
        public const string SQL_GET_USER_BY_CREDENTIALS = "CALL validate_credential(@email, @password)";
        public const string SQL_GET_USER_EMAIL = "CALL get_user_email(@email)";
        public const string SQL_UPDATE_TEMPORAL_TOKEN = "CALL set_token(@email, @temporal_token)";
        public const string SQL_UPDATE_PASSWORD_TOKEN = "CALL update_password(@email, @temporal_token, @new_password)";

        private readonly MySqlConfiguration _connectionConfiguration;
        private readonly MailHelper _mailHelper;

        public UserRepository(MySqlConfiguration connectionConfiguration) {
            _connectionConfiguration = connectionConfiguration;
            _mailHelper = new MailHelper();
        }

        protected MySqlConnection dbConnection() {
            return new MySqlConnection(_connectionConfiguration.connection);
        }

        public async Task<IEnumerable<User>> getListUsers()
        {
            var db = dbConnection();            
            return await db.QueryAsync<User>(@SQL_GET_USERS, new { });
        }

        public async Task<User> getUser(string document_number)
        {
            var db = dbConnection();
            return await db.QueryFirstAsync<User>(@SQL_GET_USER, new { document_number = document_number });
        }

        public async Task<bool> addUser(User user)
        {
            var db = dbConnection();
            var result = await db.ExecuteAsync(@SQL_ADD_USERS, new { user.name_user, user.lastname_user, user.document_number, user.document_type, user.person_status, user.email, user.password });
            return result > 0;
        }

        public async Task<bool> removeUser(User user)
        {
            var db = dbConnection();
            var result = await db.ExecuteAsync(@SQL_DELETE_USERS, new { document_number = user.document_number });
            return result > 0;
        }

        public async Task<bool> updateUser(User user)
        {
            var db = dbConnection();
            var result = await db.ExecuteAsync(@SQL_UPDATE_USERS, new {  user.name_user, user.lastname_user, user.document_type, user.document_number, user.person_status });
            return result > 0;
        }


        public async Task<User> login(string email, string password)
        {
            try
            {
                var db = dbConnection();
                var user = await db.QueryFirstAsync<User>(@SQL_GET_USER_BY_CREDENTIALS, new { email = email, password = password });
                if (user != null)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<User> getUserRecoveryAccount(string email)
        {
            var db = dbConnection();
            return await db.QueryFirstAsync<User>(SQL_GET_USER_EMAIL, new { email = email });
        }

        public async Task<User> UpdateNewPassword(string email, string token, string newPassword)
        {
            try
            {
                var db = dbConnection();
                var user = await db.QueryFirstAsync<User>(@SQL_UPDATE_PASSWORD_TOKEN, new { email = email, token = token, newPassword = newPassword });
                if (user != null)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public async Task<User> setToken(string email, string token)
        {
            var db = dbConnection();
            try
            {
                User user = await db.QueryFirstAsync<User>(SQL_GET_USER_EMAIL, new { email = email });
                if (user != null)
                {
                    await db.ExecuteAsync(SQL_UPDATE_TEMPORAL_TOKEN, new { email = email, temporal_token = token });
                    return user;
                } else
                {
                    return null;
                }                
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
