namespace AuthService.Data.EmailModel
{
    public class EmailMessageModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Otp { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string userLastName { get; set; }
        public string RoleName { get; set; }
    }
}
