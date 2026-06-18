namespace KajTest.DTOs.AuthDtos
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime ExpiresIn { get; set; }

        public List<string> Roles = new();

    }
}
