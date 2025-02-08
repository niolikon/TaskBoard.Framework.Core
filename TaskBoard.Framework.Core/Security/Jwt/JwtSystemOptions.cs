using Microsoft.IdentityModel.Protocols.Configuration;

namespace TaskBoard.Framework.Core.Security.Jwt;

public class JwtSystemOptions
{
    public JwtSystemOptions() { }

    public string Secret { get; set; } = null!;

    public string Issuer { get; set; } = null!;

    public string Audience { get; set; } = null!;

    public string SecurityAlgorithm { get; set; } = null!;

    public JwtSystemOptions Verified()
    {
        if (string.IsNullOrEmpty(Secret))
        {
            throw new InvalidConfigurationException("Missing configuration Secret");
        }

        if (string.IsNullOrEmpty(Audience))
        {
            throw new InvalidConfigurationException("Missing configuration Audience");
        }

        if (string.IsNullOrEmpty(Issuer))
        {
            throw new InvalidConfigurationException("Missing configuration Issuer");
        }

        if (string.IsNullOrEmpty(SecurityAlgorithm))
        {
            throw new InvalidConfigurationException("Missing configuration SecurityAlgorithm");
        }

        return this;
    }
}
