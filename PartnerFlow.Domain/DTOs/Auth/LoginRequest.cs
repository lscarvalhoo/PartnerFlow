namespace PartnerFlow.Domain.DTOs.Auth;

public class LoginRequest
{
    public string Usuario { get; set; } = null!;
    public string Senha { get; set; } = null!;
}
