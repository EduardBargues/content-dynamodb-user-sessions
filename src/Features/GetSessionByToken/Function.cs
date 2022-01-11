using System;
using System.Net;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System.Text.Json;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace GetSessionByToken
{
    public class Function
    {
        private readonly IRepository _db;

        public Function() : this(new Repository(Configuration.DynamoDbTableName))
        { }
        public Function(IRepository db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<APIGatewayProxyResponse> Handler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var token = request.PathParameters["sessionId"];
            var session = await _db.GetSessionByToken(token);

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.Created,
                Body = JsonSerializer.Serialize(session)
            };
        }
    }
}