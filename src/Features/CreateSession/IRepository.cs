using System.Threading.Tasks;

namespace CreateSession
{
    public interface IRepository
    {
        Task<string> CreateSession(string userName);
    }
}