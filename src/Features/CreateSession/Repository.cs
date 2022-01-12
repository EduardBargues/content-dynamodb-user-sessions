using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace CreateSession
{
    public interface IRepository
    {
        Task<string> CreateSession(string userName);
    }
    public class Repository : IRepository
    {
        private const string PARTITION_KEY_NAME = "SessionToken";
        private const string USER_NAME_ATTRIBUTE_NAME = "UserName";
        private const string CREATED_AT_ATTRIBUTE_NAME = "CreatedAt";
        private const string EXPIRES_AT_ATTRIBUTE_NAME = "ExpiresAt";
        private const string TTL_ATTRIBUTE_NAME = "TTL";
        private static int SessionLifeSpanInSeconds
            => int.Parse(Environment.GetEnvironmentVariable("SESSION_LIFE_SPAN_IN_SECONDS"));

        private readonly IAmazonDynamoDB _client;
        private readonly string _tableName;

        public Repository(string tableName)
        {
            _client = new AmazonDynamoDBClient();
            _tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        }

        public async Task<string> CreateSession(string userName)
        {
            var createdAt = DateTime.UtcNow;
            var expirestAt = createdAt.AddSeconds(SessionLifeSpanInSeconds);
            var expiresAtEpoch = (long)(expirestAt - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            var item = new Dictionary<string, AttributeValue>(){
                {PARTITION_KEY_NAME, new AttributeValue($"{Guid.NewGuid()}")},
                {USER_NAME_ATTRIBUTE_NAME, new AttributeValue(userName)},
                {CREATED_AT_ATTRIBUTE_NAME, new AttributeValue(createdAt.ToString("s"))},
                {EXPIRES_AT_ATTRIBUTE_NAME, new AttributeValue(expirestAt.ToString("s"))},
                {TTL_ATTRIBUTE_NAME, new AttributeValue(){N=$"{expiresAtEpoch}"}},
            };

            await _client.PutItemAsync(_tableName, item);

            return item[PARTITION_KEY_NAME].S;
        }
    }
}