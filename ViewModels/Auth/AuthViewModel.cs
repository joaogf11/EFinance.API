namespace EFinnance.API.ViewModels.Auth
{
    public class AuthViewModel
    {
        public class RegisterModel
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Password { get; set; }
        }

        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
