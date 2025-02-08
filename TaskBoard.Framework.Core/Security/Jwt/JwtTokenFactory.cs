using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskBoard.Framework.Core.Entities;
using TaskBoard.Framework.Core.Security.Authentication;

namespace TaskBoard.Framework.Core.Security.Jwt;

public class JwtTokenFactory : IJwtTokenFactory
{
    private JwtSystemOptions _config;

    public JwtTokenFactory(JwtSystemOptions config)
    {
        _config = config;
    }

    public JwtTokenDto CreateJwtToken(AuthenticatedUser user, DateTime expiration)
    {
        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        ];

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_config.Secret));
        SigningCredentials credentials = new(key, _config.SecurityAlgorithm);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _config.Issuer,
            audience: _config.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

        return new JwtTokenDto() { AccessToken = tokenAsString };
    }

    public JwtTokenDto CreateJwtToken(IOwnerEntity owner, DateTime expiration)
    {
        AuthenticatedUser authenticatedUser = AuthenticatedUserMapper.MapToAuthenticatedUser(owner);
        return CreateJwtToken(authenticatedUser, expiration);
    }
}
