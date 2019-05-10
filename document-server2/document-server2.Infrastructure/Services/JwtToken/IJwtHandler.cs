using System;

namespace document_server2.Infrastructure.Services.JwtToken
{
    public interface IJwtHandler
    {
        string CreateToken(string email, string role);
    }
}
