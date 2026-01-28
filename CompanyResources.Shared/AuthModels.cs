using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CompanyResources.Shared
{
    public class LoginDto
    {
        [Required] public string Email { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
    }

    // Klasa 2: Dane przesyłane przy rejestracji
    public class RegisterDto
    {
        [Required] public string Email { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // Domyślnie zwykły użytkownik
    }

    // Klasa 3: Odpowiedź serwera (czy się udało + token)
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
