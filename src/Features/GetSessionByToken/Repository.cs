using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace GetSessionByToken
{
    public interface IRepository
    {
        Task<Session> GetSessionByToken(string token);
    }
    public class Repository : IRepository
    {
        private const string PARTITION_KEY_NAME = "SessionToken";
        private const string USER_NAME_ATTRIBUTE_NAME = "UserName";
        private const string CREATED_AT_ATTRIBUTE_NAME = "CreatedAt";
        private const string EXPIRES_AT_ATTRIBUTE_NAME = "ExpiresAt";

        private readonly IAmazonDynamoDB _client;
        private readonly string _tableName;

        public Repository(string tableName)
        {
            _client = new AmazonDynamoDBClient();
            _tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        }

        public async Task<Session> GetSessionByToken(string token)
        {
            var primaryKey = new Dictionary<string, AttributeValue>(){
                {PARTITION_KEY_NAME, new AttributeValue(token)}
            };
            var response = await _client.GetItemAsync(_tableName, primaryKey);

            var session = new Session();
            session.Token = response.Item[PARTITION_KEY_NAME].S;
            session.UserName = response.Item[USER_NAME_ATTRIBUTE_NAME].S;
            session.CreatedAt = DateTime.Parse(response.Item[CREATED_AT_ATTRIBUTE_NAME].S);
            session.ExpiresAt = DateTime.Parse(response.Item[EXPIRES_AT_ATTRIBUTE_NAME].S);

            return session;
        }
    }
}