using Front.Models;
using System.Threading.Tasks;

namespace Front.Repositories
{
    public interface IUserRepository
    {
        Task<UserSession> FindSessionAsync(int sessionId);
    }
}
