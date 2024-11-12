namespace DTO.Register
{
    public class RegisterRequestModule
    {
        public string AccountName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string rePassword { get; set; } = null!;

        public bool Validate()
        {
            if (String.IsNullOrWhiteSpace(AccountName))
                return false;
            if (String.IsNullOrWhiteSpace(Password))
                return false;
            if (String.IsNullOrWhiteSpace(rePassword))
                return false;
            return Password.Equals(rePassword);
        }
    }
}
