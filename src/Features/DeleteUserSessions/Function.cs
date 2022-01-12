using System;
using System.Net;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace DeleteUserSessions
{
    public class Function
    {
        private static string DynamoDbTableName
            => Environment.GetEnvironmentVariable("DYNAMODB_TABLE_NAME");
        private readonly IRepository _db;

        public Function() : this(new Repository(DynamoDbTableName))
        { }
        public Function(IRepository db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<APIGatewayProxyResponse> Handler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var userName = request.QueryStringParameters["userName"];
            await _db.DeleteSessionsByUserName(userName);

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
            };
        }
    }
}