using System.Threading.Tasks;

namespace GetSessionByToken
{
    public interface IRepository
    {
        Task<Session> GetSessionByToken(string token);
    }
}