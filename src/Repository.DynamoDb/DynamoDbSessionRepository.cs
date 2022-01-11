using System;
using System.Threading.Tasks;
using Repository.Abstractions;

namespace Repository.DynamoDb
{
    public class DynamoDbSessionRepository : INoSqlRepository<Session>
    {
        public Task<Session> CreateAsync(Session session)
        {
            throw new NotImplementedException();
        }

        public Task<Session> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
