using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace CreateSession
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

        public async Task<string> CreateSession(string userName)
        {
            var item = GetItem(userName);
            await _client.PutItemAsync(_tableName, item);

            return item[Configuration.PARTITION_KEY_NAME].S;
        }

        private Dictionary<string, AttributeValue> GetItem(string userName)
        {
            var createdAt = DateTime.UtcNow;
            var expirestAt = createdAt.AddSeconds(Configuration.SessionLifeSpanInSeconds);
            var token = $"{Guid.NewGuid()}";
            var item = new Dictionary<string, AttributeValue>(){
                {Configuration.PARTITION_KEY_NAME, new AttributeValue(token)},
                {Configuration.USER_NAME_ATTRIBUTE_NAME, new AttributeValue(userName)},
                {Configuration.CREATED_AT_ATTRIBUTE_NAME, new AttributeValue(createdAt.ToString("s"))},
                {Configuration.EXPIRES_AT_ATTRIBUTE_NAME, new AttributeValue(expirestAt.ToString("s"))},
                {Configuration.TTL_ATTRIBUTE_NAME, new AttributeValue(){N=$"{ToEpoch(expirestAt)}"}},
            };

            return item;
        }

        private long ToEpoch(DateTime date)
            => (long)(date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
    }
}