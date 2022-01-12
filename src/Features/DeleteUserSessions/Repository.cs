using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace DeleteUserSessions
{
    public interface IRepository
    {
        Task DeleteSessionsByUserName(string userName);
    }
    public class Repository : IRepository
    {
        private const string PARTITION_KEY_NAME = "SessionToken";
        private const string USER_NAME_ATTRIBUTE_NAME = "UserName";
        private const string INDEX_NAME = "GSI_0";

        private readonly IAmazonDynamoDB _client;
        private readonly string _tableName;

        public Repository(string tableName)
        {
            _client = new AmazonDynamoDBClient();
            _tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        }

        public async Task DeleteSessionsByUserName(string userName)
        {
            var queryRequest = GetQueryRequest(userName);
            var queryResponse = await _client.QueryAsync(queryRequest);

            foreach (var item in queryResponse.Items)
            {
                var deleteRequest = GetDeleteRequest(item);
                await _client.DeleteItemAsync(deleteRequest);
            }
        }

        private DeleteItemRequest GetDeleteRequest(Dictionary<string, AttributeValue> item)
        {
            var partitionKey = new Dictionary<string, AttributeValue>();
            partitionKey.Add(PARTITION_KEY_NAME, item[PARTITION_KEY_NAME]);

            return new DeleteItemRequest(_tableName, partitionKey);
        }

        private QueryRequest GetQueryRequest(string userName)
        {
            var queryRequest = new QueryRequest(_tableName);
            queryRequest.IndexName = INDEX_NAME;
            var userNameExpressionValueSymbol = ":username";
            var userNameExpressionAttributeSymbol = "#username";
            queryRequest.KeyConditionExpression = $"{userNameExpressionAttributeSymbol} = {userNameExpressionValueSymbol}";
            queryRequest.ExpressionAttributeNames = new Dictionary<string, string>() {
                {userNameExpressionAttributeSymbol, USER_NAME_ATTRIBUTE_NAME}
            };
            queryRequest.ExpressionAttributeValues = new Dictionary<string, AttributeValue>() {
                {userNameExpressionValueSymbol, new AttributeValue(userName)}
            };

            return queryRequest;
        }
    }
}