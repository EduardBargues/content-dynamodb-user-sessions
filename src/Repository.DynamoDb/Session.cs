using System;

namespace Repository.Abstractions
{
    public class Session
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public DateTime ExpirestAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
