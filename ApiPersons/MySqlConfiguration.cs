namespace ApiPersons
{
    public class MySqlConfiguration
    {
        public string connection { get; set; }

        public MySqlConfiguration(string connection)
        {
            this.connection = connection;
        }
    }
}
