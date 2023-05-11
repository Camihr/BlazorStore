using System;

namespace AdaStore.Shared.DTOs
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
