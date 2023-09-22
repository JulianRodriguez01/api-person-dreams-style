namespace ApiPersons.Models
{
    public class User
    {
        public string name_user { get; set; } 
        public string lastname_user { get; set; }
        public string document_number { get; set; }

        public string document_type { get; set; }
        
        public int person_status { get; set; }

        public string email { get; set; }
        public string password { get; set; }
    }
}
