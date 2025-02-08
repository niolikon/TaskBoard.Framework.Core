namespace TaskBoard.Framework.Core.Security.Jwt;

public class JwtTokenDto
{
    public required string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
