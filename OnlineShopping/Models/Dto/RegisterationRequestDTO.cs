namespace OnlineShopping.WebApi.Models.Dto
{
    public class RegisterationRequestDTO
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string? Role { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImageUrl { get; set; }
    }
}
