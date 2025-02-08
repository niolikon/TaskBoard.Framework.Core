using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace TaskBoard.Framework.Core.Security.Jwt;

public class JwtSystemConfig
{
    public JwtSystemConfig(IConfigurationSection configurationSection)
    {
        Secret = configurationSection["Secret"] ?? 
            throw new InvalidConfigurationException("Missing configuration JwtConfig:Secret");
        Issuer = configurationSection["Issuer"] ??
            throw new InvalidConfigurationException("Missing configuration JwtConfig:Issuer");
        Audience = configurationSection["Audience"] ??
            throw new InvalidConfigurationException("Missing configuration JwtConfig:Audience");
        SecurityAlgorithm = configurationSection["SecurityAlgorithm"] ??
            throw new InvalidConfigurationException("Missing configuration JwtConfig:Audience");
    }

    public JwtSystemConfig() { }

    public required string Secret { get; set; }

    public required string Issuer { get; set; }

    public required string Audience { get; set; }

    public required string SecurityAlgorithm { get; set; }

    public JwtSystemConfig Verified()
    {
        if (string.IsNullOrEmpty(Secret))
        {
            throw new InvalidConfigurationException("Missing configuration JwtConfig:Secret");
        }

        if (string.IsNullOrEmpty(Audience))
        {
            throw new InvalidConfigurationException("Missing configuration JwtConfig:Audience");
        }

        if (string.IsNullOrEmpty(Issuer))
        {
            throw new InvalidConfigurationException("Missing configuration JwtConfig:Issuer");
        }

        if (string.IsNullOrEmpty(SecurityAlgorithm))
        {
            throw new InvalidConfigurationException("Missing configuration JwtConfig:SecurityAlgorithm");
        }

        return this;
    }
}
