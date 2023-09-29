using ApiPersons.Models;
using Dapper;
using MySqlConnector;
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

        private readonly MySqlConfiguration _connectionConfiguration;

        public UserRepository(MySqlConfiguration connectionConfiguration) {
            _connectionConfiguration = connectionConfiguration;
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

        /*
        public  Task<bool> IUserRepository.login(string email, string password)
        {
            var db = dbConnection();
            var user = await db.QueryFirstOrDefaultAsync<User>(@SQL_GET_USER_BY_CREDENTIALS, new { username, password });
            return user;
        }*/
    }
}
