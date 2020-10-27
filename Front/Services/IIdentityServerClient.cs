using System;
using System.Threading.Tasks;

namespace Front_jwt.Services
{
    public interface IIdentityServerClient
    {
        Task<string> RequestClientCredentialsTokenAsync();
    }
}
