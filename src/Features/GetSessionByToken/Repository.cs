using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace GetSessionByToken
{
    public class Repository : IRepository
    {
        private readonly IAmazonDynamoDB _client;
        private readonly string _tableName;

        public Repository(string tableName)
        {
            _client = new AmazonDynamoDBClient();
            _tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        }

        public async Task<Session> GetSessionByToken(string token)
        {
            var primaryKey = GetPrimaryKey(token);
            var response = await _client.GetItemAsync(_tableName, primaryKey);
            var session = GetSession(response.Item);

            return session;
        }

        private Session GetSession(Dictionary<string, AttributeValue> item)
        {
            return new Session()
            {
                Token = item[Configuration.PARTITION_KEY_NAME].S,
                UserName = item[Configuration.USER_NAME_ATTRIBUTE_NAME].S,
                CreatedAt = DateTime.Parse(item[Configuration.CREATED_AT_ATTRIBUTE_NAME].S),
                ExpiresAt = DateTime.Parse(item[Configuration.EXPIRES_AT_ATTRIBUTE_NAME].S)
            };
        }

        private Dictionary<string, AttributeValue> GetPrimaryKey(string token)
        {
            return new Dictionary<string, AttributeValue>(){
                {Configuration.PARTITION_KEY_NAME, new AttributeValue(token)}
            };
        }
    }
}