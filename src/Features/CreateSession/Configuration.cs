using System;

namespace CreateSession
{
    public static class Configuration
    {
        internal const string PARTITION_KEY_NAME = "SessionToken";
        internal const string USER_NAME_ATTRIBUTE_NAME = "UserName";
        public const string CREATED_AT_ATTRIBUTE_NAME = "CreatedAt";
        public const string EXPIRES_AT_ATTRIBUTE_NAME = "ExpiresAt";
        public const string TTL_ATTRIBUTE_NAME = "TTL";

        private const string ENV_VAR_DYNAMODB_TABLE = "DYNAMODB_TABLE_NAME";
        public static string DynamoDbTableName => Environment.GetEnvironmentVariable(ENV_VAR_DYNAMODB_TABLE);
        private const string ENV_VAR_SESSION_LIFE_SPAN = "SESSION_LIFE_SPAN_IN_SECONDS";
        public static int SessionLifeSpanInSeconds => int.Parse(Environment.GetEnvironmentVariable(ENV_VAR_SESSION_LIFE_SPAN));
    }
}