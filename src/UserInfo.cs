using System;

namespace AGW.Base
{
    public class UserInfo
    {
        public string UserCode { get; internal set; }
        public string UserName { get; internal set; }
        public bool IsAdminintrator { get; internal set; }
        public DateTime LoginTime { get; internal set; }
    }
}
