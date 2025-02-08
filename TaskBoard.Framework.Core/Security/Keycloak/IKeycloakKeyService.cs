using Microsoft.IdentityModel.Tokens;

namespace TaskBoard.Framework.Core.Security.Keycloak;

interface IKeycloakKeyService
{
    JsonWebKeySet GetKeySet();
}
