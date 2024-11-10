namespace DTO.User
{
    public class LoginRequestModule
    {
        public string AccountName { get; set; } = null!;
        public string Password { get; set; } = null!;

        public bool Validate()
        {
            if(String.IsNullOrWhiteSpace(AccountName))
                return false;
            if (String.IsNullOrWhiteSpace(Password))
                return false;
            return true;
        }
    }
}
