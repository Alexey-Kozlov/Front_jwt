using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Front.Services
{
    public interface ITestService
    {
        Task<string> TestWebApi(HttpContext context);
    }
}
