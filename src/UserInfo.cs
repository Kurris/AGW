using System;

namespace AGW.Base
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 用户代号
        /// </summary>
        public string UserCode { get; internal set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; internal set; }

        /// <summary>
        /// 是否为管理员
        /// </summary>
        public bool IsAdminintrator { get; internal set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; internal set; }
    }
}
