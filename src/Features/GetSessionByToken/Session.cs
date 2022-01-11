using System;

namespace GetSessionByToken
{
    public class Session
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}