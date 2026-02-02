namespace Movies.Application.AuthModels;

public class TokenRequestVm
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}