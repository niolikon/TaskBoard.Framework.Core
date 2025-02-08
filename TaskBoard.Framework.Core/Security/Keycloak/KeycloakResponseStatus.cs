using System;
using System.Collections.Generic;
using System.Linq;
namespace TaskBoard.Framework.Core.Security.Keycloak;

public class KeycloakResponseStatus
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
}
